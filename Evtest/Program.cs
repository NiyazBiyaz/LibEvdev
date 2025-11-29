// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Evtest.Utils;
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
                await StartInteractiveModeAsync(path);
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
                        .MoreChoicesText("(Move up and down to reveal more devices)".DyeDimmed())
                        .AddChoices(devices.Select((d) => $"{d.Path} [bold]{d.Name}[/]"))
                ).Split(" ").First();

                AnsiConsole.MarkupLine($"Your selection: {deviceToOpen.DyePath()}.");

                using (var device = DeviceHelper.OpenReadOnly(deviceToOpen))
                    DeviceInfoPrinter.WriteDeviceInfo(new DeviceDescription(device));

                await StartInteractiveModeAsync(deviceToOpen);
            }
        }

        public async static Task StartInteractiveModeAsync(string path)
        {
            validatePath(path);

            CancellationTokenSource cts = new();
            long eventsCount = 0;

            Console.CancelKeyPress += (_, _) =>
            {
                cts.Cancel();
                AnsiConsole.MarkupLine($"Device {path.DyePath()}: Events was read: [bold]{eventsCount.DyeIntegerValue()}[/]");
            };

            using var device = DeviceHelper.OpenReadOnly(path);
            var description = new DeviceDescription(device);

            AnsiConsole.MarkupLine($"[bold]Start receiving events from device {description.Name.DyeStringValue()}[/]");

            await foreach (var evt in device.ReadInputEventsAsync(100, cts.Token))
            {
                eventsCount++;

                if (evt.Type == EventType.Synchronization)
                {
                    int width = Console.WindowWidth;
                    const string sync_string = "Sync reported";

                    for (int i = 0; i < (width - sync_string.Length) / 2; i++)
                        AnsiConsole.Write("-");
                    AnsiConsole.Markup(sync_string.DyeAttention());
                    for (int i = (width - sync_string.Length) / 2 + sync_string.Length; i < width; i++)
                        AnsiConsole.Write("-");

                    continue;
                }

                DateTime timeStamp = evt.TimeValue.AsDateTime();
                string typeName = Evdev.GetEventTypeName(evt.Type);
                string codeName = Evdev.GetEventCodeName(evt.Type, evt.Code);

                AnsiConsole.MarkupLine($"[grey30]{timeStamp.ToLocalTime():T}.{timeStamp:fffff}[/] Received event: " +
                                        $"type = [bold]{evt.Type.DyeEnumValue()}[/] {typeName.DyeStringValue()} " +
                                        $"code = [bold]{evt.Code.DyeIntegerValue()}[/] {codeName.DyeStringValue()} " +
                                        $"value = [bold]{evt.Value.DyeIntegerValue()}[/]");
            }
        }

        private static void validatePath(string path)
        {
            if (!Device.IsValidDevicePath(path))
            {
                AnsiConsole.MarkupLine($"""
                {$"Received path {path.DyePath()} is not valid.".DyeError()}
                Try another like [bold]/dev/input/eventX[/].
                """);
                return;
            }
        }
    }
}
