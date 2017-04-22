using System.Collections.Generic;
using System.Linq;
using VisioAutomation.Exceptions;
using VisioAutomation.Extensions;
using VisioAutomation.Scripting.Helpers;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Scripting.Commands
{
    public class DocumentCommands : CommandSet
    {
        internal DocumentCommands(Client client) :
            base(client)
        {

        }

        public bool HasActiveDocument
        {
            get
            {
                var app = this._client.Application.Get();

                // if there's no active document, then there can't be an active document
                if (app.ActiveDocument == null)
                {
                    this._client.WriteVerbose("HasActiveDocument: No Active Window");
                    return false;
                }

                var active_window = app.ActiveWindow;

                // If there's no active window there can't be an active document
                if (active_window == null)
                {
                    this._client.WriteVerbose("HasActiveDocument: No Active Document");
                    return false;
                }

                // Check if the window type matches that of a document
                short active_window_type = active_window.Type;
                var vis_drawing = (int)IVisio.VisWinTypes.visDrawing;
                var vis_master = (int)IVisio.VisWinTypes.visMasterWin;
                // var vis_sheet = (short)IVisio.VisWinTypes.visSheet;

                this._client.WriteVerbose("The Active Window: Type={0} & SybType={1}", active_window_type, active_window.SubType);
                if (!(active_window_type == vis_drawing || active_window_type == vis_master))
                {
                    this._client.WriteVerbose("The Active Window Type must be one of {0} or {1}", IVisio.VisWinTypes.visDrawing, IVisio.VisWinTypes.visMasterWin);
                    return false;
                }

                //  verify there is an active page
                if (app.ActivePage == null)
                {
                    this._client.WriteVerbose("HasActiveDocument: Active Page is null");

                    if (active_window.SubType == 64)
                    {
                        // 64 means master is being edited

                    }
                    else
                    {
                        this._client.WriteVerbose("HasActiveDocument: Active Page is null");
                        return false;
                    }
                }

                this._client.WriteVerbose("HasActiveDocument: Verified a drawing is available for use");

                return true;
            }
        }

        internal void AssertDocumentAvailable()
        {
            if (!this._client.Document.HasActiveDocument)
            {
                throw new VisioOperationException("No Drawing available");
            }
        }


        public void Activate(string name)
        {
            this._client.Application.AssertApplicationAvailable();

            var application = this._client.Application.Get();
            var documents = application.Documents;
            var doc = documents[name];

            this.Activate(doc);
        }

        public void Activate(IVisio.Document doc)
        {
            this._client.Application.AssertApplicationAvailable();

            var app = doc.Application;
            var cur_active_doc = app.ActiveDocument;

            // if the doc is already active do nothing
            if (doc == cur_active_doc)
            {
                // do nothing
                return;
            }

            // go through each window and check if it is assigned
            // to the target document
            var appwindows = app.Windows;
            var allwindows = appwindows.ToEnumerable();
            foreach (var curwin in allwindows)
            {
                if (curwin.Document == doc)
                {
                    // we did find one, so activate that window
                    // and then exit the method
                    curwin.Activate();
                    if (app.ActiveDocument != doc)
                    {
                        throw new InternalAssertionException("failed to activate document");
                    }
                    return;
                }
            }

            // If we get here, we couldn't find any matching window
            throw new VisioOperationException("could not find window for document");

        }

        public void Close(bool force)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var application = this._client.Application.Get();
            var doc = application.ActiveDocument;

            if (doc.Type != IVisio.VisDocumentTypes.visTypeDrawing)
            {
                this._client.WriteVerbose("Not a Drawing Window", doc.Name);
                throw new AutomationException("Not a Drawing Window");
            }

            this._client.WriteVerbose( "Closing Document Name=\"{0}\"", doc.Name);
            this._client.WriteVerbose( "Closing Document FullName=\"{0}\"", doc.FullName);

            if (force)
            {
                using (var alert = new Application.AlertResponseScope(application, Application.AlertResponseCode.No))
                {
                    doc.Close();
                }
            }
            else
            {
                doc.Close();
            }
        }

        public void CloseAllWithoutSaving()
        {
            this._client.Application.AssertApplicationAvailable();
            var application = this._client.Application.Get();
            var documents = application.Documents;
            var docs = documents.ToEnumerable().Where(doc => doc.Type == IVisio.VisDocumentTypes.visTypeDrawing).ToList();

            using (var alert = new Application.AlertResponseScope(application, Application.AlertResponseCode.No))
            {
                foreach (var doc in docs)
                {
                    this._client.WriteVerbose( "Closing Document Name=\"{0}\"", doc.Name);
                    this._client.WriteVerbose( "Closing Document FullName=\"{0}\"", doc.FullName);
                    doc.Close();
                }
            }
        }

        public IVisio.Document New()
        {
            return this.New(null);
        }

        public IVisio.Document New(string template)
        {
            this._client.Application.AssertApplicationAvailable();

            this._client.WriteVerbose("Creating Empty Drawing");
            var application = this._client.Application.Get();
            var documents = application.Documents;
            
            if (template == null)
            {
                var doc = documents.Add(string.Empty);
                return doc;
            }
            else
            {

                var doc = documents.Add(string.Empty);
                var template_doc = documents.AddEx(template, IVisio.VisMeasurementSystem.visMSDefault,
                              (int)IVisio.VisOpenSaveArgs.visAddStencil +
                              (int)IVisio.VisOpenSaveArgs.visOpenDocked,
                              0);
                return doc;
            }
        }

        public void Save()
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var application = this._client.Application.Get();
            var doc = application.ActiveDocument;
            doc.Save();
        }

        public void SaveAs(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            var application = this._client.Application.Get();
            var doc = application.ActiveDocument;
            doc.SaveAs(filename);
        }

        public IVisio.Document New(VisioAutomation.Drawing.Size size)
        {
            return this.New(size,null);
        }

        public IVisio.Document New(VisioAutomation.Drawing.Size size,string template)
        {
            this._client.Application.AssertApplicationAvailable();
            var doc = this.New(template);
            this._client.Page.SetSize(size);
            return doc;
        }

        public IVisio.Document OpenStencil(string name)
        {
            this._client.Application.AssertApplicationAvailable();
            
            if (name == null)
            {
                throw new System.ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                throw new System.ArgumentException("name");
            }

            this._client.WriteVerbose( "Loading stencil \"{0}\"", name);

            var application = this._client.Application.Get();
            var documents = application.Documents;
            var doc = documents.OpenStencil(name);

            this._client.WriteVerbose( "Finished loading stencil \"{0}\"", name);
            return doc;
        }

        public IVisio.Document Open(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            
            if (filename == null)
            {
                throw new System.ArgumentNullException(nameof(filename));
            }

            if (filename.Length == 0)
            {
                throw new System.ArgumentException("filename cannot be empty", nameof(filename));
            }

            string abs_filename = System.IO.Path.GetFullPath(filename);

            this._client.WriteVerbose( "Input filename: {0}", filename);
            this._client.WriteVerbose( "Absolute filename: {0}", abs_filename);

            if (!System.IO.File.Exists(abs_filename))
            {
                string msg = string.Format("File \"{0}\"does not exist", abs_filename);
                throw new System.ArgumentException(msg, nameof(filename));
            }

            var application = this._client.Application.Get();
            var documents = application.Documents;
            var doc = documents.Add(filename);
            return doc;
        }


        public IVisio.Document Get(string name)
        {
            this._client.Application.AssertApplicationAvailable();

            var application = this._client.Application.Get();
            var documents = application.Documents;
            var doc = documents[name];
            return doc;
        }

        public List<IVisio.Document> GetDocumentsByName(string name)
        {
            var application = this._client.Application.Get();
            var documents = application.Documents;
            if (name == null || name == "*")
            {
                // return all documents
                var docs1 = documents.ToEnumerable().ToList();
                return docs1;
            }

            // get the named document
            var docs2 = WildcardHelper.FilterObjectsByNames(documents.ToEnumerable(), new[] {name}, d => d.Name, true, WildcardHelper.FilterAction.Include).ToList();
            return docs2;
        }
    }
}