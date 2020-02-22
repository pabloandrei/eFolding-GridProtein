using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

[assembly: CLSCompliant(true)]
namespace GridProteinFolding.Middle.Helpers.RegistryHelpers
{
    public class RegistryHelpers
    {
        public static object ReturnKey(string keyPath,string keyName )
        {
            return RegistryHelpers.GetRegistryValue(keyPath, keyName);
        }

        public static RegistryKey GetRegistryKey()
        {
            return GetRegistryKey(null);
        }

        public static RegistryKey GetRegistryKey(string keyPath)
        {
            RegistryKey localMachineRegistry
                = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                          Environment.Is64BitOperatingSystem
                                              ? RegistryView.Registry64
                                              : RegistryView.Registry32);

            return string.IsNullOrEmpty(keyPath)
                ? localMachineRegistry
                : localMachineRegistry.OpenSubKey(keyPath);
        }

        public static object GetRegistryValue(string keyPath, string keyName)
        {
            RegistryKey registry = GetRegistryKey(keyPath);
            return registry.GetValue(keyName);
        }
    }
}
