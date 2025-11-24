// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;
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

                    var description = new DeviceDescription(device);
                    (int delay, int period) = device.GetRepeat();

                    writeDeviceInfo(description, delay, period);

                    device.Dispose();
                    AnsiConsole.MarkupLine($"[orange]Disposing [green]{description.Name}[/]...[/]");
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

        private static void writeDeviceInfo(DeviceDescription deviceDescription, int delay, int period)
        {
            if (deviceDescription.EventCapabilities is null)
            {
                var e = new NullReferenceException("Can't read event capabilities.");
                AnsiConsole.WriteException(e);
                throw e;
            }

            AnsiConsole.MarkupLine($"""
            Created device from path: [cyan]{deviceDescription.Path}[/]
            Device info:
                Name: [bold green]{deviceDescription.Name}[/]
                Driver version: [bold purple]{deviceDescription.DriverVersion}[/]
                Device ID:
                    Product: [bold purple]0x{deviceDescription.Id[IdProperty.Product]:X}[/]
                    Vendor: [bold purple]0x{deviceDescription.Id[IdProperty.Vendor]:X}[/]
                    BusType: [bold purple]0x{deviceDescription.Id[IdProperty.BusType]:X}[/]
                    Version: [bold purple]0x{deviceDescription.Id[IdProperty.Version]:X}[/]
            """);

            AnsiConsole.MarkupLine("Supported events:");

            foreach (var type in deviceDescription.EventCapabilities.Keys)
            {
                string typeName = Evdev.GetEventTypeName((uint)type);
                AnsiConsole.MarkupLine($"\tEvent type: [bold green]{type}[/] ([purple]{typeName}[/])");

                if (type == EventType.Synchronization) continue;
                foreach (ushort code in deviceDescription.EventCapabilities[type])
                {
                    string codeName = Evdev.GetEventCodeName((uint)type, code);
                    AnsiConsole.MarkupLine($"\t\tEvent code: [bold green]{code}[/] ([purple]{codeName}[/])");
                }
            }

            AnsiConsole.MarkupLine("Other:");

            if (delay != 0 && period != 0)
            {
                AnsiConsole.MarkupLine($"""
                    Repeat:
                        Delay: [bold purple]{delay}[/]
                        Period: [bold purple]{period}[/]
                """);
            }
        }
    }
}
