/*
 * Fastboot.cs - Developed by Dan Wager for AndroidLib.dll
 */

namespace RegawMOD.Android
{
    /// <summary>
    /// Holds formatted commands to execute through <see cref="Fastboot"/>
    /// </summary>
    /// <remarks><para>Can only be created with <c>Fastboot.FormFastbootCommand()</c></para>
    /// <para>Can only be executed with <c>Fastboot.ExecuteFastbootCommand()</c> or <c>Fastboot.ExecuteFastbootCommandNoReturn()</c></para></remarks>
    public class FastbootCommand
    {
        private string command;
        private int timeout;
        internal string Command { get { return this.command; } }
        internal int Timeout { get { return this.timeout; } }

        internal FastbootCommand(string command)
        {
            this.command = command; this.timeout = RegawMOD.Command.DEFAULT_TIMEOUT;
        }

        /// <summary>
        /// Sets the timeout for the FastbootCommand
        /// </summary>
        /// <param name="timeout">The timeout for the command in milliseconds</param>
        public FastbootCommand WithTimeout(int timeout) { this.timeout = timeout; return this; }
    }

    /// <summary>
    /// Controls all commands sent to Fastboot
    /// </summary>
    public static class Fastboot
    {
        private const string FASTBOOT_EXE = "fastboot.exe";

        internal static string Devices()
        {
            return ExecuteFastbootCommand(FormFastbootCommand("devices"));
        }

        /// <summary>
        /// Forms a <see cref="FastbootCommand"/> that is passed to <c>Fastboot.ExecuteFastbootCommand()</c>
        /// </summary>
        /// <param name="command">The fastboot command to run</param>
        /// <param name="args">Any arguments that need to be sent to <paramref name="command"/></param>
        /// <returns><see cref="FastbootCommand"/> that contains formatted command information</returns>
        /// <remarks>Should be used only for non device-specific fastboot commands such as <c>fastboot devices</c> or <c>fastboot version</c></remarks>
        /// <example>This example demonstrates how to create a non device-specific <see cref="FastbootCommand"/>
        /// <code>//This example shows how to create a non device-specific FastbootCommand
        /// //This demonstarates the fastboot command "fastboot devices"
        /// //Notice how you do not include the "fastboot" executable in the method, as the method takes care of it internally
        ///
        /// FastbootCommand fbCmd = Fastboot.FormFastbootCommand("devices");
        ///
        /// </code>
        /// </example>
        public static FastbootCommand FormFastbootCommand(string command, params string[] args)
        {
            string fbCmd = (args.Length > 0) ? command + " " : command;

            for (int i = 0; i < args.Length; i++)
                fbCmd += args[i] + " ";

            return new FastbootCommand(fbCmd);
        }

        /// <summary>
        /// Forms a <see cref="FastbootCommand"/> that is passed to <c>Fastboot.ExecuteFastbootCommand()</c>
        /// </summary>
        /// <remarks>Should be used only for device-specific fastboot commands such as <c>fastboot reboot</c> or <c>fastboot getvar all</c></remarks>
        /// <param name="device">Specific <see cref="Device"/> to run the comand on</param>
        /// <param name="command">The command to run on fastboot</param>
        /// <param name="args">Any arguments that need to be sent to <paramref name="command"/></param>
        /// <returns><see cref="FastbootCommand"/> that contains formatted command information</returns>
        /// <example>This example demonstrates how to create a non device-specific <see cref="FastbootCommand"/>
        /// <code>//This example shows how to create a device-specific FastbootCommand
        /// //This demonstarates the fastboot command "fastboot flash zip C:\rom.zip"
        /// //Notice how you do not include the "fastboot" executable in the method, as the method takes care of it internally
        /// //This example also assumes there is an instance of Device named device
        ///
        /// FastbootComand fbCmd = Fastboot.FormFastbootCommand(device, "flash", @"zip C:\rom.zip");
        ///
        /// </code>
        /// </example>
        public static FastbootCommand FormFastbootCommand(Device device, string command, params string[] args)
        {
            string fbCmd = "-s " + device.SerialNumber + " ";

            fbCmd += (args.Length > 0) ? command + " " : command;

            for (int i = 0; i < args.Length; i++)
                fbCmd += args[i] + " ";

            return new FastbootCommand(fbCmd);
        }

        /// <summary>
        /// Executes a <see cref="FastbootCommand"/>
        /// </summary>
        /// <param name="command">Instance of <see cref="FastbootCommand"/></param>
        /// <returns>Output of <paramref name="command"/> run in fastboot</returns>
        public static string ExecuteFastbootCommand(FastbootCommand command)
        {
            return Command.RunProcessReturnOutput(AndroidController.Instance.ResourceDirectory + FASTBOOT_EXE, command.Command, command.Timeout);
        }

        /// <summary>
        /// Executes a <see cref="FastbootCommand"/>
        /// </summary>
        /// <remarks>Should be used if you do not want the output of the command; good for quick fastboot commands</remarks>
        /// <param name="command">Instance of <see cref="FastbootCommand"/></param>
        public static void ExecuteFastbootCommandNoReturn(FastbootCommand command)
        {
            Command.RunProcessNoReturn(AndroidController.Instance.ResourceDirectory + FASTBOOT_EXE, command.Command, command.Timeout);
        }
    }
}