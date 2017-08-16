namespace VisioAutomation_Tests
{
    public class PowerShellTestsSession : System.IDisposable
    {
        // This class should implement IDisposable because
        // it contains disposable members

        protected System.Management.Automation.PowerShell PowerShell;
        protected System.Management.Automation.Runspaces.InitialSessionState SessionState;
        protected System.Management.Automation.Runspaces.Runspace RunSpace;
        protected System.Management.Automation.RunspaceInvoke Invoker;

        public PowerShellTestsSession()
        {
            this.SessionState = System.Management.Automation.Runspaces.InitialSessionState.CreateDefault();


            // Get path of where everything is executing so we can find the VisioPS.dll assembly
            var executing_assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var asm_path = System.IO.Path.GetDirectoryName(executing_assembly.GetName().CodeBase);
            var uri = new System.Uri(asm_path);
            var visio_ps = System.IO.Path.Combine(uri.LocalPath, "VisioPS.dll");
            var modules = new[] { visio_ps };

            // Import the latest VisioPS module into the PowerShell session
            this.SessionState.ImportPSModule(modules);
            this.RunSpace = System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace(this.SessionState);
            this.RunSpace.Open();
            this.PowerShell = System.Management.Automation.PowerShell.Create();
            this.PowerShell.Runspace = this.RunSpace;
            this.Invoker = new System.Management.Automation.RunspaceInvoke(this.RunSpace);
        }

        public void CleanUp()
        {
            // Make sure we cleanup everything
            if (this.PowerShell != null)
            {
                this.PowerShell.Dispose();
                this.PowerShell = null;
            }
            if (this.Invoker != null)
            {
                this.Invoker.Dispose();
                this.Invoker = null;
            }
            if (this.RunSpace != null)
            {
                this.RunSpace.Close();
                this.RunSpace = null;
            }

            this.SessionState = null;
        }

        public void Dispose()
        {
            this.CleanUp();
        }
    }
}