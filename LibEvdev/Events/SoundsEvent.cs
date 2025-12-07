// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct SoundsEvent : IInputEvent
    {
        public EventType Type => EventType.Sounds;

        public DateTime Time { get; init; }
        public Sounds Code { get; init; }
        public int Value { get; init; }

        public SoundsEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Sounds)
                throw new ArgumentException("Type of event should be Sounds.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Sounds)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Value;
    }
}
