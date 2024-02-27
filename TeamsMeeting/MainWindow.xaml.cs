using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput.Native;
using WindowsInput;

namespace TeamsMeeting
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Join(object sender, RoutedEventArgs e)
        {
            JoinMeeting();
        }
        private void JoinMeeting()
        {
            string teamsMeetingUrl = TextBox.Text;

            if (IsValidTeamsMeetingUrl(teamsMeetingUrl))
            {
                try
                {

                    string chromePath = GetChromePathWin11();

                    if (chromePath != null)
                    {
                        string urlToOpen = TextBox.Text;

                        string arguments = "--incognito --start-maximized";
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = chromePath,
                            Arguments = $"{arguments} \"{urlToOpen}\"",
                            UseShellExecute = true,
                            WindowStyle = ProcessWindowStyle.Normal
                        });

                    }
                    else
                    {
                        MessageBox.Show("Google Chrome is not installed or the path not found !");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening chrome..: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Invalid URL..!");
            }
            
        }
        static bool IsValidTeamsMeetingUrl(string url)
        {
            string teamsMeetingPattern = @"^https:\/\/teams\.microsoft\.com";

            return Regex.IsMatch(url, teamsMeetingPattern);
        } // validating if the given link is valid or not
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            TextBox.Clear();
        }
        static string GetChromePathWin11()
        {
            string keyName = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";

            string chromePath = GetRegistryValue(RegistryHive.LocalMachine, keyName, "");

            if (string.IsNullOrEmpty(chromePath))
            {
                chromePath = GetRegistryValue(RegistryHive.LocalMachine, keyName, "", RegistryView.Registry32);
            }

            return chromePath;
        } //find the path for chrome in windows 11 and 10
        static string GetRegistryValue(RegistryHive hive, string subKey, string valueName, RegistryView registryView = RegistryView.Registry64)
        {
            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(hive, registryView))
                {
                    using (var key = baseKey.OpenSubKey(subKey))
                    {
                        if (key != null)
                        {
                            var value = key.GetValue(valueName) as string;
                            return value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accessing registry: " + ex.Message);
            }

            return null;
        }

    }

}
