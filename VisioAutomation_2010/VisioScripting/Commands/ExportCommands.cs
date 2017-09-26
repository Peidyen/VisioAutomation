using System.Linq;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;
using SXL = System.Xml.Linq;

namespace VisioScripting.Commands
{
    public class ExportCommands : CommandSet
    {
        internal ExportCommands(Client client)
            : base(client)
        {
        }

        public void PageToFile(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            if (filename == null)
            {
                throw new System.ArgumentNullException(nameof(filename));
            }

            if (!this._client.Selection.HasShapes())
            {
                this._client.WriteVerbose("No selected shapes. Not exporting.");
                return;
            }

            var old_selection = this._client.Selection.GetShapes();

            this._client.Selection.SelectNone();
            var application = this._client.Application.Get();
            var active_page = application.ActivePage;
            active_page.Export(filename);
            var active_window = application.ActiveWindow;
            active_window.Select(old_selection, IVisio.VisSelectArgs.visSelect);
        }

        public void SelectionToFile(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            if (filename == null)
            {
                throw new System.ArgumentNullException(nameof(filename));
            }

            if (!this._client.Selection.HasShapes())
            {
                this._client.WriteVerbose("No selected shapes. Not exporting.");
                return;
            }

            var selection = this._client.Selection.Get();
            selection.Export(filename);
        }

        public void PagesToFiles(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            if (filename == null)
            {
                throw new System.ArgumentNullException(nameof(filename));
            }

            var application = this._client.Application.Get();
            var old_page = application.ActivePage;
            var active_document = application.ActiveDocument;
            var active_window = application.ActiveWindow;

            var pages = active_document.Pages.ToList();
            var pbase = System.IO.Path.GetDirectoryName(filename);

            if (!System.IO.Directory.Exists(pbase))
            {
                this._client.WriteError(" Folder {0} does not exist", pbase);
                return;
            }
            var ext = System.IO.Path.GetExtension(filename);
            string fbase = System.IO.Path.GetFileNameWithoutExtension(filename);

            for (int page_index = 0; page_index < pages.Count; page_index++)
            {
                var page = pages[page_index];
                string bkgnd = "";
                if (page.Background != 0)
                {
                    bkgnd = "(Background)";
                }
                string page_filname = string.Format("{0}_{1}_{2}{3}{4}", fbase, page_index, page.Name, bkgnd, ext);

                this._client.WriteUser("file {0}", page_filname);
                page_filname = System.IO.Path.Combine(pbase, page_filname);
                active_window.Page = page;
                this._client.Selection.SelectNone();
                page.Export(page_filname);
            }
            active_window.Page = old_page;
        }

        public void SelectionToHtml(string filename)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            if (filename == null)
            {
                throw new System.ArgumentNullException(nameof(filename));
            }

            if (!this._client.Selection.HasShapes())
            {
                this._client.WriteVerbose("No selected shapes. Not exporting.");
                return;
            }

            var selection = this._client.Selection.Get();
            this.SelectionToHtml(selection, filename, s => this._client.WriteVerbose(s));
        }

        private void SelectionToHtml(IVisio.Selection selection, string filename, System.Action<string> verboselog)
        {
            this._client.Application.AssertApplicationAvailable();
            this._client.Document.AssertDocumentAvailable();

            // Save temp SVG
            string svg_filename = System.IO.Path.GetTempFileName() + "_temp.svg";
            selection.Export(svg_filename);

            // Load temp SVG
            var load_svg_timer = new System.Diagnostics.Stopwatch();
            var svg_doc = SXL.XDocument.Load(svg_filename);
            load_svg_timer.Stop();
            verboselog(string.Format("Finished SVG Loading ({0} seconds)", load_svg_timer.Elapsed.TotalSeconds));

            // Delete temp SVG
            if (System.IO.File.Exists(svg_filename))
            {
                System.IO.File.Delete(svg_filename);
            }

            verboselog("Creating XHTML with embedded SVG");

            if (System.IO.File.Exists(filename))
            {
                verboselog(string.Format("Deleting \"{0}\"", filename));
                System.IO.File.Delete(filename);
            }

            var xhtml_doc = new SXL.XDocument();
            var xhtml_root = new SXL.XElement("{http://www.w3.org/1999/xhtml}html");
            xhtml_doc.Add(xhtml_root);
            var svg_node = svg_doc.Root;
            svg_node.Remove();

            var body = new SXL.XElement("{http://www.w3.org/1999/xhtml}body");
            xhtml_root.Add(body);
            body.Add(svg_node);

            xhtml_doc.Save(filename);
            verboselog(string.Format("Done writing XHTML file \"{0}\"", filename));
        }
    }
}