using System.Collections.Generic;
using VisioAutomation.ShapeSheet.CellGroups;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Shapes
{
    public class ControlCells : ShapeSheet.CellGroups.CellGroupMultiRow
    {
        public ShapeSheet.CellData CanGlue { get; set; }
        public ShapeSheet.CellData Tip { get; set; }
        public ShapeSheet.CellData X { get; set; }
        public ShapeSheet.CellData Y { get; set; }
        public ShapeSheet.CellData YBehavior { get; set; }
        public ShapeSheet.CellData XBehavior { get; set; }
        public ShapeSheet.CellData XDynamics { get; set; }
        public ShapeSheet.CellData YDynamics { get; set; }

        public override IEnumerable<SrcFormulaPair> SrcFormulaPairs
        {
            get
            {
                yield return this.newpair(ShapeSheet.SrcConstants.ControlCanGlue, this.CanGlue.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlTip, this.Tip.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlX, this.X.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlY, this.Y.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlYBehavior, this.YBehavior.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlXBehavior, this.XBehavior.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlXDynamics, this.XDynamics.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.ControlYDynamics, this.YDynamics.Formula);
            }
        }

        public static List<List<ControlCells>> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = ControlCells.lazy_query.Value;
            return query.GetCellGroups(page, shapeids);
        }

        public static List<ControlCells> GetCells(IVisio.Shape shape)
        {
            var query = ControlCells.lazy_query.Value;
            return query.GetCellGroups(shape);
        }

        private static readonly System.Lazy<ControlCellsReader> lazy_query = new System.Lazy<ControlCellsReader>();
    }
}