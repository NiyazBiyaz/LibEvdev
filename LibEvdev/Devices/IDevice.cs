// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Native;

namespace LibEvdev.Devices
{
    public interface IDevice : IDisposable
    {
        /// <summary>
        /// Name of the device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// File descriptor of the device.
        /// </summary>
        public int FileDescriptor { get; }

        /// <summary>
        /// Path that was used to open this device. <i>N/A</i> if device was opened by other way.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Physical path of the devices, e.g. usb-port.
        /// </summary>
        public string Phys { get; }

        /// <summary>
        /// Uniq id of the device that make different between devices.
        /// </summary>
        public string Uniq { get; }

        /// <summary>
        /// Driver version of the device.
        /// </summary>
        public int DriverVersion { get; }

        /// <summary>
        /// Allows automatically work with the <see cref="Evdev.Grab"/>, enables exclusive mode for this
        /// process. It's mean another processes (including DE, composers and etc) will never see the events
        /// from this device while you hold it at <see cref="GrabMode.Grab"/> mode. So, <b>use carefully</b>.
        /// </summary>
        public GrabMode Grab { get; set; }

        /// <summary>
        /// Combined representation of the device id properties as dictionary.
        /// It's similar to
        /// <code>
        ///     GetIdBustype(dev);
        ///     GetIdVendor(dev);
        ///     GetIdProduct(dev);
        ///     GetIdVersion(dev);
        /// </code>
        /// </summary>
        /// <remarks>
        /// Allows to individually set different <see cref="IdProperty"/> using dicts.
        /// </remarks>
        IDictionary<IdProperty, int> Id { get; }

        /// <param name="type">Event type value.</param>
        /// <param name="code">[<i>Optional</i>] Event code value. Don't set if you want to check availability only of the
        /// <see cref="type"/>.</param>
        /// <returns><see langword="true"/> if device supports event, otherwise <see langword="false"/>.</returns>
        bool HasEvent(uint type, uint? code = null);

        /// <inheritdoc cref="HasEvent"/>
        public bool HasEvent(EventType type, uint? code = null);

        /// <inheritdoc cref="HasEvent"/>
        /// <param name="typeName">Event type name.</param>
        /// <param name="codeName">[<i>Optional</i>] Event code name. Don't set if you want to check availability only of the
        /// <see cref="typeName"/>.</param>
        bool HasEvent(string typeName, string? codeName = null);

        /// <summary>
        /// Get the repeat delay and repeat period values for this device.
        /// </summary>
        /// <returns>Tuple of values in milliseconds, first is delay, second is period.</returns>
        (int delay, int period) GetRepeat();

        /// <summary>
        /// Get list of all supported event types by this device.
        /// </summary>
        List<EventType> GetSupportedEventTypes();

        /// <summary>
        /// Get list of all supported event types by this device names.
        /// </summary>
        List<string> GetSupportedEventTypesNames();
    }
}
