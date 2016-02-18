/*
 * Command.cs - Developed by Dan Wager for AndroidLib.dll - 04/12/12
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace RegawMOD
{
    internal static class Command
    {
        /// <summary>
        /// The default timeout for commands. -1 implies infinite time
        /// </summary>
        public const int DEFAULT_TIMEOUT = -1;

        [Obsolete("Method is deprecated, please use RunProcessNoReturn(string, string, int) instead.")]
        internal static void RunProcessNoReturn(string executable, string arguments, bool waitForExit = true)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;

                p.Start();

                if (waitForExit)
                    p.WaitForExit();
            }
        }

        internal static void RunProcessNoReturn(string executable, string arguments, int timeout)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;

                p.Start();

                p.WaitForExit(timeout);
            }
        }

        internal static string RunProcessReturnOutput(string executable, string arguments, int timeout)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    return HandleOutput(p, outputWaitHandle, errorWaitHandle, timeout, false);
            }
        }

        internal static string RunProcessReturnOutput(string executable, string arguments, bool forceRegular, int timeout)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    return HandleOutput(p, outputWaitHandle, errorWaitHandle, timeout, forceRegular);
            }
        }

        private static string HandleOutput(Process p, AutoResetEvent outputWaitHandle, AutoResetEvent errorWaitHandle, int timeout, bool forceRegular)
        {
            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            p.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                    outputWaitHandle.Set();
                else
                    output.AppendLine(e.Data);
            };
            p.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null)
                    errorWaitHandle.Set();
                else
                    error.AppendLine(e.Data);
            };

            p.Start();

            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            if (p.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
            {
                string strReturn = "";

                if (error.ToString().Trim().Length.Equals(0) || forceRegular)
                    strReturn = output.ToString().Trim();
                else
                    strReturn = error.ToString().Trim();

                return strReturn;
            }
            else
            {
                // Timed out.
                return "PROCESS TIMEOUT";
            }
        }

        internal static int RunProcessReturnExitCode(string executable, string arguments, int timeout)
        {
            int exitCode;

            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = true;

                p.Start();
                p.WaitForExit(timeout);
                exitCode = p.ExitCode;
            }

            return exitCode;
        }

        [Obsolete("Method is deprecated, please use RunProcessWriteInput(string, string, int, string...) instead.")]
        internal static void RunProcessWriteInput(string executable, string arguments, params string[] input)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;

                p.StartInfo.RedirectStandardInput = true;

                p.Start();

                using (StreamWriter w = p.StandardInput)
                    for (int i = 0; i < input.Length; i++)
                        w.WriteLine(input[i]);

                p.WaitForExit();
            }
        }

        internal static void RunProcessWriteInput(string executable, string arguments, int timeout, params string[] input)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = executable;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;

                p.StartInfo.RedirectStandardInput = true;

                p.Start();

                using (StreamWriter w = p.StandardInput)
                    for (int i = 0; i < input.Length; i++)
                        w.WriteLine(input[i]);

                p.WaitForExit(timeout);
            }
        }

        internal static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
                if (p.ProcessName.ToLower().Contains(processName.ToLower()))
                    return true;

            return false;
        }

        internal static void KillProcess(string processName)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                if (p.ProcessName.ToLower().Contains(processName.ToLower()))
                {
                    p.Kill();
                    return;
                }
            }
        }
    }
}