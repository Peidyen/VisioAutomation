﻿using System;
using System.IO;
using SMA = System.Management.Automation;

namespace VisioPowerShell.Commands
{
    [SMA.CmdletAttribute(SMA.VerbsData.Export, "VisioSelectionAsXHTML")]
    public class Export_VisioSelectionAsXHTML : VisioCmdlet
    {
        [SMA.ParameterAttribute(Position = 0, Mandatory = true)]
        [SMA.ValidateNotNullOrEmptyAttribute]
        public string Filename;

        [SMA.ParameterAttribute(Mandatory = false)]
        public SMA.SwitchParameter Overwrite;

        protected override void ProcessRecord()
        {
            if (!File.Exists(this.Filename))
            {
                this.WriteVerbose("File already exists");
                if (this.Overwrite)
                {
                    File.Delete(this.Filename);
                }
                else
                {
                    string msg = $"File \"{this.Filename}\" already exists";
                    var exc = new ArgumentException(msg);
                    throw exc;
                }
            }

            string ext = Path.GetExtension(this.Filename).ToLowerInvariant();

            if (ext == ".html" || ext == ".xhtml" || ext == ".htm")
            {
                this.client.Export.SelectionToSVGXHTML(this.Filename);                
            }
            else
            {
                this.client.Export.SelectionToFile(this.Filename);
            }
        }
    }
}