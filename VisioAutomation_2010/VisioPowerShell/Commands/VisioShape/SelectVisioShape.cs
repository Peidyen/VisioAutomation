﻿using SMA = System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands.VisioShape
{
    [SMA.Cmdlet(SMA.VerbsCommon.Select, Nouns.VisioShape)]
    public class SelectVisioShape : VisioCmdlet
    {
        [SMA.Parameter(Mandatory = true, Position = 0, ParameterSetName = "SelectByShapes")]
        public IVisio.Shape[] Shapes { get; set; }
        
        [SMA.Parameter(Mandatory = true, Position = 0, ParameterSetName = "SelectByShapeIDs")]
        public int[] ShapeIDs { get; set; }
        
        [SMA.Parameter(Mandatory = true, Position=0, ParameterSetName = "SelectByOperation")] 
        public VisioScripting.Models.SelectionOperation Operation { get; set; }

        protected override void ProcessRecord()
        {
            if (this.Shapes !=null)
            {
                var selection = new VisioScripting.TargetActiveSelection();

                this.Client.Selection.SelectShapes(selection, this.Shapes);
            }
            else if (this.ShapeIDs!=null)
            {
                this.Client.Selection.SelectShapesById(this.ShapeIDs);
            }
            else
            {
                var targetwindow = new VisioScripting.TargetWindow();

                if (this.Operation == VisioScripting.Models.SelectionOperation.All)
                {
                    this.Client.Selection.SelectAllShapes(targetwindow);
                }
                else if (this.Operation == VisioScripting.Models.SelectionOperation.None)
                {
                    this.Client.Selection.SelectNone(targetwindow);
                }
                else if (this.Operation == VisioScripting.Models.SelectionOperation.Invert)
                {
                    var targetselection = new VisioScripting.TargetActiveSelection();

                    this.Client.Selection.InvertSelection(targetselection);
                }
            }
        }
    }
}