// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly record struct TimeSpec(
        long Seconds,
        long NanoSeconds
    )
    {
        public static TimeSpec FromMilliseconds(long milliseconds)
        {
            long seconds = milliseconds / TimeSpan.MillisecondsPerSecond;
            long nanoseconds = milliseconds % TimeSpan.MillisecondsPerSecond * NANOSECONDS_PER_MS;
            return new(seconds, nanoseconds);
        }

        public DateTime AsDateTime() => new(Seconds * TimeSpan.TicksPerSecond + NanoSeconds / TimeSpan.NanosecondsPerTick);

        public const long NANOSECONDS_PER_MS = 1_000_000;
    }
}
