// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public interface IWriteOnlyDevice : IDevice
    {
        public void SetName(string name);

        public void SetId(IDictionary<IdProperty, int> idDict);

        /// <summary>
        /// Enable specified <paramref name="type"/> for this device for allow using by it.
        /// </summary>
        /// <param name="type">Type for enabling.</param>
        public void Enable(EventType type);

        /// <summary>
        /// Enable specified <paramref name="code"/> of the <paramref name="type"/>
        /// for this device for allow using by it.
        /// </summary>
        /// <param name="type">Type that for the <paramref name="code"/>.</param>
        /// <param name="code">Code of the <paramref name="type"/> for enabling.</param>
        public void Enable(EventType type, uint code);

        /// <summary>
        /// Disable specified <paramref name="type"/> for this device, so it's will can't
        /// be used after it without <see cref="Enable"/>
        /// </summary>
        /// <param name="type">Type for disabling.</param>
        public void Disable(EventType type);

        /// <summary>
        /// Disable specified <paramref name="code"/> of the <paramref name="type"/> for this device.
        /// </summary>
        /// <param name="type">Type of the <paramref name="code"/>.</param>
        /// <param name="code">Code for disabling.</param>
        public void Disable(EventType type, uint code);

        /// <summary>
        /// Write only <paramref name="inputEvent"/> to the device descriptor. Needs <see cref="Flush"/>
        /// for not miss the synchronization with kernel.
        /// </summary>
        /// <param name="inputEvent"></param>
        public void Write(InputEvent inputEvent);

        /// <summary>
        /// <see cref="Write"/> synchronization event to the device.
        /// </summary>
        public void Flush()
        {
            Write(new InputEvent(EventType.Synchronization, 0, 0));
        }

        /// <summary>
        /// <see cref="Write"/> one event and flush it.
        /// </summary>
        public void WriteFrame(InputEvent inputEvent)
        {
            Write(inputEvent);
            Flush();
        }

        /// <summary>
        /// <see cref="Write"/> some events and flush it.
        /// </summary>
        /// <param name="inputEvents"></param>
        public void WriteFrame(IEnumerable<InputEvent> inputEvents)
        {
            foreach (var evt in inputEvents)
                Write(evt);
            Flush();
        }
    }
}
