using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public static class NativeImport
{
    [DllImport("TestDll")]
    public extern static bool TestFunc();
}
