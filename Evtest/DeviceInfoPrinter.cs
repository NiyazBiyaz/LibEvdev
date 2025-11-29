// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Evtest.Utils;
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
            Created device from path: {deviceDescription.Path?.DyePath() ?? "N/A".DyeWarning()}
            Device info:
                Name: [bold]{deviceDescription.Name.DyeStringValue()}[/]
                Phys: [bold]{deviceDescription.Phys?.DyePath() ?? "N/A".DyeWarning()}[/]
                Driver version: [bold]{deviceDescription.DriverVersion?.DyeIntegerValue() ?? "N/A".DyeWarning()}[/]
                Device ID:
                    Product: [bold]{deviceDescription.Id[IdProperty.Product].DyeIntegerValue()}[/]
                    Vendor: [bold]{deviceDescription.Id[IdProperty.Vendor].DyeIntegerValue()}[/]
                    BusType: [bold]{deviceDescription.Id[IdProperty.BusType].DyeIntegerValue()}[/]
                    Version: [bold]{deviceDescription.Id[IdProperty.Version].DyeIntegerValue()}[/]
            """);

            AnsiConsole.MarkupLine("Supported events:");

            foreach (var type in deviceDescription.EventCapabilities.Keys)
            {
                string typeName = Evdev.GetEventTypeName((uint)type);
                AnsiConsole.MarkupLine($"\tEvent type: [bold]{type.DyeEnumValue()}[/] ({typeName.DyeStringValue()})");

                if (type == EventType.Synchronization) continue;
                foreach (ushort code in deviceDescription.EventCapabilities[type])
                {
                    string codeName = Evdev.GetEventCodeName((uint)type, code);
                    AnsiConsole.MarkupLine($"\t\tEvent code: [bold]{code.DyeIntegerValue()}[/] ({codeName.DyeStringValue()})");
                }
            }

            AnsiConsole.MarkupLine("Other information:");

            if (deviceDescription.RepeatInfo is not null)
            {
                (int delay, int period) = deviceDescription.RepeatInfo.Value;
                AnsiConsole.MarkupLine($"""
                    Repeat:
                        Delay: [bold]{delay.DyeIntegerValue()}[/]
                        Period: [bold]{period.DyeIntegerValue()}[/]
                """);
            }
        }
    }
}
