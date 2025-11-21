// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    // API references: libevdev/libevdev.h
    public enum Miscellaneous : ushort
    {
        Serial = 0x00,
        PulseLed = 0x01,
        Gesture = 0x02,
        Raw = 0x03,
        ScanCode = 0x04,
        TimeStamp = 0x05,
        Max = 0x07,
        Count = Max + 1,
    }
}
