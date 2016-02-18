/*
 * Enums.cs - Developed by Dan Wager for AndroidLib.dll
 */

namespace RegawMOD.Android
{
    /// <summary>
    /// Specifies a FileSystem Listing
    /// </summary>
    public enum ListingType
    {
        /// <summary>
        /// Represents a File
        /// </summary>
        FILE,

        /// <summary>
        /// Represents a Directory
        /// </summary>
        DIRECTORY,

        /// <summary>
        /// Represents neither File or Directory
        /// </summary>
        NONE,

        /// <summary>
        /// Usually returned if BusyBox is not installed on device
        /// </summary>
        ERROR
    }

    /// <summary>
    /// Specifies current state of <see cref="Device"/>
    /// </summary>
    public enum DeviceState
    {
        /// <summary>
        /// <see cref="Device"/> is online
        /// </summary>
        ONLINE,

        /// <summary>
        /// <see cref="Device"/> is offline
        /// </summary>
        OFFLINE,

        /// <summary>
        /// <see cref="Device"/> is in recovery
        /// </summary>
        RECOVERY,

        /// <summary>
        /// <see cref="Device"/> is in fastboot
        /// </summary>
        FASTBOOT,

        /// <summary>
        /// <see cref="Device"/> is in sideload mode
        /// </summary>
        SIDELOAD,

        /// <summary>
        /// <see cref="Device"/> is not authorized
        /// </summary>
        UNAUTHORIZED,

        /// <summary>
        /// <see cref="Device"/> is in an unknown state
        /// </summary>
        UNKNOWN
    }

    //public enum WifiEncryption
    //{
    //    WEP,
    //    WPA,
    //    WPA2
    //}

    /// <summary>
    /// Specifies how to remount the file system
    /// </summary>
    public enum MountType
    {
        /// <summary>
        /// Read-Writable
        /// </summary>
        RW,

        /// <summary>
        /// Read-Only
        /// </summary>
        RO,

        /// <summary>
        /// Used for every <see cref="DeviceState"/> except <see cref="DeviceState.ONLINE"/>
        /// </summary>
        NONE
    }

    /// <summary>
    /// Specifies a certain partition of the connected Android device
    /// </summary>
    public enum DevicePartition
    {
        /// <summary>
        /// The boot partition
        /// </summary>
        BOOT,

        /// <summary>
        /// The system partition
        /// </summary>
        SYSTEM,

        /// <summary>
        /// The data partition
        /// </summary>
        DATA,

        /// <summary>
        /// The hboot partition (bootloader)
        /// </summary>
        HBOOT,

        /// <summary>
        /// For flashing a zip
        /// </summary>
        ZIP
    }

    /// <summary>
    /// Specifies the keyevent code to send to "adb shell input keyevent {KeyEventCode}"
    /// </summary>
    /// <remarks>No root needed</remarks>
    public enum KeyEventCode
    {
        /// <summary>
        /// The Soft Right Key
        /// </summary>
        SOFT_RIGHT = 2,

        /// <summary>
        /// The Home Button
        /// </summary>
        HOME = 3,

        /// <summary>
        /// The Back Button
        /// </summary>
        BACK = 4,

        /// <summary>
        /// The Call Button
        /// </summary>
        CALL = 5,

        /// <summary>
        /// The End Call Button
        /// </summary>
        END_CALL = 6,

        /// <summary>
        /// The Number 0 Button
        /// </summary>
        NUMBER_0 = 7,

        /// <summary>
        /// The Number 1 Button
        /// </summary>
        NUMBER_1 = 8,

        /// <summary>
        /// The Number 2 Button
        /// </summary>
        NUMBER_2 = 9,

        /// <summary>
        /// The Number 3 Button
        /// </summary>
        NUMBER_3 = 10,

        /// <summary>
        /// The Number 4 Button
        /// </summary>
        NUMBER_4 = 11,

        /// <summary>
        /// The Number 5 Button
        /// </summary>
        NUMBER_5 = 12,

        /// <summary>
        /// The Number 6 Button
        /// </summary>
        NUMBER_6 = 13,

        /// <summary>
        /// The Number 7 Button
        /// </summary>
        NUMBER_7 = 14,

        /// <summary>
        /// The Number 8 Button
        /// </summary>
        NUMBER_8 = 15,

        /// <summary>
        /// The Number 9 Button
        /// </summary>
        NUMBER_9 = 16,

        /// <summary>
        /// The Star Character (*) Button
        /// </summary>
        STAR = 17,

        /// <summary>
        /// The Pound Character (#) Button
        /// </summary>
        POUND = 18,

        /// <summary>
        /// The D-Pad Up Button
        /// </summary>
        DPAD_UP = 19,

        /// <summary>
        /// The D-Pad Down Button
        /// </summary>
        DPAD_DOWN = 20,

        /// <summary>
        /// The D-Pad Left Button
        /// </summary>
        DPAD_LEFT = 21,

        /// <summary>
        /// The D-Pad Right Button
        /// </summary>
        DPAD_RIGHT = 22,

        /// <summary>
        /// The D-Pad Center Button
        /// </summary>
        DPAD_CENTER = 23,

        /// <summary>
        /// The Volume Up Button
        /// </summary>
        VOLUME_UP = 24,

        /// <summary>
        /// The Volume Down Button
        /// </summary>
        VOLUME_DOWN = 25,

        /// <summary>
        /// The Power Button
        /// </summary>
        POWER = 26,

        /// <summary>
        /// The Camera Button
        /// </summary>
        CAMERA = 27,

        /// <summary>
        /// The Clear Button
        /// </summary>
        CLEAR = 28,

        /// <summary>
        /// The A Character Button
        /// </summary>
        A = 29,

        /// <summary>
        /// The B Character Button
        /// </summary>
        B = 30,

        /// <summary>
        /// The C Character Button
        /// </summary>
        C = 31,

        /// <summary>
        /// The D Character Button
        /// </summary>
        D = 32,

        /// <summary>
        /// The E Character Button
        /// </summary>
        E = 33,

        /// <summary>
        /// The F Character Button
        /// </summary>
        F = 34,

        /// <summary>
        /// The G Character Button
        /// </summary>
        G = 35,

        /// <summary>
        /// The H Character Button
        /// </summary>
        H = 36,

        /// <summary>
        /// The I Character Button
        /// </summary>
        I = 37,

        /// <summary>
        /// The J Character Button
        /// </summary>
        J = 38,

        /// <summary>
        /// The K Character Button
        /// </summary>
        K = 39,

        /// <summary>
        /// The L Character Button
        /// </summary>
        L = 40,

        /// <summary>
        /// The M Character Button
        /// </summary>
        M = 41,

        /// <summary>
        /// The N Character Button
        /// </summary>
        N = 42,

        /// <summary>
        /// The O Character Button
        /// </summary>
        O = 43,

        /// <summary>
        /// The P Character Button
        /// </summary>
        P = 44,

        /// <summary>
        /// The Q Character Button
        /// </summary>
        Q = 45,

        /// <summary>
        /// The R Character Button
        /// </summary>
        R = 46,

        /// <summary>
        /// The S Character Button
        /// </summary>
        S = 47,

        /// <summary>
        /// The T Character Button
        /// </summary>
        T = 48,

        /// <summary>
        /// The U Character Button
        /// </summary>
        U = 49,

        /// <summary>
        /// The V Character Button
        /// </summary>
        V = 50,

        /// <summary>
        /// The W Character Button
        /// </summary>
        W = 51,

        /// <summary>
        /// The X Character Button
        /// </summary>
        X = 52,

        /// <summary>
        /// The Y Character Button
        /// </summary>
        Y = 53,

        /// <summary>
        /// The Z Character Button
        /// </summary>
        Z = 54,

        /// <summary>
        /// The Comma Character (,) Button
        /// </summary>
        COMMA = 55,

        /// <summary>
        /// The Period Character (.) Button
        /// </summary>
        PERIOD = 56,

        /// <summary>
        /// The Left Alt Button
        /// </summary>
        ALT_LEFT = 57,

        /// <summary>
        /// The Right Alt Button
        /// </summary>
        ALT_RIGHT = 58,

        /// <summary>
        /// The Left Shift Button
        /// </summary>
        SHIFT_LEFT = 59,

        /// <summary>
        /// The Right Shift Button
        /// </summary>
        SHIFT_RIGHT = 60,

        /// <summary>
        /// The Tab Button
        /// </summary>
        TAB = 61,

        /// <summary>
        /// The Space Bar Button
        /// </summary>
        SPACE = 62,

        /// <summary>
        /// Brings Up Select Input Method Dialog
        /// </summary>
        SELECT_INPUT_METHOD = 63,

        /// <summary>
        /// Not Sure
        /// </summary>
        EXPLORER = 64,

        /// <summary>
        /// Not Sure
        /// </summary>
        ENVELOPE = 65,

        /// <summary>
        /// The Enter Button
        /// </summary>
        ENTER = 66,

        /// <summary>
        /// The Delete Button
        /// </summary>
        DELETE = 67,

        /// <summary>
        /// Not Sure
        /// </summary>
        GRAVE = 68,

        /// <summary>
        /// The Minus Button
        /// </summary>
        MINUS = 69,

        /// <summary>
        /// The Equals Character (=) Button
        /// </summary>
        EQUALS = 70,

        /// <summary>
        /// The Left Bracket Character ({) Button
        /// </summary>
        BRACKET_LEFT = 71,

        /// <summary>
        /// The Right Bracket Character (}) Button
        /// </summary>
        BRACKET_RIGHT = 72,

        /// <summary>
        /// The Backslash Character (\) Button
        /// </summary>
        BACKSLASH = 73,

        /// <summary>
        /// The Semicolon Character (;) Button
        /// </summary>
        SEMICOLON = 74,

        /// <summary>
        /// The Apostrophe Character (') Button
        /// </summary>
        APOSTROPHE = 75,

        /// <summary>
        /// The Forward Slash Character (/) Button
        /// </summary>
        FORWARD_SLASH = 76,

        /// <summary>
        /// The At Character (@) Button
        /// </summary>
        AT = 77,

        /// <summary>
        /// Number Lock
        /// </summary>
        NUM = 78,

        /// <summary>
        /// Not Sure
        /// </summary>
        HEADSET_HOOK = 79,

        /// <summary>
        /// The Focus Camera Button
        /// </summary>
        FOCUS = 80,

        /// <summary>
        /// The Plus Character (+) Button
        /// </summary>
        PLUS = 81,

        /// <summary>
        /// The Menu Button
        /// </summary>
        MENU = 82,

        /// <summary>
        /// Not Sure
        /// </summary>
        NOTIFICATION = 83,

        /// <summary>
        /// The Search Button
        /// </summary>
        SEARCH = 84,

        /// <summary>
        /// Not Sure
        /// </summary>
        TAG_LAST_KEYCODE = 85,
    }
}