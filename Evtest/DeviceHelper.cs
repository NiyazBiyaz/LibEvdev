// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Evtest.Utils;
using LibEvdev.Devices;
using Mono.Unix;
using Spectre.Console;

namespace Evtest
{
    public static class DeviceHelper
    {
        public static IReadOnlyDevice OpenReadOnly(string path)
        {
            IReadOnlyDevice device;
            try
            {
                device = new ReadOnlyDevice(path);
            }
            catch (UnixIOException e)
            {
                if (e.Message.Contains("EACCES"))
                {
                    AnsiConsole.MarkupLine($"[bold]Can't open device: Permission denied.[/] ".DyeError() +
                                           $"Try as sudo");
                }
                AnsiConsole.WriteException(e);

                AnsiConsole.MarkupLine($"Can't open device from {path.DyePath()}.".DyeWarning());

                throw;
            }

            return device;
        }

        public static IEnumerable<IDevice> GetAllInputDevices()
        {
            var devicePaths = Directory.GetFiles("/dev/input/", "event*");

            foreach (var path in devicePaths)
            {
                if (!Device.IsValidDevicePath(path))
                    continue;

                using (var device = OpenReadOnly(path))
                    yield return device;
            }
        }
    }
}
