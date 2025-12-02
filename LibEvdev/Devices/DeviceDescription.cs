// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using LibEvdev.Native;
using Serilog;

namespace LibEvdev.Devices
{
    public record DeviceDescription
    {
        public string Name { get; init; }
        public required IDictionary<IdProperty, int> Id { get; init; }
        public int? DriverVersion { get; init; }
        public string? Phys { get; init; }
        public string? Uniq { get; init; }
        public string? Path { get; init; }
        public (int delay, int period)? RepeatInfo { get; init; }

        public IDictionary<EventType, List<ushort>>? EventCapabilities { get; init; }

        [SetsRequiredMembers]
        public DeviceDescription(IDevice device)
        {
            Name = safelyGetName(device);
            Phys = safelyGetPhys(device);
            Uniq = safelyGetUniq(device);
            Path = device.Path;
            DriverVersion = device.DriverVersion;
            Id = device.Id;
            RepeatInfo = device.GetRepeat();

            var eventTypes = device.GetSupportedEventTypes();
            var capabilities = new Dictionary<EventType, List<ushort>>();
            foreach (var type in eventTypes)
            {
                capabilities[type] = new();
                // 0x2ff = KEY_MAX
                for (ushort code = 0; code < 0x2ff; code++)
                {
                    if (device.HasEvent((uint)type, code))
                        capabilities[type].Add(code);
                }
            }
            EventCapabilities = capabilities;
        }

        public DeviceDescription(string name)
        {
            Name = name;
        }

        [SetsRequiredMembers]
        public DeviceDescription(string name, IDictionary<IdProperty, int> id, IDictionary<EventType, List<ushort>> eventCapabilities)
        {
            Name = name;
            Id = id;
            EventCapabilities = eventCapabilities;
        }

        public readonly static DeviceDescription DEFAULT_MOUSE_CONFIG = new("LibEvdev mouse")
        {
            Id = new Dictionary<IdProperty, int>
            {
                {IdProperty.BusType, 0x03}, // BusType USB
                {IdProperty.Product, 0x1440},
                {IdProperty.Vendor, 0x30fa},
                {IdProperty.Version, 1},
            },
            EventCapabilities = new Dictionary<EventType, List<ushort>>
            {
                {EventType.Synchronization, []},
                {EventType.Key, [
                    (ushort)Key.BTN_LEFT,
                    (ushort)Key.BTN_RIGHT,
                    (ushort)Key.BTN_MIDDLE,
                    (ushort)Key.BTN_SIDE,
                    (ushort)Key.BTN_EXTRA
                ]},
                {EventType.Relative, [
                    (ushort)Relative.REL_X,
                    (ushort)Relative.REL_Y,
                    (ushort)Relative.REL_WHEEL,
                    (ushort)Relative.REL_WHEEL_HI_RES,
                ]},
                {EventType.Miscellaneous, [ (ushort)Miscellaneous.MSC_SCAN ]}
            }
        };

        private static string safelyGetName(IDevice device)
        {
            string? ret = null;
            try
            {
                ret = device.Name;
            }
            catch (Exception e)
            {
                Log.ForContext("SourceContext", "LibEvdev.Device.DeviceDescriptor")
                   .Warning("Cannot get device name.", e);
            }
            return ret ?? "N/A";
        }

        private static string safelyGetPhys(IDevice device)
        {
            string? ret = null;
            try
            {
                ret = device.Phys;
            }
            catch (Exception e)
            {
                Log.ForContext("SourceContext", "LibEvdev.Device.DeviceDescriptor")
                   .Warning("Cannot get device phys.", e);
            }
            return ret ?? "N/A";
        }

        private static string safelyGetUniq(IDevice device)
        {
            string? ret = null;
            try
            {
                ret = device.Uniq;
            }
            catch (Exception e)
            {
                Log.ForContext("SourceContext", "LibEvdev.Device.DeviceDescriptor")
                   .Warning("Cannot get device uniq.", e);
            }
            return ret ?? "N/A";
        }
    }
}
