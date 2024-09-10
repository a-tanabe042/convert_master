using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("librust_app.dylib", CallingConvention = CallingConvention.Cdecl)]
    public static extern void hello_from_rust();

    static void Main()
    {
        hello_from_rust();
        Console.WriteLine("Hello from C#!");
    }
}
