using Ionic.Zip;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WinDroid_Universal_Android_Toolkit
{
    public partial class DownloadWindow : MetroWindow
    {
        public DownloadWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new WebClient();
                var client2 = new WebClient();
                var client3 = new WebClient();
                switch (Settings.Selector)
                {
                    case "ADB":
                        {
                            this.Title = "Downloading Drivers...";
                            client.DownloadProgressChanged += (client_DownloadProgressChanged);
                            client.DownloadFileCompleted += (client_DownloadFileCompleted);
                            client.DownloadFileAsync(
                                new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/ADBDriver.msi"),
                                "./Data/Installers/ADBDriver.msi");
                        }
                        break;

                    case "Recovery":
                        {
                            if (Settings.ThreeRecoveries == true)
                            {
                                client.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery1.img"),
                                    "./Data/Recoveries/Recovery1.img");
                                client2.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery2.img"),
                                    "./Data/Recoveries/Recovery2.img");
                                client3.DownloadProgressChanged += (client_DownloadProgressChanged);
                                client3.DownloadFileCompleted += (client_DownloadFileCompleted);
                                client3.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery3.img"),
                                    "./Data/Recoveries/Recovery3.img");
                            }
                            else if (Settings.TwoRecoveries == true)
                            {
                                client.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery1.img"),
                                    "./Data/Recoveries/Recovery1.img");
                                client2.DownloadProgressChanged += (client_DownloadProgressChanged);
                                client2.DownloadFileCompleted += (client_DownloadFileCompleted);
                                client2.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery2.img"),
                                    "./Data/Recoveries/Recovery2.img");
                            }
                            else
                            {
                                client.DownloadProgressChanged += (client_DownloadProgressChanged);
                                client.DownloadFileCompleted += (client_DownloadFileCompleted);
                                client.DownloadFileAsync(
                                    new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + Settings.Device + "/Recovery.img"),
                                    "./Data/Recoveries/Recovery1.img");
                            }
                        }
                        break;

                    case "One X AT&T":
                        {
                            this.Title = "Downloading SuperCID...";
                            client.DownloadProgressChanged += (client_DownloadProgressChanged);
                            client.DownloadFileCompleted += (client_DownloadFileCompleted);
                            client.DownloadFileAsync(
                                new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/One_X/SuperCID.zip"),
                                "./Data/Installers/SuperCID.zip");
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            this.Title = "Downloading Program...";
                            client.DownloadProgressChanged += (client_DownloadProgressChanged);
                            client.DownloadFileCompleted += (client_DownloadFileCompleted);
                            client.DownloadFileAsync(
                                new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Droid_Incredible_4G_LTE/FireballUnlock.zip"),
                                "./Data/Installers/FireballUnlock.zip");
                        }
                        break;

                    case "Desire 610":
                        {
                            this.Title = "Downloading Program...";
                            client.DownloadProgressChanged += (client_DownloadProgressChanged);
                            client.DownloadFileCompleted += (client_DownloadFileCompleted);
                            client.DownloadFileAsync(
                                new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Desire_610/Desire_610.exe"),
                                "./Data/Installers/Desire_610.exe");
                        }
                        break;
                }
                Settings.Device = null;
                Settings.TwoRecoveries = false;
                Settings.ThreeRecoveries = false;
            }
            catch (Exception ex)
            {
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                double bytesIn = e.BytesReceived;
                double totalBytes = e.TotalBytesToReceive;
                double percentage = bytesIn / totalBytes * 100;
                progressBar.Value = int.Parse(Math.Truncate(percentage).ToString());
                progressBar.Value = (int)Math.Truncate(percentage);
            }
            catch (Exception ex)
            {
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBox.Show(
                        @"An error occured while attempting to download the necessary files! Please restart the toolkit, check your internet connection and try again in a few minutes.",
                        @"Download Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (File.Exists("./Data/Recoveries/Recovery1.img"))
                    {
                        File.Delete("./Data/Recoveries/Recovery1.img");
                    }
                    if (File.Exists("./Data/Recoveries/Recovery2.img"))
                    {
                        File.Delete("./Data/Recoveries/Recovery2.img");
                    }
                    if (File.Exists("./Data/Recoveries/Recovery3.img"))
                    {
                        File.Delete("./Data/Recoveries/Recovery3.img");
                    }
                    Settings.Selector = "Recovery";
                    Close();
                }
                else if (Settings.Selector == "ADB")
                {
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/ADBDriver.msi");
                    Settings.Selector = "Recovery";
                    Close();
                }
                else if (Settings.Selector == "One X AT&T")
                {
                    using (ZipFile zip = ZipFile.Read("./Data/Installers/SuperCID.zip"))
                    {
                        zip.ExtractAll("./");
                    }
                    File.Delete("./Data/Installers/SuperCID.zip");
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/run.bat");
                    Settings.Selector = "Recovery";
                    Close();
                }
                else if (Settings.Selector == "Droid Incredible 4G LTE")
                {
                    using (ZipFile zip = ZipFile.Read("./Data/Installers/FireballUnlock.zip"))
                    {
                        zip.ExtractAll("./");
                    }
                    File.Delete("./Data/Installers/FireballUnlock.zip");
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/RunMe.bat");
                    MessageBox.Show(
                        @"You only need to complete Steps 1 and 2. After that, you can continue on to the next steps in the toolkit.",
                        @"Just a heads up.", MessageBoxButton.OK, MessageBoxImage.Information);
                    Settings.Selector = "Recovery";
                    Close();
                }
                else if (Settings.Selector == "Desire 610")
                {
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/Desire_610.exe");
                    Settings.Selector = "Recovery";
                    Close();
                }
                else
                {
                    Settings.Selector = "Recovery";
                    Close();
                }
            }
            catch (Exception ex)
            {
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        #region Nested type: Settings

        public static class Settings
        {
            public static string Selector = "Recovery";
            public static string Device;
            public static bool TwoRecoveries;
            public static bool ThreeRecoveries;
        }

        #endregion Nested type: Settings
    }
}