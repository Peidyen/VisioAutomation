using System.Collections.Generic;
using VisioAutomation.Shapes.Connections;
using SMA = System.Management.Automation;
using VA=VisioAutomation;
namespace VisioPS.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.Get, "VisioEdge")]
    public class Get_VisioEdge : VisioPSCmdlet
    {
        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter GetShapeObjects { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter Raw { get; set; }

        [SMA.Parameter(Mandatory = false)]
        public SMA.SwitchParameter TreatUndirectedAsBidirectional { get; set; }

        protected override void ProcessRecord()
        {
            var scriptingsession = this.ScriptingSession;

            var flag = VA.Shapes.Connections.DirectedEdgeHandling.Arrows_NoArrowsAreExcluded;

            if (this.Raw)
            {
                flag = VA.Shapes.Connections.DirectedEdgeHandling.Raw;
            }
            else
            {
                if (this.TreatUndirectedAsBidirectional)
                {
                    flag = VA.Shapes.Connections.DirectedEdgeHandling.Arrows_NoArrowsAreBidirectional;                
                }
                else
                {
                    flag = VA.Shapes.Connections.DirectedEdgeHandling.Arrows_NoArrowsAreExcluded;
                }
            }

            var edges = scriptingsession.Connection.GetDirectedEdges(flag);

            if (this.GetShapeObjects)
            {
                this.WriteObject(edges, false);
                return;
            }

            write_edges_with_shapeids(edges);
                
        }

        private void write_edges_with_shapeids(IList<ConnectorEdge> edges)
        {
            foreach (var edge in edges)
            {
                var e = new DirectedEdge();
                e.FromShapeID = edge.From.ID;
                e.ToShapeID = edge.To.ID;
                e.ConnectorID = edge.Connector.ID;
                this.WriteObject(e);
            }
        }
    }
}