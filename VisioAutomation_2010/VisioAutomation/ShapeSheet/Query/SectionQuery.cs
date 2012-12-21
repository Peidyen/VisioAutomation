﻿using System.Collections.Generic;
using System.Linq;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioAutomation.ShapeSheet.Query
{
    public class SectionQuery : QueryBase
    {
        private readonly short _section;
        private SectionQuery() :
            base()
        {
        }

        public SectionQuery(short section):
            this()
        {
            this._section = section;
        }

        public SectionQuery(IVisio.VisSectionIndices section) :
            this()
        {
            this._section = (short)section;
        }

        public short Section
        {
            get { return _section; }
        }

        public VA.ShapeSheet.SRC GetSRCForRow(QueryColumn col, short row)
        {
            var src = new VA.ShapeSheet.SRC(this.Section, row, col.SRC.Cell);
            return src;
        }

        public QueryColumn AddColumn(short cell)
        {
            var col = new QueryColumn(this.Columns.Count, cell, null);
            this.AddColumn(col);
            return col;
        }

        public QueryColumn AddColumn(short cell, string name)
        {
            var col = new QueryColumn(this.Columns.Count, cell, name);
            this.AddColumn(col);
            return col;
        }

        public QueryColumn AddColumn(IVisio.VisCellIndices cell)
        {
            return AddColumn((short) cell);
        }

        public QueryColumn AddColumn(VA.ShapeSheet.SRC cell)
        {
            if (cell.Section != this.Section)
            {
                string msg = string.Format("Cell's section index {0} does not match the query's section indec {1}",
                                           cell.Section, this.Section);
                throw new VA.AutomationException(msg);
            }
            return AddColumn(cell.Cell);
        }

        public QueryColumn AddColumn(IVisio.VisCellIndices cell, string name)
        {
            return AddColumn((short)cell, name);
        }

        public QueryColumn AddColumn(VA.ShapeSheet.SRC cell, string name)
        {
            return AddColumn(cell.Cell, name);
        }

        private int [] get_group_counts(IVisio.Page page, IList<int> shapeids)
        {
            int num_shapes = shapeids.Count;
            int[] group_counts = new int[num_shapes];
            var page_shapes = page.Shapes;
            for (int i = 0; i < num_shapes; i++)
            {
                int shapeid = shapeids[i];
                var shape = page_shapes.ItemFromID[shapeid];
                group_counts[i] = shape.RowCount[this.Section];
            }
            return group_counts;
        }

        public VA.ShapeSheet.Data.Table<CellData<T>> GetFormulasAndResults<T>(IVisio.Page page, IList<int> shapeids)
        {
            var qds = this._Execute<T>(page, shapeids, true, true);
            return qds.CreateMergedTable();
        }

        public VA.ShapeSheet.Data.Table<string> GetFormulas(IVisio.Page page, IList<int> shapeids)
        {
            var qds = this._Execute<double>(page, shapeids, true, true);
            return qds.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(IVisio.Page page, IList<int> shapeids)
        {
            var qds = this._Execute<T>(page, shapeids, true, true);
            return qds.Results;
        }
        
        private VA.Internal.QueryDataSet<T> _Execute<T>(IVisio.Page page, IList<int> shapeids, bool getformulas, bool getresults)
        {
            if (page == null)
            {
                throw new System.ArgumentNullException("page");
            }

            if (shapeids == null)
            {
                throw new System.ArgumentNullException("shapeids");
            }

            var cells = Columns.Select(c => c.SRC.Cell).ToList();

            // Find out how many rows are in each shape for the given section id
            var groupcounts = this.get_group_counts(page, shapeids);
            var rowcount = groupcounts.Sum();
            int total_cells = rowcount * this.Columns.Count;
            var groups = VA.ShapeSheet.Data.TableRowGroupList.Build(shapeids, groupcounts, rowcount);

            // NOTE: Keep in mind that at this point we can find out that none of the shapes have any cells
            // and the total number of cells is zero and the number of rows is zero. So in that case
            // just return an empty dataset

            //var empty_qds = new VA.ShapeSheet.Data.QueryDataSet<T>(new string[] {}, new T[] {}, shapeids,
            //                                                      this.Columns.Count, rowcount, groups); 

            // Build the Stream
            var sidsrcs = new List<VA.ShapeSheet.SIDSRC>(total_cells);
            for (int shape_index = 0; shape_index < shapeids.Count; shape_index++)
            {
                short shapeid = (short)shapeids[shape_index];
                int num_rows = groupcounts[shape_index];

                for (short row = 0; row < num_rows; row++)
                {
                    foreach (var cell in cells)
                    {
                        var sidsrc = new VA.ShapeSheet.SIDSRC(shapeid, Section, row, cell);
                        sidsrcs.Add(sidsrc);
                    }
                }
            }
            var stream = VA.ShapeSheet.SIDSRC.ToStream(sidsrcs);

            // Retrieve Formulas
            var formulas = getformulas ? VA.ShapeSheet.ShapeSheetHelper.GetFormulasU(page, stream) : null;
            var unitcodes_for_rows = getresults && rowcount >0 ? this.CreateUnitCodeArrayForRows(rowcount) : null;
            var results = getresults ? VA.ShapeSheet.ShapeSheetHelper.GetResults<T>(page, stream, unitcodes_for_rows) : null;
            var table = new VA.Internal.QueryDataSet<T>(formulas, results, shapeids, this.Columns.Count, rowcount, groups);

            return table;
        }

        public VA.ShapeSheet.Data.Table<VA.ShapeSheet.CellData<T>> GetFormulasAndResults<T>(IVisio.Shape shape)
        {
            var table = this._Execute<T>(shape, true, true);
            return table.CreateMergedTable();
        }

        public VA.ShapeSheet.Data.Table<string> GetFormulas(IVisio.Shape shape)
        {
            var qds = this._Execute<double>(shape,true,false);
            return qds.Formulas;
        }

        public VA.ShapeSheet.Data.Table<T> GetResults<T>(IVisio.Shape shape)
        {
            var qds =this._Execute<T>(shape,false,true);
            return qds.Results;
        }

        private VA.Internal.QueryDataSet<T> _Execute<T>(IVisio.Shape shape, bool getformulas, bool getresults)
        {
            if (shape == null)
            {
                throw new System.ArgumentNullException("shape");
            }

            var cells = Columns.Select(c => c.SRC.Cell).ToList();

            int rowcount = shape.RowCount[Section];
            var groupcounts = new[] { rowcount };
            int total_cells = rowcount * Columns.Count;
            
            // NOTE that groupcounts and rowcount could be zero

            // prepare the Stream
            var srcs = new List<VA.ShapeSheet.SRC>(total_cells);
            for (short row = 0; row < rowcount; row++)
            {
                foreach (var cell in cells)
                {
                    var src = new VA.ShapeSheet.SRC(this.Section, row, cell);
                    srcs.Add(src);
                }
            }

            var stream = VA.ShapeSheet.SRC.ToStream(srcs);
            var unitcodes = getresults && rowcount > 0 ? this.CreateUnitCodeArrayForRows(rowcount) : null;
            var formulas = getformulas ? VA.ShapeSheet.ShapeSheetHelper.GetFormulasU(shape, stream) : null;
            var results = getresults ? VA.ShapeSheet.ShapeSheetHelper.GetResults<T>(shape, stream, unitcodes) : null;
            var shapeids = new[] { shape.ID };
            var groups = VA.ShapeSheet.Data.TableRowGroupList.Build(shapeids, groupcounts, rowcount);
            var qds = new VA.Internal.QueryDataSet<T>(formulas, results, shapeids, this.Columns.Count, rowcount, groups);

            return qds;
        }
    }
}