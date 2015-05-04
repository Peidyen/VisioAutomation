﻿namespace TestVisioAutomation.Common
{
    public class Helper
    {
        private readonly string OutputPath;

        public Helper(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException("name is null or empty","name");
            }

            this.OutputPath = Helper.GetOutputPathEx(name);

            this.PrepareOutputPath();
        }

        public string GetTestMethodOutputFilename(string ext)
        {
            string abs_path = this.OutputPath;
            string abs_filename = System.IO.Path.Combine(abs_path, Helper.GetMethodName(2) + System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ext);
            return abs_filename;
        }

        public string GetTestMethodOutputFilename()
        {
            string abs_path = this.OutputPath;
            string abs_filename = System.IO.Path.Combine(abs_path, Helper.GetMethodName(2));
            return abs_filename;
        }

        private void PrepareOutputPath()
        {
            if (!System.IO.Directory.Exists(this.OutputPath))
            {
                System.IO.Directory.CreateDirectory(this.OutputPath);
            }
        }

        private static string GetOutputPathEx(string name)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            return System.IO.Path.Combine(path, name);
        }

        private static string GetMethodName(int depth)
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var stackFrame = stackTrace.GetFrame(depth);
            var methodBase = stackFrame.GetMethod();

            string n = methodBase.DeclaringType.Name + "." + methodBase.Name;
            return n;
        }

    }
}