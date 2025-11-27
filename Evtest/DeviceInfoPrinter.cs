// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;
using Spectre.Console;

namespace Evtest
{
    public static class DeviceInfoPrinter
    {
        public static void WriteDeviceInfo(DeviceDescription deviceDescription)
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

            AnsiConsole.MarkupLine("Other information:");

            if (deviceDescription.RepeatInfo is not null)
            {
                (int delay, int period) = deviceDescription.RepeatInfo.Value;
                AnsiConsole.MarkupLine($"""
                    Repeat:
                        Delay: [bold purple]{delay}[/]
                        Period: [bold purple]{period}[/]
                """);
            }
        }

        public static IEnumerable<string> ParseEventsFromEnumerable(IEnumerable<InputEvent> inputEvents)
        {
            foreach (var evt in inputEvents)
            {
                if (evt.Type == EventType.Synchronization)
                {
                    yield return "[bold]-------Sync reported]-------[/]";
                    continue;
                }

                DateTime timeStamp = evt.TimeValue.AsDateTime();
                string typeName = Evdev.GetEventTypeName(evt.Type);
                string codeName = Evdev.GetEventCodeName(evt.Type, evt.Code);

                yield return $"[gray54]{timeStamp:t}[/] Received event: " +
                             $"type = [bold green]{evt.Type}[/][purple]{typeName}[/] " +
                             $"code = [bold green]{evt.Type}[/][purple]{codeName}[/] " +
                             $"value = [bold green]{evt.Value}[/]";
            }
        }
    }
}
