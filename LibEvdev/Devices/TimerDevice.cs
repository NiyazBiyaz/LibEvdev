// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using Mono.Unix.Native;
using Serilog;

namespace LibEvdev.Devices
{
    public class TimerDevice : IReadOnlyDevice
    {
        private IntervalTimerSpec timerSpec = default;
        private readonly int timerId = -1;
        private static ILogger logger => Log.ForContext("SourceContext", "LibEvdev.Devices.TimerDevice");

        public bool Enabled { get; private set; }

        public int FileDescriptor { get; private set; } = -1;

        public TimerDevice(IntervalTimerSpec timerSpec)
        {
            FileDescriptor = SysCall.timerfd_create(ClockId.Monotonic, (int)OpenFlags.O_NONBLOCK | (int)OpenFlags.O_CLOEXEC);
            if (FileDescriptor < 0)
                throw AutoExternalException.New();

            this.timerSpec = timerSpec;
            updateMyTimerSpec();
        }

        public TimerDevice(IntervalTimerSpec timerSpec, int id)
            : this(timerSpec)
        {
            timerId = id;
        }

        (int delay, int period) IDevice.GetRepeat() => (timerSpec.Value.AsDateTime().Millisecond, timerSpec.Interval.AsDateTime().Millisecond);

        public TimeSpec Interval
        {
            get => timerSpec.Interval;
            set
            {
                timerSpec.Interval = value;
                updateMyTimerSpec();
            }
        }

        public TimeSpec Delay
        {
            get => timerSpec.Value;
            set
            {
                timerSpec.Value = value;
                updateMyTimerSpec();
            }
        }

        public void Disable()
        {
            var time = new IntervalTimerSpec()
            {
                Value = new TimeSpec(0, 0),
            };
            updateTimerSpec(time);
        }

        public void Enable() => updateMyTimerSpec();

        public unsafe int ReadEventFrame(Span<InputEventRaw> eventFrame)
        {
            long expirations = 0;
            SysCall.read(FileDescriptor, &expirations, 8);

            if (expirations <= 0)
                return 0;

            // If only timeSpec.Value is have been set.
            if (expirations == 1 && timerSpec.Interval == default)
            {
                eventFrame[0] = new InputEventRaw(EventType.Timer, (ushort)TimerCode.Clock, timerId);
                return 1;
            }

            int i = 0;
            for (; i < expirations && i < eventFrame.Length; i++)
                eventFrame[i] = new InputEventRaw(EventType.Timer, (ushort)TimerCode.Repeat, timerId);

            return i;
        }

        private void updateMyTimerSpec() => updateTimerSpec(timerSpec);

        private void updateTimerSpec(IntervalTimerSpec timerSpec)
        {
            int res;
            unsafe
            {
                res = SysCall.timerfd_settime(FileDescriptor, 0, ref timerSpec, null);
            }
            if (res < 0)
                throw AutoExternalException.New();

            Enabled = timerSpec.Value != default;

            logger.Information("Timer spec successfully updated. Timer is enabled: {Enabled}", Enabled);
            logger.Verbose("New timer spec value: {TimerSpec}", timerSpec);
        }

        public void Dispose()
        {
            if (FileDescriptor >= 0)
                SysCall.close(FileDescriptor);
        }

        string IDevice.Name { get; } = "Timer ReadOnlyDevice";
        string IDevice.Path { get; } = "N/A";
        string IDevice.Phys { get; } = "N/A";
        string IDevice.Uniq { get; } = "N/A";
        int IDevice.DriverVersion { get; } = 0;
        GrabMode IDevice.Grab { get; set; } = GrabMode.UnGrab;
        IDictionary<IdProperty, int> IDevice.Id { get; } = new Dictionary<IdProperty, int>();

#pragma warning disable CS1066
        bool IDevice.HasEvent(EventType type, uint? code = null) => false;
        bool IDevice.HasEvent(uint type, uint? code = null) => false;
        bool IDevice.HasEvent(string type, string? code = null) => false;
#pragma warning restore CS1066

        List<EventType> IDevice.GetSupportedEventTypes() => [];
        List<string> IDevice.GetSupportedEventTypesNames() => [];
    }

    public enum TimerCode : ushort
    {
        Repeat,
        Clock,
    }
}
