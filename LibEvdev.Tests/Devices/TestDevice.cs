// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using LibEvdev.Devices;

namespace LibEvdev.Tests.Devices;

public class TestDevice
{
    [Theory]
    [InlineData("/dev/input/event3")]
    [InlineData("/dev/input/event4")]
    [InlineData("/dev/input/event1337")]
    public void TestIsValidDevicePath_Good(string path)
    {
        Assert.True(Device.IsValidDevicePath(path));
    }

    [Theory]
    [InlineData("/dev/input/event")]
    [InlineData("/dev/input/event9.0")]
    [InlineData("/dev/input/event9,0")]
    [InlineData("/dev/input/event9.txt")]
    [InlineData("/dev/input/event-1", Skip = "allow only non-negative numbers")]
    [InlineData("/dev/input/event*")]
    public void TestIsValidDevicePath_Bad(string path)
    {
        Assert.False(Device.IsValidDevicePath(path));
    }
}
