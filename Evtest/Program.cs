// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;
using LibEvdev.Native;

namespace Evtest
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: Evtest <path-to-dev>");
                return 1;
            }

            var mouse = new ReadOnlyDevice(args[0]);

            Console.CancelKeyPress += (_, _) =>
            {
                mouse.StopFlag = true;
            };

            foreach (var inputEvent in mouse.ReadInputEvents(1000))
            {
                if (inputEvent.Type != EventType.Synchronization)
                    Console.WriteLine(formatEvent(inputEvent));
                else
                    Console.WriteLine("===================End event frame===================");
            }

            mouse.Dispose();

            return 0;
        }

        private static string formatEvent(InputEvent inputEvent)
        {
            string typeName = Evdev.GetEventTypeName((uint)inputEvent.Type);
            string codeName = Evdev.GetEventCodeName((uint)inputEvent.Type, inputEvent.Code);

            return string.Format("Event: time: {0}, type: {1}, code: {2}, value: {3}",
                                inputEvent.TimeValue.AsDateTime().ToLocalTime(),
                                typeName,
                                codeName,
                                inputEvent.Value);
        }
    }
}
