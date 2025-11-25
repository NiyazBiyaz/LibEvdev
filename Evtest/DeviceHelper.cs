// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mono.Unix;
using LibEvdev.Devices;
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
                AnsiConsole.MarkupLine($"[bold red]Can't open device.[/] Try as [bold purple]sudo[/]");
                AnsiConsole.WriteException(e);
                throw;
            }

            return device;
        }
    }
}
