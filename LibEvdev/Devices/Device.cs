// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using Mono.Unix;
using Mono.Unix.Native;
using Serilog;

namespace LibEvdev.Devices
{
    public abstract class Device : IDevice
    {
        protected nint Dev;
        protected int FileDescriptor = -1;

        protected ILogger Logger => Log.ForContext("SourceContext", "LibEvdev.Devices.Device")
                                       .ForContext("DevicePath", Path);

        private GrabMode grabMode = GrabMode.UnGrab;
        private readonly string path = "N/A";

        protected Device(string path)
        {
            FileDescriptor = SysCall.open(path, (int)(OpenFlags.O_RDONLY | OpenFlags.O_NONBLOCK));
            if (FileDescriptor < 0)
                throw new UnixIOException(Stdlib.GetLastError());

            int err = Evdev.NewFromFd(FileDescriptor, ref Dev);
            if (err < 0)
            {
                SysCall.close(FileDescriptor);
                FileDescriptor = -1;

                throw AutoExternalException.New(-err);
            }

            Path = path;

            Logger.Information("Created device from path: {Path}", path);
        }

        public string Name => Evdev.GetName(Dev);

        public string Path
        {
            get => path;
            init { path = value; }
        }

        public GrabMode Grab
        {
            get => grabMode;
            set
            {
                grabMode = value;
                Evdev.Grab(Dev, Grab);
                Log.Information("New grab mode: {NewGrabMode}", this, grabMode);
            }
        }

        public IDictionary<IdProperty, int> Id
        {
            get
            {
                int bus = Evdev.GetIdBustype(Dev);
                int vdr = Evdev.GetIdVendor(Dev);
                int pro = Evdev.GetIdProduct(Dev);
                int ver = Evdev.GetIdVersion(Dev);
                return new Dictionary<IdProperty, int>()
                {
                    { IdProperty.BusType, bus },
                    { IdProperty.Vendor,  vdr },
                    { IdProperty.Product, pro },
                    { IdProperty.Version, ver }
                };
            }
            set
            {
                if (value.TryGetValue(IdProperty.BusType, out int bus))
                    Evdev.SetIdBustype(Dev, bus);
                if (value.TryGetValue(IdProperty.Vendor, out int vdr))
                    Evdev.SetIdVendor(Dev, vdr);
                if (value.TryGetValue(IdProperty.Product, out int pro))
                    Evdev.SetIdProduct(Dev, pro);
                if (value.TryGetValue(IdProperty.Version, out int ver))
                    Evdev.SetIdVersion(Dev, ver);
            }
        }

        public bool HasEvent(uint type, uint? code = null) =>
            Evdev.HasEventType(Dev, type) == 1 && (code is null || Evdev.HasEventCode(Dev, type, (uint)code) == 1);

        public bool HasEvent(string typeName, string? codeName = null)
        {
            ArgumentNullException.ThrowIfNull(typeName);
            uint type = Evdev.GetEventTypeByName(typeName);
            return HasEvent(type, codeName is null ? null : Evdev.GetEventCodeByName(typeName, codeName));
        }

        public (int delay, int period) GetRepeat()
        {
            int delay = 0;
            int period = 0;
            Evdev.GetRepeat(Dev, ref delay, ref period);

            return (delay, period);
        }

        public List<EventType> GetSupportedEventTypes()
        {
            var result = new List<EventType>();

            if (HasEvent((uint)EventType.Absolute))
                result.Add(EventType.Absolute);

            if (HasEvent((uint)EventType.Count))
                result.Add(EventType.Count);

            if (HasEvent((uint)EventType.ForceFeedback))
                result.Add(EventType.ForceFeedback);

            if (HasEvent((uint)EventType.ForceFeedbackStatus))
                result.Add(EventType.ForceFeedbackStatus);

            if (HasEvent((uint)EventType.Key))
                result.Add(EventType.Key);

            if (HasEvent((uint)EventType.Led))
                result.Add(EventType.Led);

            if (HasEvent((uint)EventType.Maximum))
                result.Add(EventType.Maximum);

            if (HasEvent((uint)EventType.Miscellaneous))
                result.Add(EventType.Miscellaneous);

            if (HasEvent((uint)EventType.Power))
                result.Add(EventType.Power);

            if (HasEvent((uint)EventType.Relative))
                result.Add(EventType.Relative);

            if (HasEvent((uint)EventType.Sounds))
                result.Add(EventType.Sounds);

            if (HasEvent((uint)EventType.Switch))
                result.Add(EventType.Switch);

            if (HasEvent((uint)EventType.Synchronization))
                result.Add(EventType.Synchronization);

            return result;
        }

        public List<string> GetSupportedEventTypesNames()
        {
            var supportedEventTypes = GetSupportedEventTypes();
            var result = new List<string>();

            foreach (EventType type in supportedEventTypes)
            {
                string? typeName = Evdev.EventTypeGetName((uint)type);
                if (typeName is null)
                    continue;
                result.Add(typeName);
            }

            return result;
        }

        public void Dispose()
        {
            if (Dev != nint.Zero)
                Evdev.Free(Dev);
            if (FileDescriptor >= 0)
                Syscall.close(FileDescriptor);
            GC.SuppressFinalize(this);
        }
    }
}
