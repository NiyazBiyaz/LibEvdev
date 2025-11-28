// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;

namespace LibEvdev.UInputWrappers
{
    public abstract class Wrapper : IDisposable
    {
        protected readonly IWriteOnlyDevice WriteOnlyDevice;

        public DeviceDescription DeviceDescription { get; }

        protected Wrapper(IWriteOnlyDevice writeOnlyDevice)
        {
            WriteOnlyDevice = writeOnlyDevice;
            DeviceDescription = new(WriteOnlyDevice);
        }

        public void Dispose()
        {
            WriteOnlyDevice.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
