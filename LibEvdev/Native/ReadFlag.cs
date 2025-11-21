// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace LibEvdev.Native
{
    // API references: libevdev/libevdev.h
    public enum ReadFlag : uint
    {
        Sync = 1,
        Normal = 2,
        ForceSync = 4,
        Blocking = 8,
    }
}
