// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    // API references: libevdev/libevdev.h
    public enum EventType : ushort
    {
        Synchronization = 0x00,
        Key = 0x01,
        Relative = 0x02,
        Absolute = 0x03,
        Miscellaneous = 0x04,
        Switch = 0x05,
        Led = 0x11,
        Sounds = 0x12,
        Repeat = 0x14,
        ForceFeedback = 0x15,
        Power = 0x16,
        ForceFeedbackStatus = 0x17,
        Maximum = 0x1f,
        Count = Maximum + 1,

        // Non-native values
        // Count starts from 100
        /// <summary>
        /// Non-native value for represent <see cref="Devices.TimerDevice"/> events.
        /// </summary>
        Timer = 100,
    }
}
