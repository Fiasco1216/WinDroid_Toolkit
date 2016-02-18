using System;
using System.Collections.Generic;
using System.IO;

namespace RegawMOD.Android
{
    /// <summary>
    /// Wrapper for the AAPT Android binary
    /// </summary>
    public partial class AAPT : IDisposable
    {
        private static Dictionary<string, string> RESOURCES = new Dictionary<string, string>
        {
            {"aapt.exe", "26a35ee028ed08d7ad0d18ffb6bb587a"}
        };

        private string resDir;

        /// <summary>
        /// Initializes a new instance of the <c>AAPT</c> class
        /// </summary>
        public AAPT()
        {
            ResourceFolderManager.Register("AAPT");
            this.resDir = ResourceFolderManager.GetRegisteredFolderPath("AAPT");

            ExtractResources(this.resDir);
        }

        /// <summary>
        /// Dumps the specified Apk's badging information
        /// </summary>
        /// <param name="source">Source Apk on local machine</param>
        /// <returns><see cref="AAPT.Badging"/> object containing badging information</returns>
        public Badging DumpBadging(FileInfo source)
        {
            if (!source.Exists)
                throw new FileNotFoundException();

            return new Badging(source, Command.RunProcessReturnOutput(Path.Combine(this.resDir, "aapt.exe"), "dump badging \"" + source.FullName + "\"", true, Command.DEFAULT_TIMEOUT));
        }

        private void ExtractResources(string path)
        {
            string[] res = new string[RESOURCES.Count];
            RESOURCES.Keys.CopyTo(res, 0);

            Extract.Resources("RegawMOD.Android", path, "Resources.AAPT", res);
        }

        /// <summary>
        /// Call to free up resources after use of <c>AAPT</c>
        /// </summary>
        public void Dispose()
        {
            ResourceFolderManager.Unregister("AAPT");
        }
    }
}