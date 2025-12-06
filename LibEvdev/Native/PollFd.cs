// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PollFd
    {
        public int FileDescriptor;
        public short Events;
        public short Revents;
    }
}
