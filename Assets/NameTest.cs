using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

public class NameTest : MonoBehaviour
{

    private string username;

    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("UserName: " + Environment.UserName);
        //Debug.Log("UserDomainName: " + Environment.UserDomainName);
        //Debug.Log("MachineName: " + Environment.MachineName);

        //Debug.Log("DisplayName: " + UserPrincipal.Current.DisplayName);

        try
        {
            if (IsRunningOnMac())
            {
                Debug.Log("DeviceName: " + SystemInfo.deviceName);
                username = SystemInfo.deviceName;
            }
            else
            {
                Debug.Log("UserName: " + Environment.UserName);
                username = System.Environment.UserName;
                //text.text = "UserName: " + System.Environment.UserName + "\nUserDomainName: " + System.Environment.UserDomainName
                //+ "\nMachineName: " + System.Environment.MachineName;
                //text.text = "DisplayName: " + UserPrincipal.Current.DisplayName;

                //text.text = "1: " + UserPrincipal.Current.GivenName + "\n2: " + UserPrincipal.Current.Name
                //+ "\n3: " + UserPrincipal.Current.UserPrincipalName + "\n3: " + UserPrincipal.Current.DistinguishedName;

                //text.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                //text.text = WindowsIdentity.GetCurrent();
                //text.text = "1: " + UserPrincipal.Current.Name;
            }
            //username = "Tamas, Flaviu R.";
            //username = "O'Bryan Moore";

            if (username != null)
            {
                if (username.Contains(','))
                {
                    username = username.Split(',')[1];
                    username = username.Substring(1);
                    Debug.Log(username);
                }

                if (username.Contains(' '))
                {
                    username = username.Split(' ')[0];
                    //username = username.Substring(1);
                    Debug.Log(username);
                }

                //if (!username.Any(char.IsDigit))
                //{
                GameControl.YourName = username;
                //}
            }
        }
        catch
        {
        }

 
        

        //Debug.Log(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));

        //text.text = "UserName: " + System.Environment.UserName + "\nUserDomainName: " + System.Environment.UserDomainName
        //+ "\nMachineName: " + System.Environment.MachineName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //From Managed.Windows.Forms/XplatUI
    [DllImport("libc")]
    static extern int uname(IntPtr buf);

    static bool IsRunningOnMac()
    {
        IntPtr buf = IntPtr.Zero;
        try
        {
            buf = Marshal.AllocHGlobal(8192);
            // This is a hacktastic way of getting sysname from uname ()
            if (uname(buf) == 0)
            {
                string os = Marshal.PtrToStringAnsi(buf);
                if (os == "Darwin")
                    return true;
            }
        }
        catch
        {
        }
        finally
        {
            if (buf != IntPtr.Zero)
                Marshal.FreeHGlobal(buf);
        }
        return false;
    }


}
