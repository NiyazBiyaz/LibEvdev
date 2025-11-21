// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Runtime.InteropServices;

namespace LibEvdev
{
    /// <summary>
    /// <see cref="ExternalException"/> with automatic insertion of <see cref="Marshal.GetLastPInvokeErrorMessage"/>
    ///  and <see cref="Marshal.GetLastPInvokeError"/>
    /// </summary>
    public static class AutoExternalException
    {
        public static ExternalException New() => new(Marshal.GetLastPInvokeErrorMessage(), Marshal.GetLastPInvokeError());

        public static ExternalException Throw(int errorCode) => new(Marshal.GetLastPInvokeErrorMessage(), errorCode);
    }
}

