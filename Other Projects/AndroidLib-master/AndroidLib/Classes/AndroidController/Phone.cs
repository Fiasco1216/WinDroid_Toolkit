/*
 * Phone.cs - Developed by Dan Wager for AndroidLib.dll
 */

namespace RegawMOD.Android
{
    /// <summary>
    /// Controls radio options on an Android device
    /// </summary>
    public class Phone
    {
        private Device device;

        internal Phone(Device device)
        {
            this.device = device;
        }

        /// <summary>
        /// Calls a phone number on the Android device
        /// </summary>
        /// <param name="phoneNumber">Phone number to call</param>
        public void CallPhoneNumber(string phoneNumber)
        {
            if (this.device.State != DeviceState.ONLINE)
                return;

            AdbCommand adbCmd = Adb.FormAdbShellCommand(this.device, false, "service", "call", "phone", "2", "s16", phoneNumber);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
            adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.BACK);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
        }

        /// <summary>
        /// Dials (does not call) a phone number on the Android device
        /// </summary>
        /// <param name="phoneNumber">Phone number to dial</param>
        public void DialPhoneNumber(string phoneNumber)
        {
            if (this.device.State != DeviceState.ONLINE)
                return;

            AdbCommand adbCmd = Adb.FormAdbShellCommand(this.device, false, "service", "call", "phone", "1", "s16", phoneNumber);
            Adb.ExecuteAdbCommandNoReturn(adbCmd);
        }

        //public void SendSMS(string phoneNumber, string messageContents)
        //{
        //    throw new NotImplementedException();

        //    try { this.device.Processes.KillProcess(this.device.Processes["com.android.mms"]); }
        //    catch { }
        //    AdbCommand adbCmd = Adb.FormAdbShellCommand(this.device, false, "am", "start", "-a android.intent.action.SENDTO", "-d sms:" + phoneNumber, "--es sms_body \"" + messageContents + "\"", "--ez exit_on_sent true");
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.DPAD_RIGHT);
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.ENTER);
        //    adbCmd = Adb.FormAdbShellCommand(this.device, false, "input", "keyevent", (int)KeyEventCode.HOME);
        //}
    }
}