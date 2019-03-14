using System.Collections.Generic;

namespace VisioAutomation.ShapeSheet.CellGroups
{
    public class CellGroupBase
    {
        public virtual IEnumerable<SrcValuePair> SrcValuePairs { get; }

        public IEnumerable<SrcValuePair> SrcValuePairs_NewRow(short row)
        {
            foreach (var pair in this.SrcValuePairs)
            {
                var new_src = pair.Src.CloneWithNewRow(row);
                var new_pair = new SrcValuePair(new_src, pair.Value);
                yield return new_pair;

            }
        }
    }
}