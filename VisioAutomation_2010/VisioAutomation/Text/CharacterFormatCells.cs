using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Text
{
    public class CharacterFormatCells : ShapeSheet.CellGroups.CellGroupMultiRow
    {
        public ShapeSheet.CellData Color { get; set; }
        public ShapeSheet.CellData Font { get; set; }
        public ShapeSheet.CellData Size { get; set; }
        public ShapeSheet.CellData Style { get; set; }
        public ShapeSheet.CellData ColorTransparency { get; set; }
        public ShapeSheet.CellData AsianFont { get; set; }
        public ShapeSheet.CellData Case { get; set; }
        public ShapeSheet.CellData ComplexScriptFont { get; set; }
        public ShapeSheet.CellData ComplexScriptSize { get; set; }
        public ShapeSheet.CellData DoubleStrikethrough { get; set; }
        public ShapeSheet.CellData DoubleUnderline { get; set; }
        public ShapeSheet.CellData LangID { get; set; }
        public ShapeSheet.CellData Locale { get; set; }
        public ShapeSheet.CellData LocalizeFont { get; set; }
        public ShapeSheet.CellData Overline { get; set; }
        public ShapeSheet.CellData Perpendicular { get; set; }
        public ShapeSheet.CellData Pos { get; set; }
        public ShapeSheet.CellData RTLText { get; set; }
        public ShapeSheet.CellData FontScale { get; set; }
        public ShapeSheet.CellData Letterspace { get; set; }
        public ShapeSheet.CellData Strikethru { get; set; }
        public ShapeSheet.CellData UseVertical { get; set; }

        public override IEnumerable<VisioAutomation.ShapeSheet.CellGroups.SrcFormulaPair> SrcFormulaPairs
        {
            get
            {
                yield return this.newpair(ShapeSheet.SrcConstants.CharColor, this.Color.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharFont, this.Font.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharSize, this.Size.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharStyle, this.Style.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharColorTransparency, this.ColorTransparency.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharAsianFont, this.AsianFont.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharCase, this.Case.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharComplexScriptFont, this.ComplexScriptFont.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharComplexScriptSize, this.ComplexScriptSize.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharDoubleUnderline, this.DoubleUnderline.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharDoubleStrikethrough, this.DoubleStrikethrough.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharLangID, this.LangID.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharFontScale, this.FontScale.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharLangID, this.LangID.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharLetterspace, this.Letterspace.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharLocale, this.Locale.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharLocalizeFont, this.LocalizeFont.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharOverline, this.Overline.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharPerpendicular, this.Perpendicular.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharPos, this.Pos.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharRTLText, this.RTLText.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharStrikethru, this.Strikethru.Formula);
                yield return this.newpair(ShapeSheet.SrcConstants.CharUseVertical, this.UseVertical.Formula);
            }
        }

        public static List<List<CharacterFormatCells>> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = CharacterFormatCells.lazy_query.Value;
            return query.GetCellGroups(page, shapeids);
        }

        public static List<CharacterFormatCells> GetCells(IVisio.Shape shape)
        {
            var query = CharacterFormatCells.lazy_query.Value;
            return query.GetCellGroups(shape);
        }

        private static readonly System.Lazy<CharacterFormatCellsReader> lazy_query = new System.Lazy<CharacterFormatCellsReader>();
    }
}