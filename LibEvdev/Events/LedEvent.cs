// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct LedEvent : IInputEvent
    {
        public EventType Type => EventType.Led;

        public DateTime Time { get; init; }
        public Led Led { get; init; }
        public int Value { get; init; }

        public LedEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Led)
                throw new ArgumentException("Type of event should be Led.");

            Time = raw.TimeValue.AsDateTime();
            Led = (Led)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Led;
        public int RawValue => Value;
    }
}
