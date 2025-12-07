// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct SyncEvent : IInputEvent
    {
        public EventType Type => EventType.Synchronization;

        public DateTime Time { get; init; }
        public Synchronization Code { get; init; }
        public int Value { get; init; }

        public SyncEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Synchronization)
                throw new ArgumentException("Type of event should be Synchronization.");

            Time = raw.TimeValue.AsDateTime();
            Code = (Synchronization)raw.Code;
            Value = raw.Value;
        }

        public ushort RawCode => (ushort)Code;
        public int RawValue => Value;
    }
}
