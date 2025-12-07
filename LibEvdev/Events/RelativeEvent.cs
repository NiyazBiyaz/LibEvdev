// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct RelativeEvent : IInputEvent
    {
        public EventType Type => EventType.Relative;

        public DateTime Time { get; init; }
        public Relative Axis { get; init; }
        public int Step { get; init; }

        public RelativeEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Relative)
                throw new ArgumentException("Type of event should be Relative.");

            Time = raw.TimeValue.AsDateTime();
            Axis = (Relative)raw.Code;
            Step = raw.Value;
        }

        public ushort RawCode => (ushort)Axis;
        public int RawValue => Step;
    }
}
