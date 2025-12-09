// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public class ReadOnlyDevice(string path) : Device(path), IReadOnlyDevice
    {
        public int ReadEventFrame(Span<InputEventRaw> eventFrame)
        {
            if (!canRead())
                throw new NotSupportedException("No events to read.");

            InputEventRaw inputEvent = default;
            ReadStatus status;
            ReadFlag flag = ReadFlag.Normal;

            Logger.Information("Start reading events");

            int wasRead = 0;
            bool stop = false;
            while (!stop)
            {
                status = Evdev.NextEvent(Dev, flag, ref inputEvent);

                if (status == ReadStatus.Sync)
                {
                    Logger.Warning("Synchronization is requested.");
                    flag = ReadFlag.Sync;
                }

                else if (status == ReadStatus.Again)
                {
                    stop = true;
                    continue;
                }

                else if ((int)status < 0 && status != ReadStatus.Again)
                    throw AutoExternalException.New(-(int)status);

                eventFrame[wasRead++] = inputEvent;
            }

            return wasRead;
        }

        private bool canRead() => Evdev.HasEventPending(Dev) == 1;
    }
}
