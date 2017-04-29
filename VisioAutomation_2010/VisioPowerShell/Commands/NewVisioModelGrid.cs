﻿using System.Management.Automation;
using VisioAutomation.Models.Layouts.Grid;
using IVisio = Microsoft.Office.Interop.Visio;
using VA = VisioAutomation;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.New, VisioPowerShell.Commands.Nouns.VisioModelGrid)]
    public class NewVisioModelGrid : VisioCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public IVisio.Master Master { get; set; }

        [Parameter(Mandatory = true)]
        public int Columns { get; set; }

        [Parameter(Mandatory = true)]
        public int Rows { get; set; }

        [Parameter(Mandatory = true)]
        public double CellWidth = 0.5;
        
        [Parameter(Mandatory = true)]
        public double CellHeight = 0.5;

        [Parameter(Mandatory = false)]
        public double CellHorizontalSpacing = 0.25;

        [Parameter(Mandatory = false)]
        public double CellVerticalSpacing = 0.25;

        [Parameter(Mandatory = false)]
        public RowDirection RowDirection = RowDirection.BottomToTop;

        [Parameter(Mandatory = false)]
        public ColumnDirection ColumnDirection = ColumnDirection.LeftToRight;

        protected override void ProcessRecord()
        {
            var cellsize = new VA.Drawing.Size(this.CellWidth, this.CellHeight);
            var layout = new GridLayout(this.Columns, this.Rows, cellsize, this.Master);
            layout.CellSpacing = new VA.Drawing.Size(this.CellHorizontalSpacing, this.CellVerticalSpacing);
            layout.RowDirection = this.RowDirection;
            layout.ColumnDirection = this.ColumnDirection;
            this.WriteObject(layout);
        }
    }
}