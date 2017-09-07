using System.Management.Automation;
using VisioAutomation.Shapes;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, VisioPowerShell.Commands.Nouns.VisioUserDefinedCell)]
    public class SetVisioUserDefinedCell : VisioCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Value { get; set; }

        [Parameter(Mandatory = false)] 
        public string Prompt;

        [Parameter(Mandatory = false)]
        public IVisio.Shape[] Shapes; 

        protected override void ProcessRecord()
        {
            var targets = new VisioScripting.Models.TargetShapes(this.Shapes);
            var userprop = new VisioScripting.Models.UserDefinedCell(this.Name, this.Value);
            if (this.Prompt != null)
            {
                userprop.Cells.Prompt = this.Prompt;
            }

            this.Client.UserDefinedCell.Set(targets, userprop);
        }
    }
}