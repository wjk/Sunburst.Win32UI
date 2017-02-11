using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Win32UI.Build.Tasks
{
    public enum MsvcLinkUacPrivilegeLevel
    {
        AsInvoker,
        HighestAvailable,
        RequireAdministrator
    }

    public sealed class MsvcLink : ToolTask
    {
        [RequiredAttribute]
        public string OutputDirectory { get; set; }
        [RequiredAttribute]
        public ITaskItem[] Objects { get; set; }
        public ITaskItem[] SxsManifestFragments { get; set; }
        public ITaskItem[] SxsReferences { get; set; }
        public MsvcLinkUacPrivilegeLevel UacPrivilegeLevel { get; set; }

        public override bool Execute()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.LogWarning("Skipping CompileResourceScript task on non-Windows platform");
                return true;
            }

            bool success = base.Execute();

            if (success)
            {
                if (ExitCode != 0)
                {
                    Log.LogError("{0} failed with code {1}", GenerateFullPathToTool(), ExitCode);
                    success = false;
                }
            }

            return success;
        }

        protected override string ToolName => "link.exe";

        protected override string GenerateFullPathToTool()
        {
            return ToolPath ?? "link.exe";
        }

        protected override string GenerateCommandLineCommands()
        {
            string outputPath = Path.Combine(OutputDirectory, "Resources.dll");
            int index = 0;
            while (File.Exists(outputPath))
            {
                outputPath = Path.Combine(OutputDirectory, $"Resources.{index++}.dll");
            }

            List<string> argv = new List<string>();
            argv.Add("/nologo");
            argv.Add("/dll");
            argv.Add("/noentry");
            argv.Add($"/out:{outputPath}");
            argv.Add("/manifest:embed,id=1");
            argv.Add($"/manifestuac:{GetManifestUacString()}");
            argv.AddRange(Objects.Select(item => item.GetMetadata("FullPath")));
            argv.AddRange(SxsManifestFragments.Select(item => item.GetMetadata("FullPath")));
            argv.AddRange(SxsReferences.Select(item => GetManifestDependencyFlag(item)));

            return string.Join(" ", argv.Select(x => "\"" + x + "\""));
        }

        private string GetManifestDependencyFlag(ITaskItem item)
        {
            string name = item.ItemSpec;
            string version = item.GetMetadata("Version");
            string publicKeyToken = item.GetMetadata("PublicKeyToken");
            return $"/manifestDependency:type='win32' name='{name}' version='{version}' processorArchitecture='*' publicKeyToken='{publicKeyToken}' language='*'";
        }

        private string GetManifestUacString()
        {
            switch (UacPrivilegeLevel)
            {
                case MsvcLinkUacPrivilegeLevel.AsInvoker: return "asInvoker";
                case MsvcLinkUacPrivilegeLevel.HighestAvailable: return "highestAvailable";
                case MsvcLinkUacPrivilegeLevel.RequireAdministrator: return "requireAdministrator";
                default: throw new ArgumentException("Unrecognized UacPrivilegeLevel");
            }
        }
    }
}