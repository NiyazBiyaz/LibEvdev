// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    public enum ClockId
    {
        Realtime = 0,
        Monotonic = 1,
        ProcessCPUTime = 2,
        ThreadCPUTime = 3,
        RealtimeCoarse = 5,
        MonotonicCoarse = 6,
        MonotonicRaw = 4,
        RealtimeAlarm = 8,
        Boottime = 7,
        BoottimeAlarm = 9,
    }
}
