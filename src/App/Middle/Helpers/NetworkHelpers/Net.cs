using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Globalization;
using System.Security;

[assembly: CLSCompliant(true)]
//[assembly: AllowPartiallyTrustedCallers]
namespace GridProteinFolding.Middle.Helpers.NetworkHelpers
{
    public class Net
    {
        ~Net()
        {
            System.GC.SuppressFinalize(this);
        }

        public struct NetworkInterfaces { 
            public string hostName;
            public List<string> macAdress;
        }
        public static void ShowNetworkInterfaces(ref NetworkInterfaces networkInterfaces)
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //GICO.WriteLine("Interface information for {0}.{1}     ",
            //        computerProperties.HostName, computerProperties.DomainName);

            networkInterfaces.hostName = computerProperties.HostName;

            if (nics == null || nics.Length < 1)
            {
                //GICO.WriteLine("  No network interfaces found.");
                return;
            }

            //GICO.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {

                    IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();
                    //GICO.WriteLine();
                    //GICO.WriteLine(adapter.Description);
                    //GICO.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                    //GICO.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                    //GICO.Write("  Physical address ........................ : ");

                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    string tempMac = string.Empty;
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        // Display the physical address in hexadouble.
                        tempMac += bytes[i].ToString("X2");
                        // Insert a hyphen after each byte, unless we are at the end of the 
                        // address.
                        if (i != bytes.Length - 1)
                        {
                            tempMac += "-";
                        }
                    }
                    networkInterfaces.macAdress.Add(tempMac);
                    //GICO.Write(macAdress);

                    //GICO.WriteLine();
                }
            }
        }

        public static PhysicalAddress GetMacAddress()
        {
            //ONLINE
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }

            //OFFLINE
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }
    }
}
