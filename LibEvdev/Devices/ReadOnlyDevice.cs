// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.CompilerServices;
using LibEvdev.Native;
using Mono.Unix.Native;

namespace LibEvdev.Devices
{
    public class ReadOnlyDevice(string path) : Device(path), IReadOnlyDevice
    {
        /// <summary>
        /// Stop flag for end read loop if. It works only for synchronous loop, for
        /// async use <see cref="CancellationToken"/> parameter.
        /// </summary>
        public bool StopFlag { get; set; }

        public IEnumerable<InputEventRaw> ReadInputEvents(int timeoutPeriod)
        {
            PollFd[] pollFds = [new PollFd() { FileDescriptor = FileDescriptor, Events = (int)PollEvents.POLLIN }];
            InputEventRaw inputEvent = default;
            ReadStatus status;
            ReadFlag flag;

            InputEventRaw[] eventsFrame = new InputEventRaw[16];

            Logger.Information("Start polling events.");

            while (!StopFlag)
            {
                bool canRead = poll(pollFds, timeoutPeriod);

                if (canRead)
                {
                    Logger.Verbose("Can read events.");
                    flag = ReadFlag.Normal;
                    bool stop = false;
                    int wasRead = 0;
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
                        eventsFrame[wasRead++] = inputEvent;
                    }

                    foreach (var evt in eventsFrame)
                        yield return evt;

                    Array.Clear(eventsFrame);
                }
            }
        }

        public async IAsyncEnumerable<InputEventRaw> ReadInputEventsAsync(int timeoutPeriod, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            PollFd[] pollFds = [new PollFd() { FileDescriptor = FileDescriptor, Events = (int)PollEvents.POLLIN }];
            InputEventRaw inputEvent = default;
            ReadStatus status;
            ReadFlag flag;

            InputEventRaw[] eventsFrame = new InputEventRaw[32];

            Logger.Information("Start polling events.");

            while (!cancellationToken.IsCancellationRequested)
            {
                await pollAsync(pollFds, timeoutPeriod, cancellationToken);

                Logger.Verbose("Can read events");
                flag = ReadFlag.Normal;
                bool stop = false;
                int wasRead = 0;
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
                    eventsFrame[wasRead++] = inputEvent;
                }

                foreach (var evt in eventsFrame)
                    yield return evt;

                Array.Clear(eventsFrame);
            }
        }

        private async ValueTask pollAsync(PollFd[] pollFds, int timeout, CancellationToken token)
        {
            if (Evdev.HasEventPending(Dev) == 1)
                return;

            await Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    int result = SysCall.poll(pollFds, 1, timeout);

                    if (result > 0)
                        return;
                    else if (result == 0)
                        continue;

                    throw AutoExternalException.New((int)Stdlib.GetLastError());
                }
            }, token);
        }

        private bool poll(PollFd[] pollFds, int timeout)
        {
            int result = SysCall.poll(pollFds, 1, timeout);

            if (result > 0)
                return true;
            else if (result == 0)
                return false;

            throw AutoExternalException.New((int)Stdlib.GetLastError());
        }
    }
}
