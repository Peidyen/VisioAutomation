using SMA = System.Management.Automation;

namespace VisioPowerShell.Commands
{
    [SMA.Cmdlet(SMA.VerbsCommon.Get, VisioPowerShell.Commands.Nouns.VisioLayer)]
    public class GetVisioLayer : VisioCmdlet
    {
        [SMA.Parameter(Position = 0, Mandatory = false)]
        public string Name;

        protected override void ProcessRecord()
        {
            if (VisioScripting.Helpers.WildcardHelper.NullOrStar(this.Name))
            {
                // get all
                var layers = this.Client.Layer.Get();
                this.WriteObject(layers, true);
            }
            else
            {
                // get all that match a specific name
                var layer = this.Client.Layer.Get(this.Name);
                this.WriteObject(layer);
            }
        }
    }
}