using System;
using Microsoft.Win32;

namespace WebLib
{
    public class ApplicationRegistryHandler
    {
        public void WriteToRegistry(string registryName, string keyName, string keyValue)
        {
            try
            {
                RegistryKey registryKey1 = Registry.LocalMachine;
                if (registryKey1.OpenSubKey(registryName) == null)
                    registryKey1.CreateSubKey(registryName);
                RegistryKey registryKey2 = registryKey1.OpenSubKey(registryName, true);
                registryKey2.SetValue(keyName, (object)keyValue);
                registryKey1.Close();
                registryKey2.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan pada penulisan ke registry. Error Message : " + ex.Message);
            }
        }

        public string ReadFromRegistry(string registryName, string keyName)
        {
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(registryName, true);
                if (registryKey == null)
                    return (string)null;
                else
                    return Convert.ToString(registryKey.GetValue(keyName));
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan pada membaca registry. Error Message : " + ex.Message);
            }
        }
    }
}