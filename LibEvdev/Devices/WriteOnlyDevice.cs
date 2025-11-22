// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public class WriteOnlyDevice : Device, IWriteOnlyDevice, IDisposable
    {
        private nint uiDev;

        public WriteOnlyDevice(DeviceDescription configuration)
            : base()
        {
            ArgumentNullException.ThrowIfNull(configuration.EventCapabilities);

            SetId(configuration.Id);
            SetName(configuration.Name);

            foreach (var type in configuration.EventCapabilities.Keys)
            {
                Enable(type);
                foreach (uint code in configuration.EventCapabilities[type])
                    Enable(type, code);
            }

            int err = Evdev.UinputCreateFromDevice(Dev, (int)UinputOpenMode.Managed, ref uiDev);
            if (err != 0)
                throw AutoExternalException.New(err);

            Logger.Information("Created new device from scratch using configuration: {Config}", configuration);
        }

        public void SetName(string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            Evdev.SetName(Dev, name);
        }

        public void SetId(IDictionary<IdProperty, int> idDict)
        {
            if (idDict.TryGetValue(IdProperty.BusType, out int bus))
                Evdev.SetIdBustype(Dev, bus);
            if (idDict.TryGetValue(IdProperty.Vendor, out int vdr))
                Evdev.SetIdVendor(Dev, vdr);
            if (idDict.TryGetValue(IdProperty.Product, out int pro))
                Evdev.SetIdProduct(Dev, pro);
            if (idDict.TryGetValue(IdProperty.Version, out int ver))
                Evdev.SetIdVersion(Dev, ver);
        }

        public void Enable(EventType type) => Evdev.EnableEventType(Dev, (uint)type);

        // original function signature have an argument that not used in this method
        // so we just place there a null
        public unsafe void Enable(EventType type, uint code) => Evdev.EnableEventCode(Dev, (uint)type, code, null);

        public void Disable(EventType type) => Evdev.DisableEventType(Dev, (uint)type);

        public void Disable(EventType type, uint code) => Evdev.DisableEventCode(Dev, (uint)type, code);

        public void Write(InputEvent inputEvent)
        {
            Evdev.UinputWriteEvent(uiDev, (uint)inputEvent.Type, inputEvent.Code, inputEvent.Value);
        }

        public new void Dispose()
        {
            base.Dispose();
            Evdev.UinputDestroy(uiDev);
            GC.SuppressFinalize(this);
        }
    }
}
