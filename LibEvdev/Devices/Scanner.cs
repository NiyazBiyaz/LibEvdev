// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;
using Mono.Unix.Native;
using Serilog;

namespace LibEvdev.Devices
{
    public class Scanner
    {
        private readonly PollFd[] pollFds;
        private readonly IReadOnlyDevice[] devices;
        private readonly ILogger logger = Log.ForContext("SourceContext", "LibEvdev.Devices.Scanner");

        public Scanner(IReadOnlyDevice[] devices)
        {
            this.devices = devices;

            pollFds = new PollFd[devices.Length];
            for (int i = 0; i < pollFds.Length; i++)
            {
                int fd = devices[i].FileDescriptor;
                pollFds[i] = new PollFd { FileDescriptor = fd, Events = (short)PollEvents.POLLIN };
            }
        }

        public IEnumerable<InputEventRaw> ReadInputEvents(int timeoutPeriod, CancellationToken cancel)
        {
            var eventFrame = new InputEventRaw[12];

            logger.Information("Start reading events");
            while (!cancel.IsCancellationRequested)
            {
                for (int i = 0; i < pollFds.Length; i++)
                    pollFds[i].Revents = 0;

                if (poll(timeoutPeriod))
                {
                    for (int devIndex = 0; devIndex < pollFds.Length; devIndex++)
                    {
                        if (canRead(pollFds[devIndex]))
                        {
                            int wasRead = devices[devIndex].ReadEventFrame(eventFrame.AsSpan());

                            for (int evtIndex = 0; evtIndex < wasRead; evtIndex++)
                                yield return eventFrame[evtIndex];

                            Array.Clear(eventFrame);
                        }
                    }
                }
            }
            logger.Information("End reading events");
            cancel.ThrowIfCancellationRequested();
        }

        private bool poll(int timeout)
        {
            int result = SysCall.poll(pollFds, pollFds.Length, timeout);

            if (result > 0)
                return true;
            if (result == 0)
                return false;

            throw AutoExternalException.New();
        }

        private static bool canRead(PollFd pollFd) => (pollFd.Revents & (short)PollEvents.POLLIN) != 0;
    }
}
