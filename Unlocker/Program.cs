using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Runtime;

internal class Program
{
    public static void Main(string[] args)
    {
        if (IsAdministrator() == true)
        {
            Console.WriteLine("Enter your username, (expamle:s17179) :");
            var username = Console.ReadLine();
            Cmdexecute("reg load HKU\\" + username + " C:\\Users\\" + username + "\\ntuser.dat");

            Console.WriteLine("----Enable themes, dir, windows store, background, style----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKLM\\SOFTWARE\\Policies\\Microsoft\\WindowsStore /v RemoveWindowsStore /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer /v DisablePersonalDirChange /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer /v NoThemesTab /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v NoDispBackgroundPage /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v NoSizeChoice /f"    );
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v NoVisualStyleChoice /f");

            Console.WriteLine("----Enable wallpaper and sign options-----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\ActiveDesktop /v NoChangingWallPaper /t REG_DWORD /d 0 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKLM\\SOFTWARE\\Microsoft\\PolicyManager\\default\\Settings\\AllowSignInOptions /v value /t REG_DWORD /d 1 /f");

            Console.WriteLine("----Enable xbox game bar----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\GameDVR /v AppCaptureEnabled /t REG_DWORD /d 1 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKCU\\System\\GameConfigStore /v GameDVR_Enabled /t REG_DWORD /d 1 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKCU\\SOFTWARE\\Policies\\Microsoft\\Windows\\GameDVR /v AllowGameDVR /t REG_DWORD /d 1 /f");

            Console.WriteLine("----Unlock contorl panel----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer /v DisallowCpl /t REG_DWORD /d 0 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\DisallowCpl /va /f");

            Console.WriteLine("----Enable mouse pointers, lockscreen, homegroup----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKCU\\Software\\Policies\\Microsoft\\Windows\\Personalization /v NoChangingMousePointers /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\Personalization /v NoChangingLockScreen /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG DELETE HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\HomeGroup /v DisableHomeGroup /f");

            Console.WriteLine("----Enable netcache----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\NetCache /v Enabled /t REG_DWORD /d 1 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\NetCache /v NoConfigCache /t REG_DWORD /d 0 /f");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\NetCache /v NoMakeAvailableOffline /t REG_DWORD /d 0 /f");

            Console.WriteLine("----Stop gvsc----");
            Cmdexecute("cd /d C:\\Windows\\system32&&REG ADD HKLM\\SYSTEM\\CurrentControlSet\\services\\gpsvc /t REG_DWORD /d 4 /f");

            Cmdexecute("reg unload HKU\\" + username);

            Console.WriteLine("Execution completed, any key to exit");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }

    public static void Cmdexecute(string command)
    {
        // Create a new process to execute the command
        Process process = new Process();

        // Set the process start info
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "cmd.exe"; // Specify the command prompt executable
        startInfo.RedirectStandardInput = true; // Redirect input/output
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false; // Don't use the shell to execute the command
        startInfo.CreateNoWindow = true; // Don't create a command prompt window

        process.StartInfo = startInfo;
        process.Start();

        // Send the command to the command prompt
        process.StandardInput.WriteLine(command);
        process.StandardInput.Flush();
        process.StandardInput.Close();

        // Read the output of the command
        string output = process.StandardOutput.ReadToEnd();

        // Wait for the command to finish executing
        process.WaitForExit();

        // Display the output
        Console.WriteLine(output);
    }

    public static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}