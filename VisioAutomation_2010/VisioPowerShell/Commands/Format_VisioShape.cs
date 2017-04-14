using System.Management.Automation;
using VisioAutomation.Models;
using VisioAutomation.Scripting.Models;
using VisioPowerShell.Models;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Format, VisioPowerShell.Nouns.VisioShape)]
    public class Format_VisioShape : VisioCmdlet
    {
        [Parameter(Mandatory = false)]
        public double NudgeX { get; set; }

        [Parameter(Mandatory = false)]
        public double NudgeY { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter DistributeHorizontal { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter DistributeVertical { get; set; }

        [Parameter(Mandatory = false)] public double Distance = -1.0;

        [Parameter(Mandatory = false)]
        public VerticalAlignment AlignVertical = VerticalAlignment.None;

        [Parameter(Mandatory = false)]
        public HorizontalAlignment AlignHorizontal = HorizontalAlignment.None;

        [Parameter(Mandatory = false)]
        public IVisio.Shape[] Shapes;

        protected override void ProcessRecord()
        {
            var targets = new TargetShapes(this.Shapes);

            if (this.NudgeX != 0.0 || this.NudgeY != 0.0)
            {
                this.Client.Arrange.Nudge(targets, this.NudgeX, this.NudgeY);                
            }

            if (this.DistributeHorizontal)
            {
                if (this.Distance < 0)
                {
                    this.Client.Distribute.DistributeOnAxis(targets, Axis.XAxis);
                }
                else
                {
                    this.Client.Distribute.DistributeOnAxis(targets, Axis.XAxis, this.Distance);
                }
            }

            if (this.DistributeVertical)
            {
                if (this.Distance < 0)
                {
                    this.Client.Distribute.DistributeOnAxis(targets, Axis.YAxis);
                }
                else
                {
                    this.Client.Distribute.DistributeOnAxis(targets, Axis.YAxis, this.Distance);
                }
            }

            if (this.AlignVertical != VerticalAlignment.None)
            {
                this.Client.Align.AlignVertical(targets, (AlignmentVertical)this.AlignVertical);
            }

            if (this.AlignHorizontal != HorizontalAlignment.None)
            {
                this.Client.Align.AlignHorizontal(targets, (AlignmentHorizontal)this.AlignHorizontal);
            }

        }
    }
}