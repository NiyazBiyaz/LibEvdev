// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct AbsoluteEvent : IInputEvent
    {
        public EventType Type => EventType.Absolute;

        public DateTime Time { get; init; }
        public Absolute Code { get; init; }
        public int Value { get; init; }

        public AbsoluteEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Absolute)
                throw new ArgumentException("Type of event should be Absolute.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Absolute)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Value;
    }
}
