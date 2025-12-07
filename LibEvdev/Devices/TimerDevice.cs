// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using Mono.Unix.Native;

namespace LibEvdev.Devices
{
    public class TimerDevice : IReadOnlyDevice
    {
        private IntervalTimerSpec timerSpec = default;

        public int FileDescriptor { get; private set; } = -1;

        public TimerDevice(IntervalTimerSpec timerSpec)
        {
            FileDescriptor = SysCall.timerfd_create(ClockId.Monotonic, (int)OpenFlags.O_NONBLOCK | (int)OpenFlags.O_CLOEXEC);
            if (FileDescriptor < 0)
                throw AutoExternalException.New();

            int res;
            unsafe
            {
                res = SysCall.timerfd_settime(FileDescriptor, 0, ref timerSpec, null);
            }
            if (res < 0)
                throw AutoExternalException.New();
        }

        (int delay, int period) IDevice.GetRepeat() => (timerSpec.Value.AsDateTime().Millisecond, timerSpec.Interval.AsDateTime().Millisecond);

        public unsafe int ReadEventFrame(Span<InputEventRaw> eventFrame)
        {
            long expirations = 0;
            SysCall.read(FileDescriptor, &expirations, 8);

            if (expirations <= 0)
                return 0;

            int i = 0;
            for (; i < expirations && i < eventFrame.Length; i++)
                eventFrame[i] = InputEventRaw.TIMER;

            return i;
        }

        public bool CanRead() => throw new NotSupportedException(); // Wait it on FD, please

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

    public enum Timer : ushort
    {
        Repeat = 0x03
    }
}
