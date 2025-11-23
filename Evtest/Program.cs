// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using Mono.Unix;
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
                    writeDeviceInfo(pathName);
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

        private static void writeDeviceInfo(string pathName)
        {
            if (!Device.IsValidDevicePath(pathName))
            {
                AnsiConsole.MarkupLine($"""
                [red]Received path [cyan]{pathName}[/] is not valid.[/]
                Try another like [bold]/dev/input/eventX[/].
                """);
                return;
            }

            ReadOnlyDevice device;

            try
            {
                device = new ReadOnlyDevice(pathName);
            }
            catch (UnixIOException e)
            {
                AnsiConsole.MarkupLine($"[bold red]Can't open device.[/] Try as [bold purple]sudo[/]");
                AnsiConsole.WriteException(e);
                return;
            }

            AnsiConsole.MarkupLine($"""
            Created device from path: [cyan]{pathName}[/]
            Device info:
                Name: [bold green]{device.Name}[/]
                Driver version: [bold purple]{device.DriverVersion}[/]
                Device ID:
                    Product: [bold purple]0x{device.Id[IdProperty.Product]:X}[/]
                    Vendor: [bold purple]0x{device.Id[IdProperty.Vendor]:X}[/]
                    BusType: [bold purple]0x{device.Id[IdProperty.BusType]:X}[/]
                    Version: [bold purple]0x{device.Id[IdProperty.Version]:X}[/]
            """);

            AnsiConsole.MarkupLine($"Disposing [green]{device.Name}[/]...");
            device.Dispose();
        }
    }
}
