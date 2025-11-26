// Copyright (c) NiyazBiyaz <niyazik114422@gmail.com>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LibEvdev.Tests;

public class TestAutoExternalException
{
    [Fact]
    public void TestSetErrorCodeViaMarshal()
    {
        Marshal.SetLastPInvokeError(10);
        ExternalException? exc;

        try
        {
            throw AutoExternalException.New();
        }
        catch (ExternalException e)
        {
            exc = e;
        }

        Debug.Assert(exc is not null);
        Assert.Equal(10, exc.ErrorCode);
    }

    [Fact]
    public void TestSetErrorCodeManually()
    {
        ExternalException? exc;

        try
        {
            throw AutoExternalException.New(10);
        }
        catch (ExternalException e)
        {
            exc = e;
        }

        Debug.Assert(exc is not null);
        Assert.Equal(10, exc.ErrorCode);
    }
}
