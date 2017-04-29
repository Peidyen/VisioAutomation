﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using VisioAutomation.ShapeSheet;
using VisioPowerShell.Models;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, VisioPowerShell.Commands.Nouns.VisioPageCell)]
    public class Get_VisioPageCell : VisioCmdlet
    {
        [Parameter(Mandatory = false, Position = 0)]
        public string[] Cells { get; set; }

        [Parameter(Mandatory = false)]
        public IVisio.Page Page { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter GetResults;

        [Parameter(Mandatory = false)]
        public ResultType ResultType = ResultType.String;

        protected override void ProcessRecord()
        {
            var cellmap = VisioScripting.Models.CellSrcDictionary.GetCellMapForPages();

            if (this.Cells == null || this.Cells.Length < 1 || this.Cells.Contains("*"))
            {
                this.Cells = cellmap.GetNames().ToArray();
            }

            Get_VisioPageCell.EnsureEnoughCellNames(this.Cells);
            var target_page = this.Page ?? this.Client.Page.Get();
            this.WriteVerbose("Valid Names: " + string.Join(",", cellmap.GetNames()));
            var query = cellmap.ToQuery(this.Cells);
            var surface = new ShapeSheetSurface(target_page);
            var target_shapeids = new[] { surface.Target.Page.PageSheet.ID };
            var dt = DataTableHelpers.QueryToDataTable(query, this.GetResults, this.ResultType, target_shapeids, surface);
            this.WriteObject(dt);
        }

        public static void EnsureEnoughCellNames(IList<string> Cells)
        {
            if (Cells == null)
            {
                throw new ArgumentNullException(nameof(Cells));
            }

            if (Cells.Count< 1)
            {
                string msg = "Must provide at least one cell name";
                throw new ArgumentException(msg,nameof(Cells));
            }
        }
    }
}