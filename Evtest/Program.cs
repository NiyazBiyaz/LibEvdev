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

                    using (var device = DeviceHelper.OpenReadOnly(pathName))
                    {
                        var description = new DeviceDescription(device);

                        DeviceInfoPrinter.WriteDeviceInfo(description);

                        AnsiConsole.Write("\n");
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[bold]No one device is assigned.[/]");

                List<DeviceDescription> devices = DeviceHelper.GetAllInputDevices()
                                                              .Select((device) => new DeviceDescription(device))
                                                              .ToList();

                var deviceToOpen = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select device to open.")
                        .PageSize(8)
                        .MoreChoicesText("[grey](Move up and down to reveal more devices)[/]")
                        .AddChoices(devices.Select((d) => $"{d.Path} [bold]{d.Name}[/]"))
                ).Split(" ").First();

                AnsiConsole.MarkupLine($"Your selection: [bold cyan]{deviceToOpen}[/].");

                using (var device = DeviceHelper.OpenReadOnly(deviceToOpen))
                    DeviceInfoPrinter.WriteDeviceInfo(new DeviceDescription(device));
            }
        }
    }
}
