// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;
using Serilog;
using Spectre.Console;

namespace Evtest
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console(outputTemplate:
                                "{Timestamp:yyyy-MM-dd-HH:mm:ss} [{Level}][{SourceContext}] {Message:lj}{NewLine}{Exception}")
                            .MinimumLevel.Error()
                            .CreateLogger();

            if (args.Length > 1)
            {
                foreach (string pathName in args)
                {
                    validatePath(pathName);

                    using var device = DeviceHelper.OpenReadOnly(pathName);

                    var description = new DeviceDescription(device);

                    DeviceInfoPrinter.WriteDeviceInfo(description);

                    AnsiConsole.Write("\n");

                }
            }
            else if (args.Length == 1)
            {
                string path = args.First();
                await StartInteractiveMode(path);
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

                await StartInteractiveMode(deviceToOpen);
            }
        }

        public async static Task StartInteractiveMode(string path)
        {
            validatePath(path);

            CancellationTokenSource cts = new();
            long eventsCount = 0;

            Console.CancelKeyPress += (_, _) =>
            {
                cts.Cancel();
                AnsiConsole.MarkupLine($"Events was read: [bold green]{eventsCount}[/]");
            };

            using var device = DeviceHelper.OpenReadOnly(path);
            var description = new DeviceDescription(device);
            DeviceInfoPrinter.WriteDeviceInfo(description);

            AnsiConsole.MarkupLine($"[bold]Start receiving events from device [green]{description.Name}[/][/]");

            await foreach (var evt in device.ReadInputEventsAsync(100, cts.Token))
            {
                eventsCount++;

                if (evt.Type == EventType.Synchronization)
                {
                    AnsiConsole.MarkupLine("[bold]------------------Sync reported------------------[/]");
                    continue;
                }

                DateTime timeStamp = evt.TimeValue.AsDateTime();
                string typeName = Evdev.GetEventTypeName(evt.Type);
                string codeName = Evdev.GetEventCodeName(evt.Type, evt.Code);

                AnsiConsole.MarkupLine($"[gray54]{timeStamp:T}.{timeStamp:fffff}[/] Received event: " +
                                        $"type = [bold green]{evt.Type}[/] [purple]{typeName}[/] " +
                                        $"code = [bold green]{evt.Code}[/] [purple]{codeName}[/] " +
                                        $"value = [bold green]{evt.Value}[/]");
            }
        }

        private static void validatePath(string path)
        {
            if (!Device.IsValidDevicePath(path))
            {
                AnsiConsole.MarkupLine($"""
                [red]Received path [cyan]{path}[/] is not valid.[/]
                Try another like [bold]/dev/input/eventX[/].
                """);
                return;
            }
        }
    }
}
