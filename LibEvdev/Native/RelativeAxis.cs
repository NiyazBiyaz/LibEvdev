// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    // API references: libevdev/libevdev.h
    public enum RelativeAxis : ushort
    {
        X = 0x00,
        Y = 0x01,
        Z = 0x02,
        RX = 0x03,
        RY = 0x04,
        RZ = 0x05,
        HorizontalWheel = 0x06,
        Dial = 0x07,
        Wheel = 0x08,
        Miscellaneous = 0x09,
        /*
        * 0x0a is reserved and should not be used in input drivers.
        * It was used by HID as REL_MISC+1 and userspace needs to detect if
        * the next REL_* event is correct or is just REL_MISC + n.
        * We define here REL_RESERVED so userspace can rely on it and detect
        * the situation described above.
        */
        Reserved = 0x0a,
        WheelHighResolution = 0x0b,
        HorizontalWheelHighResolution = 0x0c,
        Max = 0x0f,
        Count = Max + 1,
    }
}
