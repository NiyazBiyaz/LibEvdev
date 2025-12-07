// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct MiscEvent : IInputEvent
    {
        public EventType Type => EventType.Miscellaneous;

        public DateTime Time { get; init; }
        public Miscellaneous Code { get; init; }
        public int Value { get; init; }

        public MiscEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Miscellaneous)
                throw new ArgumentException("Type of event should be Miscellaneous.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Miscellaneous)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Value;
    }
}
