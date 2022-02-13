using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AvalonMessageBox
{
    internal class LocalizedStrings
    {
        public LocalizedStrings()
        {
            using (var loader = new Loader())
            {

                Localization.Add(Buttons.Ok, loader.LoadString(800));
                Localization.Add(Buttons.Cancel, loader.LoadString(801));
                Localization.Add(Buttons.Abort, loader.LoadString(802));
                Localization.Add(Buttons.Retry, loader.LoadString(803));
                Localization.Add(Buttons.Ignore, loader.LoadString(804));
                Localization.Add(Buttons.Yes, loader.LoadString(805));
                Localization.Add(Buttons.No, loader.LoadString(806));
                Localization.Add(Buttons.Close, loader.LoadString(807));
            }
            // Help 808
            // TryAgain 809
            // Continue 810
        }

        public IDictionary<Buttons, string> Localization { get; } = new Dictionary<Buttons, string>();

        private class Loader : IDisposable
        {
            private readonly IntPtr instance;
            private readonly StringBuilder buffer;

            public Loader(int bufferSize = 32)
            {
                instance = LoadLibraryEx("user32.dll", IntPtr.Zero, 0);
                buffer = new StringBuilder(bufferSize);
            }

            public string LoadString(int id)
            {
                LoadString(instance, (uint)id, buffer, buffer.Capacity - 1);
                return buffer.ToString().Replace('&', '_');
            }

            public void Dispose()
            {
                FreeLibrary(instance);
            }

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);
        }
    }
}