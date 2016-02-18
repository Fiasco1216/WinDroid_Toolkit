/*
 * Su.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System.IO;

namespace RegawMOD.Android
{
    /// <summary>
    /// Contains information about the Su binary on the Android device
    /// </summary>
    public class Su
    {
        private Device device;

        private string version;
        private bool exists;

        internal Su(Device device)
        {
            this.device = device;
            GetSuData();
        }

        internal bool Exists { get { return this.exists; } }

        /// <summary>
        /// Gets a value indicating the version of Su on the Android device
        /// </summary>
        public string Version { get { return this.version; } }

        private void GetSuData()
        {
            if (this.device.State != DeviceState.ONLINE)
            {
                this.version = null;
                this.exists = false;
                return;
            }

            AdbCommand adbCmd = Adb.FormAdbShellCommand(this.device, false, "su", "-v");
            using (StringReader r = new StringReader(Adb.ExecuteAdbCommand(adbCmd)))
            {
                string line = r.ReadLine();

                if (line.Contains("not found") || line.Contains("permission denied"))
                {
                    this.version = "-1";
                    this.exists = false;
                }
                else
                {
                    this.version = line;
                    this.exists = true;
                }
            }
        }
    }
}