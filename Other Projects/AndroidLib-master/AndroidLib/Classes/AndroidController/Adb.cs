/*
 * Adb.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System;
using System.IO;

namespace RegawMOD.Android
{
    /// <summary>
    /// Holds formatted commands to execute through <see cref="Adb"/>
    /// </summary>
    /// <remarks><para>Can only be created with <c>Adb.FormAdbCommand()</c> or <c>Adb.FormAdbShellCommand()</c></para>
    /// <para>Can only be executed with <c>Adb.ExecuteAdbCommand()</c> or <c>Adb.ExecuteAdbCommandNoReturn()</c></para></remarks>
    public class AdbCommand
    {
        private string command;
        private int timeout;
        internal string Command { get { return this.command; } }
        internal int Timeout { get { return this.timeout; } }

        internal AdbCommand(string command)
        {
            this.command = command; this.timeout = RegawMOD.Command.DEFAULT_TIMEOUT;
        }

        /// <summary>
        /// Sets the timeout for the AdbCommand
        /// </summary>
        /// <param name="timeout">The timeout for the command in milliseconds</param>
        public AdbCommand WithTimeout(int timeout) { this.timeout = timeout; return this; }
    }

    /// <summary>
    /// Controls all commands sent to the currently running Android Debug Bridge Server
    /// </summary>
    public static class Adb
    {
        private static Object _lock = new Object();
        internal const string ADB = "adb";
        internal const string ADB_EXE = "adb.exe";
        internal const string ADB_VERSION = "1.0.32";

        /// <summary>
        /// Forms an <see cref="AdbCommand"/> that is passed to <c>Adb.ExecuteAdbCommand()</c>
        /// </summary>
        /// <remarks><para>This should only be used for non device-specific Adb commands, such as <c>adb devices</c> or <c>adb version</c>.</para>
        /// <para>Never try to start/kill the running Adb Server, as the <see cref="AndroidController"/> type handles it internally.</para></remarks>
        /// <param name="command">The command to run on the Adb Server</param>
        /// <param name="args">Any arguments that need to be sent to <paramref name="command"/></param>
        /// <returns><see cref="AdbCommand"/> that contains formatted command information</returns>
        /// <example>This example demonstrates how to create an <see cref="AdbCommand"/>
        /// <code>
        /// //This example shows how to create an AdbCommand object to execute on the running server.
        /// //The command we will create is "adb devices".
        /// //Notice how in the formation, you don't supply the prefix "adb", because the method takes care of it for you.
        ///
        /// AdbCommand adbCmd = Adb.FormAdbCommand("devices");
        ///
        /// </code>
        /// </example>
        public static AdbCommand FormAdbCommand(string command, params object[] args)
        {
            string adbCommand = (args.Length > 0) ? command + " " : command;

            for (int i = 0; i < args.Length; i++)
                adbCommand += args[i] + " ";

            return new AdbCommand(adbCommand);
        }

        /// <summary>
        /// Forms an <see cref="AdbCommand"/> that is passed to <c>Adb.ExecuteAdbCommand()</c>
        /// </summary>
        /// <remarks>This should only be used for device-specific Adb commands, such as <c>adb push</c> or <c>adb pull</c>.</remarks>
        /// <param name="device">Specific <see cref="Device"/> to run the command on</param>
        /// <param name="command">The command to run on the Adb Server</param>
        /// <param name="args">Any arguments that need to be sent to <paramref name="command"/></param>
        /// <returns><see cref="AdbCommand"/> that contains formatted command information</returns>
        /// <example>This example demonstrates how to create an <see cref="AdbCommand"/>
        /// <code>//This example shows how to create an AdbCommand object to execute on the running server.
        /// //The command we will create is "adb pull /system/app C:\".
        /// //Notice how in the formation, you don't supply the prefix "adb", because the method takes care of it for you.
        /// //This example also assumes you have a Device instance named device.
        ///
        /// AdbCommand adbCmd = Adb.FormAdbCommand(device, "pull", "/system/app", @"C:\");
        ///
        /// </code>
        /// </example>
        public static AdbCommand FormAdbCommand(Device device, string command, params object[] args)
        {
            return FormAdbCommand("-s " + device.SerialNumber + " " + command, args);
        }

        /// <summary>
        /// Forms an <see cref="AdbCommand"/> that is passed to <c>Adb.ExecuteAdbCommand()</c>
        /// </summary>
        /// <param name="device">Specific <see cref="Device"/> to run the command on</param>
        /// <param name="rootShell">Specifies if you need <paramref name="executable"/> to run in a root shell</param>
        /// <param name="executable">Executable file on <paramref name="device"/> to execute</param>
        /// <param name="args">Any arguments that need to be sent to <paramref name="executable"/></param>
        /// <returns><see cref="AdbCommand"/> that contains formatted command information</returns>
        /// <remarks>This should only be used for Adb Shell commands, such as <c>adb shell getprop</c> or <c>adb shell dumpsys</c>.</remarks>
        /// <exception cref="DeviceHasNoRootException"> if <paramref name="device"/> does not have root</exception>
        /// <example>This example demonstrates how to create an <see cref="AdbCommand"/>
        /// <code>
        /// //This example shows how to create an AdbCommand object to execute on the running server.
        /// //The command we will create is "adb shell input keyevent KeyEventCode.HOME".
        /// //Notice how in the formation, you don't supply the prefix "adb", because the method takes care of it for you.
        /// //This example also assumes you have a Device instance named device.
        ///
        /// AdbCommand adbCmd = Adb.FormAdbCommand(device, true, "input", "keyevent", (int)KeyEventCode.HOME);
        ///
        /// </code>
        /// </example>
        public static AdbCommand FormAdbShellCommand(Device device, bool rootShell, string executable, params object[] args)
        {
            if (rootShell && !device.HasRoot)
                throw new DeviceHasNoRootException();

            string shellCommand = string.Format("-s {0} shell \"", device.SerialNumber);

            if (rootShell)
                shellCommand += "su -c \"";

            shellCommand += executable;

            for (int i = 0; i < args.Length; i++)
                shellCommand += " " + args[i];

            if (rootShell)
                shellCommand += "\"";

            shellCommand += "\"";

            return new AdbCommand(shellCommand);
        }

        /// <summary>
        /// Opens Adb Shell and allows input to be typed directly to the shell.  Experimental!
        /// </summary>
        /// <remarks>Added specifically for RegawMOD CDMA Hero Rooter.  Always remember to pass "exit" as the last command or it will not return!</remarks>
        /// <param name="device">Specific <see cref="Device"/> to run the command on</param>
        /// <param name="inputLines">Lines of commands to send to shell</param>
        [Obsolete("Method is deprecated, please use ExecuteAdbShellCommandInputString(Device, int, string...) instead.")]
        public static void ExecuteAdbShellCommandInputString(Device device, params string[] inputLines)
        {
            lock (_lock)
            {
                Command.RunProcessWriteInput(AndroidController.Instance.ResourceDirectory + ADB_EXE, "shell", inputLines);
            }
        }

        /// <summary>
        /// Opens Adb Shell and allows input to be typed directly to the shell.  Experimental!
        /// </summary>
        /// <remarks>Added specifically for RegawMOD CDMA Hero Rooter.  Always remember to pass "exit" as the last command or it will not return!</remarks>
        /// <param name="device">Specific <see cref="Device"/> to run the command on</param>
        /// <param name="timeout">The timeout in milliseonds</param>
        /// <param name="inputLines">Lines of commands to send to shell</param>
        public static void ExecuteAdbShellCommandInputString(Device device, int timeout, params string[] inputLines)
        {
            lock (_lock)
            {
                Command.RunProcessWriteInput(AndroidController.Instance.ResourceDirectory + ADB_EXE, "shell", timeout, inputLines);
            }
        }

        /// <summary>
        /// Executes an <see cref="AdbCommand"/> on the running Adb Server
        /// </summary>
        /// <remarks>This should be used if you want the output of the command returned</remarks>
        /// <param name="command">Instance of <see cref="AdbCommand"/></param>
        /// <param name="forceRegular">Forces Output of stdout, not stderror if any</param>
        /// <returns>Output of <paramref name="command"/> run on server</returns>
        public static string ExecuteAdbCommand(AdbCommand command, bool forceRegular = false)
        {
            string result = "";

            lock (_lock)
            {
                result = Command.RunProcessReturnOutput(AndroidController.Instance.ResourceDirectory + ADB_EXE, command.Command, forceRegular, command.Timeout);
            }

            return result;
        }

        /// <summary>
        /// Executes an <see cref="AdbCommand"/> on the running Adb Server
        /// </summary>
        /// <remarks>This should be used if you do not want the output of the command returned.  Good for quick abd shell commands</remarks>
        /// <param name="command">Instance of <see cref="AdbCommand"/></param>
        /// <returns>Output of <paramref name="command"/> run on server</returns>
        public static void ExecuteAdbCommandNoReturn(AdbCommand command)
        {
            lock (_lock)
            {
                Command.RunProcessNoReturn(AndroidController.Instance.ResourceDirectory + ADB_EXE, command.Command, command.Timeout);
            }
        }

        /// <summary>
        /// Executes an <see cref="AdbCommand"/> on the running Adb Server
        /// </summary>
        /// <param name="command">Instance of <see cref="AdbCommand"/></param>
        /// <returns>Exit code of the process</returns>
        public static int ExecuteAdbCommandReturnExitCode(AdbCommand command)
        {
            int result = -1;

            lock (_lock)
            {
                result = Command.RunProcessReturnExitCode(AndroidController.Instance.ResourceDirectory + ADB_EXE, command.Command, command.Timeout);
            }

            return result;
        }

        /// <summary>
        /// Gets a value indicating if an Android Debug Bridge Server is currently running.
        /// </summary>
        public static bool ServerRunning
        {
            get { return Command.IsProcessRunning(Adb.ADB); }
        }

        internal static void StartServer()
        {
            ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("start-server"));
        }

        internal static void KillServer()
        {
            ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("kill-server"));
        }

        internal static string Devices()
        {
            return ExecuteAdbCommand(Adb.FormAdbCommand("devices"));
        }

        /// <summary>
        /// Forwards a port that remains until the current <see cref="AndroidController"/> instance is Disposed, or the device is unplugged
        /// </summary>
        /// <remarks>Only supports tcp: forward spec for now</remarks>
        /// <param name="device">Instance of <see cref="Device"/> to apply port forwarding to</param>
        /// <param name="localPort">Local port number</param>
        /// <param name="remotePort">Remote port number</param>
        /// <returns>True if successful, false if unsuccessful</returns>
        public static bool PortForward(Device device, int localPort, int remotePort)
        {
            bool success = false;

            AdbCommand adbCmd = Adb.FormAdbCommand(device, "forward", "tcp:" + localPort, "tcp:" + remotePort);
            using (StringReader r = new StringReader(ExecuteAdbCommand(adbCmd)))
            {
                if (r.ReadToEnd().Trim() == "")
                    success = true;
            }

            return success;
        }
    }
}