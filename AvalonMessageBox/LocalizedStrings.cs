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
                AddLocalization(Buttons.Ok, 800);
                AddLocalization(Buttons.Cancel, 801);
                AddLocalization(Buttons.Abort, 802);
                AddLocalization(Buttons.Retry, 803);
                AddLocalization(Buttons.Ignore, 804);
                AddLocalization(Buttons.Yes, 805);
                AddLocalization(Buttons.No, 806);
                AddLocalization(Buttons.Close, 807);
                AddLocalization(Buttons.Help, 808);
                AddLocalization(Buttons.TryAgain, 809);
                AddLocalization(Buttons.Continue, 810);

                void AddLocalization(Buttons button, int id)
                {
                    Localization.Add(button, loader.LoadString(id));
                }
            }
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
                return buffer.Replace('&', '_').ToString();
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