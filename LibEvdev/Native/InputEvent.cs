// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    /// <summary>
    /// Input event structure from <see href="linux/input.h"/>.
    /// </summary>
    /// <param name="TimeValue">Time of event receiving.</param>
    /// <param name="Type">Event type.</param>
    /// <param name="Code">Event code.</param>
    /// <param name="Value">Event value.</param>
    [StructLayout(LayoutKind.Sequential)]
    public readonly record struct InputEvent(TimeValue TimeValue, EventType Type, ushort Code, int Value)
    {
        public InputEvent(ushort type, ushort code, int value)
            : this(default, (EventType)type, code, value)
        {
        }

        public InputEvent(EventType type, ushort code, int value)
            : this(default, type, code, value)
        {
        }

        public InputEvent(EventType type, RelativeAxis code, int value)
            : this(default, type, (ushort)code, value)
        {
        }

        public InputEvent(EventType type, Key code, int value)
            : this(default, type, (ushort)code, value)
        {
        }
    }
}
