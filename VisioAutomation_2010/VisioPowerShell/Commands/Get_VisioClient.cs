using System.Management.Automation;

namespace VisioPowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, VisioPowerShell.Commands.Nouns.VisioClient)]
    public class Get_VisioClient : VisioCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client);
        }
    }
}