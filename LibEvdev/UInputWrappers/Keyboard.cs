// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;

namespace LibEvdev.UInputWrappers
{
    public class Keyboard : Wrapper
    {
        public Keyboard(IWriteOnlyDevice writeOnlyDevice)
            : base(writeOnlyDevice)
        {
        }

        public void PressAsync(ushort code)
        {
            if (!isValidEventCode(code))
                return;

            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, code, 1));
        }

        public void ReleaseAsync(ushort code)
        {
            if (!isValidEventCode(code))
                return;

            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, code, 0));
        }

        private bool isValidEventCode(ushort code) => WriteOnlyDevice.HasEvent(EventType.Key, code);
    }
}
