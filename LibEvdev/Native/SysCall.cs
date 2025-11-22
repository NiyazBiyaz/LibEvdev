// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    public static partial class SysCall
    {
        [LibraryImport("libc", SetLastError = true)]
        internal static partial int poll(PollFd[] fds, int timeout);

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int poll(PollFd[] fds, int nfds, int timeout);

        [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial int open(string pathname, int flags);

        [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
        internal static partial int open(string pathname, int flags, uint mode);

        [LibraryImport("libc", SetLastError = true)]
        internal static partial int close(int fd);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PollFd
    {
        public int FileDescriptor;
        public short Events;
        public short Revents;
    }
}
