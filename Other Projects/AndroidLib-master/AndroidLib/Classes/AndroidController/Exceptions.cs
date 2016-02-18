/*
 * Exceptions.cs - Developed by Dan Wager for AndroidLib.dll
 */

using System;

namespace RegawMOD.Android
{
    /// <summary>
    /// Thrown when a root shell command is executed on a device without root
    /// </summary>
    /// <remarks>Only created and called internally</remarks>
    public class DeviceHasNoRootException : Exception
    {
        internal DeviceHasNoRootException()
        {
        }
    }
}