// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Events
{
    public interface IInputEvent
    {
        public EventType Type { get; }

        public DateTime Time { get; }

        public ushort RawCode { get; }

        public int RawValue { get; }

        public InputEventRaw ToRaw() => new(new TimeValue(Time.Second, Time.Microsecond), Type, RawCode, RawValue);

        public static IInputEvent FromRaw(InputEventRaw raw)
        {
            switch (raw.Type)
            {
                case EventType.Synchronization:
                    return new SyncEvent(raw);
                case EventType.Key:
                    return new KeyEvent(raw);
                case EventType.Relative:
                    return new RelativeEvent(raw);
                case EventType.Absolute:
                    return new AbsoluteEvent(raw);
                case EventType.Miscellaneous:
                    return new MiscEvent(raw);
                case EventType.Switch:
                    return new SwitchEvent(raw);
                case EventType.Led:
                    return new LedEvent(raw);
                case EventType.Sounds:
                    return new SoundsEvent(raw);
                case EventType.Repeat:
                    if (raw.Code > (ushort)Repeat.REP_CNT)
                        return new RepeatEvent(raw);
                    else
                        return new TimerEvent(raw);
                default:
                    throw new ArgumentOutOfRangeException(nameof(raw), "Event type does not match with supported event types.");
            }
        }
    }
}
