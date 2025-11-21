// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mono.Unix.Native;

namespace LibEvdev.Native
{
    public enum ReadStatus
    {
        Success,
        Sync,
        Again = -Errno.EAGAIN
    }
}
