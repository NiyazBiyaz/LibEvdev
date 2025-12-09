// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using LibEvdev.Devices;

namespace LibEvdev.Events
{
    public readonly record struct TimerEvent : IInputEvent
    {
        public EventType Type => EventType.Repeat;

        public DateTime Time { get; init; }
        public TimerCode Code { get; private init; }
        public int Value { get; private init; }

        public TimerEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Timer)
                throw new ArgumentException("Type of event should be Timer.");

            Time = DateTime.Now;
            Code = (TimerCode)raw.Code;
            Value = 0;
        }

        public InputEventRaw ToRaw() => throw new NotSupportedException("This object doesn't support converting to native value.");
        public ushort RawCode => throw new NotSupportedException("This object doesn't support converting to native value.");
        public int RawValue => throw new NotSupportedException("This object doesn't support converting to native value.");
    }
}
