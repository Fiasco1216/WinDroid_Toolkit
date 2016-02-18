/*
 * Extract.cs - Extract Embedded Resources
 * Developed by Dan Wager - 06/22/2011
 */

using System.IO;
using System.Reflection;

namespace RegawMOD
{
    internal static class Extract
    {
        /// <summary>
        /// Extracts Multiple Embedded Resources From Calling Assembly
        /// </summary>
        /// <param name="obj">Object To Derive Default Namespace From</param>
        /// <param name="internalFolderPath">Period . Delimited path of embedded resources in assembly</param>
        /// <param name="fullPathOfItems">Exact Names Of Embedded Resources to Extract</param>
        /// <param name="outDirectory">Full Directory of Path For Extracted Resources</param>
        internal static void Resources(object obj, string outDirectory, string internalFolderPath, params string[] fullPathOfItems)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string defaultNamespace = obj.GetType().Namespace;

            foreach (string item in fullPathOfItems)
                using (Stream s = assembly.GetManifestResourceStream(defaultNamespace + "." + (internalFolderPath == null ? "" : internalFolderPath + ".") + item))
                using (BinaryReader r = new BinaryReader(s))
                using (FileStream fs = new FileStream(outDirectory + "\\" + item, FileMode.OpenOrCreate))
                using (BinaryWriter w = new BinaryWriter(fs))
                    w.Write(r.ReadBytes((int)s.Length));
        }

        /// <summary>
        /// Extracts Multiple Embedded Resources From Calling Assembly
        /// </summary>
        /// <param name="nameSpace">Namespace of calling assembly</param>
        /// <param name="outDirectory">Full Directory of Path For Extracted Resources</param>
        /// <param name="internalFolderPath">Period . Delimited path of embedded resources in assembly</param>
        /// <param name="fullPathOfItems">Exact Names Of Embedded Resources to Extract</param>
        internal static void Resources(string nameSpace, string outDirectory, string internalFolderPath, params string[] fullPathOfItems)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            foreach (string item in fullPathOfItems)
                using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFolderPath == null ? "" : internalFolderPath + ".") + item))
                using (BinaryReader r = new BinaryReader(s))
                using (FileStream fs = new FileStream(outDirectory + "\\" + item, FileMode.OpenOrCreate))
                using (BinaryWriter w = new BinaryWriter(fs))
                    w.Write(r.ReadBytes((int)s.Length));
        }
    }
}