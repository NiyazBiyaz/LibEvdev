// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;

namespace Evtest
{
    public static class DeviceHelper
    {
        public static IReadOnlyDevice OpenReadOnly(string path)
        {
            IReadOnlyDevice device = new ReadOnlyDevice(path);

            return device;
        }
    }
}
