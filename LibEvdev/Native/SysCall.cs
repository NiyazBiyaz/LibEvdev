// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    public static partial class SysCall
    {
        #region System IO

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int poll(PollFd[] fds, int nfds, int timeout);

        [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial int open(string pathname, int flags);

        [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial int open(string pathname, int flags, uint mode);

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int close(int fd);

        [LibraryImport("libc", SetLastError = true)]
        internal static unsafe partial nint read(int fd, void* buf, nuint nbytes);

        #endregion

        #region TimeFd

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int timerfd_create(ClockId clockid, int flags);

        [LibraryImport("libc", SetLastError = true)]
        internal static unsafe partial int timerfd_settime(int fd, int flags,
                                    ref IntervalTimerSpec newValue, void* oldValue);

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int timerfd_gettime(int fd, ref IntervalTimerSpec currValue);

        #endregion
    }
}
