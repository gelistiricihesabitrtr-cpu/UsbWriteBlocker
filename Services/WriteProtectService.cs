using System;
using Microsoft.Win32;

namespace UsbWriteBlocker.Services
{
    /// <summary>
    /// HKLM\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies altındaki
    /// WriteProtect DWORD değerini yönetir. Bu değer 1 olduğunda Windows,
    /// sisteme bağlı tüm USB kitle depolama aygıtlarını salt-okunur olarak işler.
    /// </summary>
    public static class WriteProtectService
    {
        private const string KeyPath = @"SYSTEM\CurrentControlSet\Control\StorageDevicePolicies";
        private const string ValueName = "WriteProtect";

        public static bool IsWriteBlockActive()
        {
            using var key = Registry.LocalMachine.OpenSubKey(KeyPath, writable: false);
            if (key == null)
            {
                return false;
            }

            var value = key.GetValue(ValueName);
            if (value is int intValue)
            {
                return intValue == 1;
            }

            return false;
        }

        public static void Enable()
        {
            using var key = Registry.LocalMachine.CreateSubKey(KeyPath, writable: true);
            key.SetValue(ValueName, 1, RegistryValueKind.DWord);
        }

        public static void Disable()
        {
            using var key = Registry.LocalMachine.CreateSubKey(KeyPath, writable: true);
            key.SetValue(ValueName, 0, RegistryValueKind.DWord);
        }
    }
}
