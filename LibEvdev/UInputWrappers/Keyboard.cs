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

        public void Press(ushort code, bool strict = false)
        {
            if (!isValidEventCode(code))
            {
                if (strict)
                    throw new ArgumentOutOfRangeException(nameof(code));

                return;
            }

            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, code, 1));
        }

        public void Release(ushort code, bool strict = false)
        {
            if (!isValidEventCode(code))
            {
                if (strict)
                    throw new ArgumentOutOfRangeException(nameof(code));

                return;
            }
            WriteOnlyDevice.WriteFrame(new InputEvent(EventType.Key, code, 0));
        }

        private bool isValidEventCode(ushort code) => WriteOnlyDevice.HasEvent(EventType.Key, code);
    }
}
