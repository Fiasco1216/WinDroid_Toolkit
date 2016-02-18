/*
 * Java.cs - Developed by Dan Wager for AndroidLib.dll
 */

using Microsoft.Win32;
using System;
using System.IO;

namespace RegawMOD
{
    /// <summary>
    /// Contains information about the current machine's Java installation
    /// </summary>
    public static class Java
    {
        private static bool isInstalled;
        private static string installationPath;
        private static string binPath;
        private static string javaExecutable;
        private static string javacExecutable;

        /// <summary>
        /// Gets a value indicating if Java is currently installed on the local machine
        /// </summary>
        public static bool IsInstalled { get { return isInstalled; } }

        /// <summary>
        /// Gets a value indicating the installation path of Java on the local machine
        /// </summary>
        public static string InstallationPath { get { return installationPath; } }

        /// <summary>
        /// Gets a value indicating the path to Java's bin directory on the local machine
        /// </summary>
        public static string BinPath { get { return binPath; } }

        /// <summary>
        /// Gets a value indicating the path to Java.exe on the local machine
        /// </summary>
        public static string JavaExe { get { return javaExecutable; } }

        /// <summary>
        /// Gets a value indicating the path to Javac.exe on the local machine
        /// </summary>
        public static string JavacExe { get { return javacExecutable; } }

        static Java()
        {
            Update();
        }

        /// <summary>
        /// Updates the information stored in the <see cref="Java"/> class
        /// </summary>
        /// <remarks>Generally called if Java installation might have changed</remarks>
        public static void Update()
        {
            installationPath = GetJavaInstallationPath();
            isInstalled = !string.IsNullOrEmpty(installationPath);

            if (isInstalled)
            {
                binPath = Path.Combine(installationPath, "bin");
                javaExecutable = Path.Combine(installationPath, "bin\\java.exe");
                javacExecutable = Path.Combine(installationPath, "bin\\javac.exe");
            }
        }

        private static string GetJavaInstallationPath()
        {
            string environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");

            if (!string.IsNullOrEmpty(environmentPath))
                return environmentPath;

            string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";

            try
            {
                using (RegistryKey r = Registry.LocalMachine.OpenSubKey(javaKey))
                {
                    using (RegistryKey k = r.OpenSubKey(r.GetValue("CurrentVersion").ToString()))
                    {
                        environmentPath = k.GetValue("JavaHome").ToString();
                    }
                }
            }
            catch
            {
                environmentPath = null;
            }

            return environmentPath;
        }

        /// <summary>
        /// Runs the specified Jar file with the specified arguments
        /// </summary>
        /// <param name="pathToJar">Full path the Jar file on local machine</param>
        /// <param name="arguments">Arguments to pass to the Jar at runtime</param>
        /// <returns>True if successful run, false if Java is not installed or the Jar does not exist</returns>
        public static bool RunJar(string pathToJar, params string[] arguments)
        {
            if (!isInstalled)
                return false;

            if (!File.Exists(pathToJar))
                return false;

            string args = "-jar " + pathToJar;

            for (int i = 0; i < arguments.Length; i++)
                args += " " + arguments[i];

            Command.RunProcessNoReturn(javaExecutable, args, Command.DEFAULT_TIMEOUT);

            return true;
        }
    }
}