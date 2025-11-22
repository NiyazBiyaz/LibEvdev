// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using Mono.Unix.Native;

namespace LibEvdev.Devices
{
    public class ReadOnlyDevice(string path) : Device(path), IReadOnlyDevice
    {
        public bool StopFlag { get; set; }

        public IEnumerable<InputEvent> ReadInputEvents(int timeoutPeriod)
        {
            PollFd[] pollFds = [new PollFd() { FileDescriptor = FileDescriptor, Events = (int)PollEvents.POLLIN }];
            InputEvent inputEvent = default;
            ReadStatus status;
            ReadFlag flag;

            Logger.Information("Start polling events.");

            while (true)
            {
                if (StopFlag)
                    break;

                bool canRead = poll(pollFds, timeoutPeriod);

                if (canRead)
                {
                    Logger.Verbose("Can read events.");
                    flag = ReadFlag.Normal;
                    bool stop = false;
                    while (!stop)
                    {
                        status = Evdev.NextEvent(Dev, flag, ref inputEvent);
                        Logger.Verbose("Event reading result: {ReadStatus}", status);

                        if (status == ReadStatus.Sync)
                        {
                            Logger.Warning("Synchronization is requested.");
                            flag = ReadFlag.Sync;
                        }

                        else if (status == ReadStatus.Again)
                        {
                            Logger.Verbose("No events to read.");
                            stop = true;
                            continue;
                        }

                        else if ((int)status < 0 && status != ReadStatus.Again)
                            throw AutoExternalException.New(-(int)status);

                        Logger.Verbose("Received event: {InputEvent}", inputEvent);
                        yield return inputEvent;
                    }
                }
            }
        }

        private bool poll(PollFd[] pollFds, int timeout)
        {
            int result = SysCall.poll(pollFds, timeout);

            if (result > 0)
                return true;
            else if (result == 0)
                return false;

            throw AutoExternalException.New((int)Stdlib.GetLastError());
        }
    }
}
