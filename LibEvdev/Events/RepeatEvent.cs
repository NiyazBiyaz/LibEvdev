// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct RepeatEvent : IInputEvent
    {
        public EventType Type => EventType.Repeat;

        public DateTime Time { get; init; }
        public Repeat Code { get; init; }
        public int Milliseconds { get; init; }

        public RepeatEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Repeat)
                throw new ArgumentException("Type of event should be Repeat.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Repeat)raw.Code;
            Milliseconds = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Milliseconds;
    }
}
