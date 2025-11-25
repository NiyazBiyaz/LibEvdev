// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using Serilog;
using Spectre.Console;

namespace Evtest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console(outputTemplate:
                                "{Timestamp:yyyy-MM-dd-HH:mm:ss} [{Level}][{SourceContext}] {Message:lj}{NewLine}{Exception}")
                            .MinimumLevel.Error()
                            .CreateLogger();

            if (args.Length > 0)
            {
                foreach (string pathName in args)
                {
                    if (!Device.IsValidDevicePath(pathName))
                    {
                        AnsiConsole.MarkupLine($"""
                        [red]Received path [cyan]{pathName}[/] is not valid.[/]
                        Try another like [bold]/dev/input/eventX[/].
                        """);
                        return;
                    }

                    IReadOnlyDevice device;
                    try
                    {
                        device = DeviceHelper.OpenReadOnly(pathName);
                    }
                    catch (Exception)
                    {
                        AnsiConsole.MarkupLine($"[maroon]Can't open device from [cyan]{pathName}[/].[/]");
                        continue;
                    }

                    var description = new DeviceDescription(device);

                    DeviceInfoPrinter.WriteDeviceInfo(description);

                    device.Dispose();
                    AnsiConsole.MarkupLine($"[yellow]Disposing [green]{description.Name}[/]...[/]");
                    AnsiConsole.Write("\n");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("No devices assigned to use.");
                AnsiConsole.MarkupLine("At this moment you can't choose any device there, maybe later...");
                return;
            }
        }
    }
}
