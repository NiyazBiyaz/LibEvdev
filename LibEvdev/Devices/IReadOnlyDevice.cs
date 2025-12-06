// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public interface IReadOnlyDevice : IDevice
    {
        public bool StopFlag { get; set; }

        /// <summary>
        /// Read input events from this device per frame.
        /// </summary>
        /// <param name="timeoutPeriod">Timeout of <see cref="SysCall.poll(PollFd[], int, int)"/>.
        /// It's specifies how frequently loop would to check stop flag.</param>
        /// <returns>Unwrapped raw input events frames.</returns>
        public IEnumerable<InputEventRaw> ReadInputEvents(int timeoutPeriod);

        /// <summary>
        /// Asynchronously read input events from this device per frame.
        /// </summary>
        /// <param name="timeoutPeriod">Timeout of <see cref="SysCall.poll(PollFd[], int, int)"/>
        /// It's specifies how frequently loop would to check <paramref name="cancellationToken"/> status.</param>
        /// <param name="cancellationToken">Cancellation token for stop reading loop.</param>
        /// <returns>Unwrapped raw input events frames.</returns>
        public IAsyncEnumerable<InputEventRaw> ReadInputEventsAsync(int timeoutPeriod, CancellationToken cancellationToken);
    }
}
