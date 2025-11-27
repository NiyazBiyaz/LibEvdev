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

        public void PressButton(Key key)
        {
            if (!buttons.Contains(key))
                return;

            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, key, 1));
        }

        public void ReleaseButton(Key key)
        {
            if (!buttons.Contains(key))
                return;

            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, key, 0));
        }

        public void Move(int x = 0, int y = 0, int wheel = 0)
        {
            if (x == 0 && y == 0 && wheel == 0)
                return;

            if (x != 0)
                WriteOnlyDevice.Write(new InputEvent(EventType.Relative, RelativeAxis.X, x));
            if (x != 0)
                WriteOnlyDevice.Write(new InputEvent(EventType.Relative, RelativeAxis.Y, y));
            if (wheel != 0)
                WriteOnlyDevice.Write(new InputEvent(EventType.Relative, RelativeAxis.Wheel, wheel));

            WriteOnlyDevice.Flush();
        }
    }
}
