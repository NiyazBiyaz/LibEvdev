// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;

namespace LibEvdev.UInputWrappers
{
    public class Cursor : Wrapper
    {
        private readonly static Key[] buttons = [
            Key.ButtonLeft,
            Key.ButtonRight,
            Key.ButtonMiddle,
            Key.ButtonSide,
            Key.ButtonExtra,
        ];

        public Cursor(IWriteOnlyDevice writeOnlyDevice)
            : base(writeOnlyDevice)
        {
        }

        public void PressButton(Key button, bool strict = false)
        {
            if (!buttons.Contains(button))
            {
                if (strict)
                    throw new ArgumentOutOfRangeException(nameof(button));

                return;
            }

            WriteOnlyDevice.WriteFrame(new InputEventRaw(EventType.Key, (ushort)button, 1));
        }

        public void ReleaseButton(Key button, bool strict = false)
        {
            if (!buttons.Contains(button))
            {
                if (strict)
                    throw new ArgumentOutOfRangeException(nameof(button));

                return;
            }

            WriteOnlyDevice.WriteFrame(new InputEventRaw(EventType.Key, (ushort)button, 0));
        }

        public void Move(int horizontal = 0, int vertical = 0, int wheel = 0)
        {
            if (horizontal == 0 && vertical == 0 && wheel == 0)
                return;

            if (horizontal != 0)
                WriteOnlyDevice.Write(new InputEventRaw(EventType.Relative, (ushort)RelativeAxis.X, horizontal));
            if (vertical != 0)
                WriteOnlyDevice.Write(new InputEventRaw(EventType.Relative, (ushort)RelativeAxis.Y, vertical));
            if (wheel != 0)
                WriteOnlyDevice.Write(new InputEventRaw(EventType.Relative, (ushort)RelativeAxis.Wheel, wheel));

            WriteOnlyDevice.Flush();
        }
    }
}
