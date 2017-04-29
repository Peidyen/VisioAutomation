using System.Management.Automation;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, VisioPowerShell.Commands.Nouns.VisioDocument)]
    public class SetVisioDocument : VisioCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Name")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Doc")]
        [ValidateNotNull]
        public IVisio.Document Document  { get; set; }
        
        protected override void ProcessRecord()
        {
            if (this.Name != null)
            {
                this.Client.Document.Activate(this.Name);
            }
            else if (this.Document != null)
            {
                this.Client.Document.Activate(this.Document);
            }
        }
    }
}