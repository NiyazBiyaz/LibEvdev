// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public interface IReadonlyDevice : IDevice
    {
        public IEnumerable<InputEvent> ReadInputEvents(int timeoutPeriod);
    }
}
