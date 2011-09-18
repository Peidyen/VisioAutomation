using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.ShapeSheet
{
    public struct SRC
    {
        public short Section { get; private set; }
        public short Row { get; private set; }
        public short Cell { get; private set; }

        public SRC(
            IVisio.VisSectionIndices section,
            IVisio.VisRowIndices row,
            IVisio.VisCellIndices cell) : this((short)section,(short)row,(short)cell)
        {
        }

        public SRC(
            short section,
            short row,
            short cell) : this()
        {
            this.Section = section;
            this.Row = row;
            this.Cell = cell;
        }       
        
        public override string ToString()
        {
            return System.String.Format("({0},{1},{2})", this.Section, this.Row, this.Cell);
        }

        public SRC ForRow(short row)
        {
            return new SRC((short)this.Section, (short)row, (short)this.Cell);
        }

        public SRC ForSectionAndRow(short section, short row)
        {
            return new SRC(section , (short)row, (short)this.Cell);
        }

        public bool AreEqual(SRC other)
        {
            return ((this.Section == other.Section) && (this.Row == other.Row) && (this.Cell == other.Cell));
        }

        internal delegate VA.ShapeSheet.SRC SRCFromCellIndex(IVisio.VisCellIndices c);

        internal static SRCFromCellIndex GetSRCFactory(IVisio.VisSectionIndices sec, IVisio.VisRowIndices row)
        {
            SRCFromCellIndex new_func = (IVisio.VisCellIndices cell) => new VA.ShapeSheet.SRC(sec, row, cell);
            return new_func;
        }
    }
}