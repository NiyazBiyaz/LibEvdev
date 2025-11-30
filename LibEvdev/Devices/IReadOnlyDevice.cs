// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public interface IReadOnlyDevice : IDevice
    {
        public IEnumerable<InputEventRaw> ReadInputEvents(int timeoutPeriod);

        public IAsyncEnumerable<InputEventRaw> ReadInputEventsAsync(int timeoutPeriod, CancellationToken cancellationToken);
    }
}
