// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public readonly record struct KeyEvent : IInputEvent
    {
        public EventType Type => EventType.Key;

        public DateTime Time { get; init; }
        public Key Key { get; init; }
        public KeyAction Action { get; init; }

        public KeyEvent(InputEventRaw raw)
        {
            if (raw.Type is not EventType.Key)
                throw new ArgumentException("Type of event should be Key.");

            Time = raw.TimeValue.AsDateTime();
            Key = (Key)raw.Code;
            Action = (KeyAction)raw.Value;
        }

        public ushort RawCode => (ushort)Key;
        public int RawValue => (int)Action;
    }

    public enum KeyAction
    {
        Release,
        Press,
        Repeat
    }
}
