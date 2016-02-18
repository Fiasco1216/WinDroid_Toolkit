/*
 * Hasher.cs - Computes File Hashes
 * Developed by Dan Wager
 * 05/31/2011
 * Revised 10/27/2011
 */

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RegawMOD
{
    internal enum HashType
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    internal static class Hasher
    {
        private static StringBuilder builder = new StringBuilder();

        internal static string HashFile(string IN_FILE, HashType algo)
        {
            byte[] hashBytes = null;

            switch (algo)
            {
                case HashType.MD5:
                    hashBytes = MD5.Create().ComputeHash(new FileStream(IN_FILE, FileMode.Open));
                    break;

                case HashType.SHA1:
                    hashBytes = SHA1.Create().ComputeHash(new FileStream(IN_FILE, FileMode.Open));
                    break;

                case HashType.SHA256:
                    hashBytes = SHA256.Create().ComputeHash(new FileStream(IN_FILE, FileMode.Open));
                    break;

                case HashType.SHA384:
                    hashBytes = SHA384.Create().ComputeHash(new FileStream(IN_FILE, FileMode.Open));
                    break;

                case HashType.SHA512:
                    hashBytes = SHA512.Create().ComputeHash(new FileStream(IN_FILE, FileMode.Open));
                    break;
            }

            return MakeHashString(hashBytes);
        }

        internal static string HashString(string IN_STRING, HashType algo)
        {
            byte[] inStringBytes = null, hashBytes = null;

            inStringBytes = Encoding.ASCII.GetBytes(IN_STRING);

            switch (algo)
            {
                case HashType.MD5:
                    hashBytes = MD5.Create().ComputeHash(inStringBytes);
                    break;

                case HashType.SHA1:
                    hashBytes = SHA1.Create().ComputeHash(inStringBytes);
                    break;

                case HashType.SHA256:
                    hashBytes = SHA256.Create().ComputeHash(inStringBytes);
                    break;

                case HashType.SHA384:
                    hashBytes = SHA384.Create().ComputeHash(inStringBytes);
                    break;

                case HashType.SHA512:
                    hashBytes = SHA512.Create().ComputeHash(inStringBytes);
                    break;
            }

            return MakeHashString(hashBytes);
        }

        private static string MakeHashString(byte[] hash)
        {
            builder.Remove(0, builder.Length);

            foreach (byte b in hash)
                builder.Append(b.ToString("x2").ToLower());

            return builder.ToString();
        }
    }
}