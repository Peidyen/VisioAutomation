using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;
using VisioAutomation.Extensions;

namespace TestVisioAutomation
{
    [TestClass]
    public class VisioAutomationTest
    {
        private static readonly VisioApplicationSafeReference app_ref = new VisioApplicationSafeReference();
        public readonly VA.Drawing.Size StandardPageSize = new VA.Drawing.Size(8.5, 11);
        public readonly VA.Drawing.Rectangle StandardPageSizeRect = new VA.Drawing.Rectangle(new VA.Drawing.Point(0, 0), new VA.Drawing.Size(8.5, 11));

        public IVisio.Application GetVisioApplication()
        {
            var app = VisioAutomationTest.app_ref.GetVisioApplication();
            return app;
        }

        public IVisio.Page GetNewPage()
        {
            return this.GetNewPage("");
        }

        public IVisio.Page GetNewPage(string suffix)
        {

            var frame = new System.Diagnostics.StackFrame(2);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            var page = this.GetNewPage(this.StandardPageSize);
            string pagename = string.Format("{0}{1}", name,suffix);
            page.NameU = pagename;
            return page;
        }

        public IVisio.Page GetNewPage(VA.Drawing.Size s)
        {
            var app = this.GetVisioApplication();
            var documents = app.Documents;
            if (documents.Count < 1)
            {
                documents.Add(string.Empty);
            }
            var active_document = app.ActiveDocument;
            var pages = active_document.Pages;
            var page = pages.Add();
            page.Background = 0;
            VisioAutomationTest.SetPageSize(page, s);

            return page;
        }

        public IVisio.Document GetNewDoc()
        {
            var app = this.GetVisioApplication();
            var documents = app.Documents;
            var doc = documents.Add("");

            var frame = new System.Diagnostics.StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            string pagename = string.Format("{0}{1}", name, "_doc");

            doc.Subject = pagename;
            return doc;

        }

        public VA.Scripting.Client GetScriptingClient()
        {
            var app = this.GetVisioApplication();
            // this ensures that any debug, verbose, user , etc. messages are 
            // sent to a useful place in the unit tests
            var context = new DiagnosticDebugContext(); 
            var client = new VA.Scripting.Client(app,context);
            return client;
        }

        public static VA.Drawing.Size GetSize(IVisio.Shape shape)
        {
            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_w = query.AddCell(VA.ShapeSheet.SRCConstants.Width,"Width");
            var col_h = query.AddCell(VA.ShapeSheet.SRCConstants.Height,"Height");

            var table = query.GetResults<double>(shape);
            double w = table[col_w];
            double h = table[col_h];
            var size = new VA.Drawing.Size(w, h);
            return size;
        }

        public static void ResetDoc(IVisio.Document doc)
        {
            var pages = doc.Pages;

            var target_pages = new List<IVisio.Page>(pages.Count);
            foreach (IVisio.Page p in pages)
            {
                target_pages.Add(p);
            }

            var empty_page = pages.Add();

            foreach (IVisio.Page p in target_pages)
            {
                p.Delete(1);
            }
        }

        /// Selects a series of shapes and groups them into one shape
        public static IVisio.Shape SelectAndGroup(IVisio.Window window, IEnumerable<IVisio.Shape> shapes)
        {
            if (window == null)
            {
                throw new System.ArgumentNullException("window");
            }

            if (shapes == null)
            {
                throw new System.ArgumentNullException("shapes");
            }

            var selectargs = IVisio.VisSelectArgs.visSelect;
            window.Select(shapes, selectargs);
            var selection = window.Selection;
            var group = selection.Group();
            return group;
        }

        public static void SetPageSize(IVisio.Page page, VA.Drawing.Size size)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            var page_sheet = page.PageSheet;

            var update = new VA.ShapeSheet.Update(2);
            update.SetFormula(VA.ShapeSheet.SRCConstants.PageWidth, size.Width);
            update.SetFormula(VA.ShapeSheet.SRCConstants.PageHeight, size.Height);
            update.Execute(page_sheet);
        }

        public static VA.Drawing.Size GetPageSize(IVisio.Page page)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            var query = new VA.ShapeSheet.Query.CellQuery();
            var col_height = query.AddCell(VA.ShapeSheet.SRCConstants.PageHeight, "PageHeight");
            var col_width = query.AddCell(VA.ShapeSheet.SRCConstants.PageWidth, "PageWidth");
            var results = query.GetResults<double>(page.PageSheet);
            double height = results[col_height];
            double width = results[col_width];
            var s = new VA.Drawing.Size(width, height);
            return s;
        }

        protected string GetTestResultsOutPath(string path)
        {
            return System.IO.Path.Combine(this.TestResultsOutFolder, path);
        }

        private static string tr_out_folder;

        protected string TestResultsOutFolder
        {
            get
            {
                if (VisioAutomationTest.tr_out_folder == null)
                {
                    var asm = System.Reflection.Assembly.GetExecutingAssembly();
                    VisioAutomationTest.tr_out_folder = System.IO.Path.GetDirectoryName(asm.Location);
                }
                return VisioAutomationTest.tr_out_folder;
            }
        }

    }
}