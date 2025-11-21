// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    // API references: libevdev/libevdev.h
    public enum Repeat : ushort
    {
        Delay = 0x00,
        Period = 0x01,
        Max = Period,
        Count = Max + 1,
    }
}
