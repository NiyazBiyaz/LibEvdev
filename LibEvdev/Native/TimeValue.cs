// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    /// <summary>
    /// Representation of time in native system. ABI reference: <see pref="bits/types/struct_timeval.h"/>
    /// </summary>
    /// <param name="Seconds">Time value in <i>Unix time format</i> as 64-bit signed-integer</param>
    /// <param name="Microseconds">
    /// Time value in microseconds starting from the integer value of Unix-format.
    /// Value range: [<b>0 - 999 999</b>]
    /// </param>
    [StructLayout(LayoutKind.Sequential)]
    public readonly record struct TimeValue(
        long Seconds,
        long Microseconds
    )
    {
        public long AsTicks() => Seconds * TimeSpan.TicksPerSecond + Microseconds * TimeSpan.TicksPerMicrosecond;

        public TimeOnly AsTimeOnly() => new(AsTicks());

        public DateTime AsDateTime() => DateTime.UnixEpoch.AddTicks(AsTicks());

        public static TimeValue FromMilliseconds(long milliseconds) =>
            new(milliseconds / TimeSpan.MillisecondsPerSecond, milliseconds * TimeSpan.MicrosecondsPerMillisecond);
    }
}
