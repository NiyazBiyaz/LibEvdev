// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct SwitchEvent : IInputEvent
    {
        public EventType Type => EventType.Switch;

        public DateTime Time { get; init; }
        public Switch Code { get; init; }
        public int Value { get; init; }

        public SwitchEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Switch)
                throw new ArgumentException("Type of event should be Switch.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Switch)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Value;
    }
}
