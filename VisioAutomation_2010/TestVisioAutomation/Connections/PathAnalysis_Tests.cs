using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using VisioAutomation.Shapes.Connections;
using VA = VisioAutomation;
using VisioAutomation.Extensions;
using IVisio = Microsoft.Office.Interop.Visio;

namespace TestVisioAutomation
{
    [TestClass]
    public class PathAnalysis_Tests : VisioAutomationTest
    {
        [TestMethod]
        public void PathAnalysis_VerifyEdgesAndTransitiveClosure()
        {
            var page1 = GetNewPage();
            var connectors_stencil = page1.Application.Documents.OpenStencil("connec_u.vss");
            var connectors_masters = connectors_stencil.Masters;

            var dcm = connectors_masters["Dynamic Connector"];

            var s1 = page1.DrawRectangle(0, 0, 1, 1);
            s1.Text = "From";

            var s2 = page1.DrawRectangle(2, 0, 3, 1);
            s2.Text = "To";

            var c1 = page1.Drop(dcm, new VA.Drawing.Point(-2, -2));
            c1.Text = "Con";

            ConnectorHelper.ConnectShapes(s1, s2, c1);

            var edges_bd = PathAnalysis.GetEdges(page1,
                                                             ConnectorArrowEdgeHandling.
                                                                 TreatNoArrowEdgesAsBidirectional);
            var edges_d = PathAnalysis.GetEdges(page1,
                                                             ConnectorArrowEdgeHandling.ExcludeNoArrowEdges);

            var tcbd = PathAnalysis.GetTransitiveClosure(page1,
                                                             ConnectorArrowEdgeHandling.
                                                                 TreatNoArrowEdgesAsBidirectional);

            var tcd = PathAnalysis.GetTransitiveClosure(page1,
                                                 ConnectorArrowEdgeHandling.ExcludeNoArrowEdges);

            page1.Delete(0);
        }
    }
}