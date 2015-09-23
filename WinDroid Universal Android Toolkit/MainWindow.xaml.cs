using AutoUpdaterDotNET;
using Ionic.Zip;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using RegawMOD.Android;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using WinDroid_Universal_Android_Toolkit.Models;

namespace WinDroid_Universal_Android_Toolkit
{
    public partial class MainWindow : MetroWindow
    {
        private AndroidController _android;
        private Device _device;
        private LogModel _Log = new LogModel();

        public class Thing
        {
            public string Name { get; set; }
            public string Manufacturer { get; set; }
        }
        System.Collections.ObjectModel.ObservableCollection<Thing> deviceList;

        public MainWindow()
        {
            InitializeComponent();
            logBox.DataContext = Log;

            deviceList = new System.Collections.ObjectModel.ObservableCollection<Thing>()
            {
                new Thing{ Name="Amaze", Manufacturer="HTC"},
                new Thing{ Name="Butterfly", Manufacturer="HTC"},
                new Thing{ Name="Butterfly 2", Manufacturer="HTC"},
                new Thing{ Name="Butterfly S", Manufacturer="HTC"},
                new Thing{ Name="Desire 200", Manufacturer="HTC"},
                new Thing{ Name="Desire 210", Manufacturer="HTC"},
                new Thing{ Name="Desire 300", Manufacturer="HTC"},
                new Thing{ Name="Desire 500", Manufacturer="HTC"},
                new Thing{ Name="Desire 510", Manufacturer="HTC"},
                new Thing{ Name="Desire 601", Manufacturer="HTC"},
                new Thing{ Name="Desire 610", Manufacturer="HTC"},
                new Thing{ Name="Desire 612", Manufacturer="HTC"},
                new Thing{ Name="Desire 616", Manufacturer="HTC"},
                new Thing{ Name="Desire 626", Manufacturer="HTC"},
                new Thing{ Name="Desire 816", Manufacturer="HTC"},
                new Thing{ Name="Desire 820", Manufacturer="HTC"},
                new Thing{ Name="Desire 826", Manufacturer="HTC"},
                new Thing{ Name="Desire C", Manufacturer="HTC"},
                new Thing{ Name="Desire Eye", Manufacturer="HTC"},
                new Thing{ Name="Desire HD", Manufacturer="HTC"},
                new Thing{ Name="Desire S", Manufacturer="HTC"},
                new Thing{ Name="Desire SV", Manufacturer="HTC"},
                new Thing{ Name="Desire V", Manufacturer="HTC"},
                new Thing{ Name="Desire X", Manufacturer="HTC"},
                new Thing{ Name="Droid DNA", Manufacturer="HTC"},
                new Thing{ Name="Droid Incredible", Manufacturer="HTC"},
                new Thing{ Name="Droid Incredible 2", Manufacturer="HTC"},
                new Thing{ Name="Droid Incredible 4G LTE", Manufacturer="HTC"},
                new Thing{ Name="Droid Incredible S", Manufacturer="HTC"},
                new Thing{ Name="EVO 3D", Manufacturer="HTC"},
                new Thing{ Name="EVO 4G", Manufacturer="HTC"},
                new Thing{ Name="EVO 4G LTE", Manufacturer="HTC"},
                new Thing{ Name="EVO Design", Manufacturer="HTC"},
                new Thing{ Name="EVO Shift 4G", Manufacturer="HTC"},
                new Thing{ Name="EVO Explorer", Manufacturer="HTC"},
                new Thing{ Name="EVO First", Manufacturer="HTC"},
                new Thing{ Name="myTouch 4G Slide", Manufacturer="HTC"},
                new Thing{ Name="One E8", Manufacturer="HTC"},
                new Thing{ Name="One J", Manufacturer="HTC"},
                new Thing{ Name="One E8", Manufacturer="HTC"},
                new Thing{ Name="One E9+", Manufacturer="HTC"},
                new Thing{ Name="One M7", Manufacturer="HTC"},
                new Thing{ Name="One M7 (Dual SIM)", Manufacturer="HTC"},
                new Thing{ Name="One M8", Manufacturer="HTC"},
                new Thing{ Name="One M9", Manufacturer="HTC"},
                new Thing{ Name="One Max", Manufacturer="HTC"},
                new Thing{ Name="One Mini", Manufacturer="HTC"},
                new Thing{ Name="One Mini 2", Manufacturer="HTC"},
                new Thing{ Name="One Remix", Manufacturer="HTC"},
                new Thing{ Name="One S", Manufacturer="HTC"},
                new Thing{ Name="One SV", Manufacturer="HTC"},
                new Thing{ Name="One V", Manufacturer="HTC"},
                new Thing{ Name="One VX", Manufacturer="HTC"},
                new Thing{ Name="One X", Manufacturer="HTC"},
                new Thing{ Name="One X+", Manufacturer="HTC"},
                new Thing{ Name="Rezound", Manufacturer="HTC"},
                new Thing{ Name="Sensation", Manufacturer="HTC"},
                new Thing{ Name="Sensation XL", Manufacturer="HTC"},
                new Thing{ Name="Vivid", Manufacturer="HTC"},
                new Thing{ Name="Wildfire", Manufacturer="HTC"},
                new Thing{ Name="Wildfire S", Manufacturer="HTC"},
                new Thing{ Name="Moto E", Manufacturer="Motorola"},
                new Thing{ Name="Moto E (2015)", Manufacturer="Motorola"},
                new Thing{ Name="Moto G", Manufacturer="Motorola"},
                new Thing{ Name="Moto G (2014)", Manufacturer="Motorola"},
                new Thing{ Name="Moto G (2015)", Manufacturer="Motorola"},
                new Thing{ Name="Moto Maxx", Manufacturer="Motorola"},
                new Thing{ Name="Moto X", Manufacturer="Motorola"},
                new Thing{ Name="Moto X (2014)", Manufacturer="Motorola"},
                new Thing{ Name="Moto X Play", Manufacturer="Motorola"},
                new Thing{ Name="Moto X Style (Pure)", Manufacturer="Motorola"},
                new Thing{ Name="Photon Q", Manufacturer="Motorola"},
                new Thing{ Name="Xoom", Manufacturer="Motorola"},
                new Thing{ Name="Galaxy Nexus", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 4", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 5", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 6", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 7 (2012)", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 7 (2013)", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 9", Manufacturer="Nexus"},
                new Thing{ Name="Nexus 10", Manufacturer="Nexus"},
                new Thing{ Name="Nexus S", Manufacturer="Nexus"},
                new Thing{ Name="Nexus Player", Manufacturer="Nexus"},
                new Thing{ Name="Shield", Manufacturer="Nvidia"},
                new Thing{ Name="Shield Tablet", Manufacturer="Nvidia"},
                new Thing{ Name="Tegra Note 7", Manufacturer="Nvidia"},
                new Thing{ Name="Find 5", Manufacturer="Oppo"},
                new Thing{ Name="Find 7/7a", Manufacturer="Oppo"},
                new Thing{ Name="N1", Manufacturer="Oppo"},
                new Thing{ Name="N3", Manufacturer="Oppo"},
                new Thing{ Name="R819", Manufacturer="Oppo"},
                new Thing{ Name="G Watch", Manufacturer="Smartwatches"},
                new Thing{ Name="G Watch R", Manufacturer="Smartwatches"},
                new Thing{ Name="Gear Live", Manufacturer="Smartwatches"},
                new Thing{ Name="Moto 360", Manufacturer="Smartwatches"},
                new Thing{ Name="Smartwatch 3", Manufacturer="Smartwatches"},
                new Thing{ Name="Mi 3", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi Note Pro", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi Pad", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi 1S", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi 2", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi Note", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi Note 2", Manufacturer="Xiaomi"},
                new Thing{ Name="Android One", Manufacturer="Other"},
                new Thing{ Name="OnePlus One", Manufacturer="Other"},
                new Thing{ Name="OnePlus 2", Manufacturer="Other"},
                new Thing{ Name="OneTouch Idol 3", Manufacturer="Other"},
                new Thing{ Name="XOLO Q1010i", Manufacturer="Other"},
                new Thing{ Name="YU Yureka", Manufacturer="Other"},
                new Thing{ Name="Zenfone 2", Manufacturer="Other"},
                new Thing{ Name="None", Manufacturer="Other"},
            };
            System.ComponentModel.ICollectionView view = System.Windows.Data.CollectionViewSource.GetDefaultView(deviceList);
            view.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Manufacturer"));
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Manufacturer", System.ComponentModel.ListSortDirection.Ascending));
            view.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            phoneListBox.ItemsSource = view;

        }

        public LogModel Log
        {
            get { return _Log; }
            set { _Log = value; }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void CheckFileSystem()
        {
            try
            {
                string[] neededDirectories = new string[] { "Data/", "Data/Installers", "Data/Logs", "Data/Recoveries" };

                foreach (string dir in neededDirectories)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Log.AddLogItem("File directory created.", "FILE");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private static string GetStringBetween(string source, string start, string end)
        {
            int startIndex = source.IndexOf(start, StringComparison.InvariantCulture) + start.Length;
            int endIndex = source.IndexOf(end, startIndex, StringComparison.InvariantCulture);
            int length = endIndex - startIndex;
            return source.Substring(startIndex, length);
        }

        private void LogBaseSystemInfo()
        {
            Log.AddLogItem(Environment.OSVersion + " " + (Environment.Is64BitOperatingSystem ? "64Bit" : "32bit"),
                "INFO");
            Log.AddLogItem(
                Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version,
                "INFO");
            foreach (AssemblyName referencedAssembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Log.AddLogItem(referencedAssembly.Name + " " + referencedAssembly.Version, "INFO");
            }
        }

        private void TogglePhoneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
        }

        private void ToggleLogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((Flyout)Flyouts.Items[1]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
        }

        private async void SaveLogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Log.AddLogItem("Log saved.", "LOG");
            string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
            var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
            file.WriteLine(logBox.Text);
            file.Close();
            await this.ShowMessageAsync("Log Saved!", "A copy of this log has been saved in the Logs folder.",
                        MessageDialogStyle.Affirmative);
        }

        private async Task DownloadRecoveries(string device, int variants)
        {
            Log.AddLogItem("Recovery download queued.", "DOWNLOAD");
            ProgressDialogController controller = await this.ShowProgressAsync("Downloading...", "The latest TWRP recovery is being queued for download. If this message is here for longer than 5 minutes, please close the toolkit and check your internet connection before trying again.");
            controller.SetCancelable(false);

            var client = new WebClient();
            var client2 = new WebClient();
            var client3 = new WebClient();

            client3.DownloadProgressChanged += (s, e) =>
            {
                controller.SetMessage(e.ProgressPercentage + "% Completed.");
                double progress = 0;
                if (e.ProgressPercentage > progress)
                {
                    controller.SetProgress(e.ProgressPercentage / 100.0d);
                    progress = e.ProgressPercentage;
                }
            };

            client3.DownloadFileCompleted += async (s, e) =>
            {
                await controller.CloseAsync();
                if (e.Cancelled)
                {
                    Log.AddLogItem("Recovery download cancelled.", "DOWNLOAD");
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
                }
                if (e.Error != null)
                {
                    Log.AddLogItem("Recovery download error.", "DOWNLOAD");
                    await this.ShowMessageAsync("Error!", "An error occured while attempting to download the file! Please restart the toolkit, check your internet connection and try again in a few minutes.",
                            MessageDialogStyle.Affirmative);
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
                }
                else
                {
                    Log.AddLogItem("Recovery download completed.", "DOWNLOAD");
                }
            };

            Log.AddLogItem("Recovery download started.", "DOWNLOAD");
            if (variants == 1)
            {
                await client3.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery.img"), "./Data/Recoveries/Recovery1.img");
                client3.Dispose();
            }
            else if (variants == 2)
            {
                await client2.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery1.img"), "./Data/Recoveries/Recovery1.img");
                await client3.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery2.img"), "./Data/Recoveries/Recovery2.img");
                client2.Dispose();
                client3.Dispose();
            }
            else if (variants == 3)
            {
                await client.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery1.img"), "./Data/Recoveries/Recovery1.img");
                await client2.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery2.img"), "./Data/Recoveries/Recovery2.img");
                await client3.DownloadFileTaskAsync(("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/" + device + "/Recovery3.img"), "./Data/Recoveries/Recovery3.img");
                client.Dispose();
                client2.Dispose();
                client3.Dispose();
            }
        }

        private async Task DownloadFile(string name, string url, string path)
        {
            Log.AddLogItem("Download of " + name + " started.", "DOWNLOAD");
            ProgressDialogController controller = await this.ShowProgressAsync("Downloading File...", "");
            controller.SetCancelable(false);

            var client = new WebClient();
            client.DownloadProgressChanged += (s, e) =>
            {
                controller.SetMessage(e.ProgressPercentage + "% Completed.");
                double progress = 0;
                if (e.ProgressPercentage > progress)
                {
                    controller.SetProgress(e.ProgressPercentage / 100.0d);
                    progress = e.ProgressPercentage;
                }
            };

            client.DownloadFileCompleted += async (s, e) =>
            {
                if (e.Error != null)
                {
                    await this.ShowMessageAsync("Error!", "An error occured while attempting to download the file! Please restart the toolkit, check your internet connection and try again in a few minutes.",
                       MessageDialogStyle.Affirmative);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else
                {
                    Log.AddLogItem("Download of " + name + " completed.", "DOWNLOAD");
                }
            };

            await client.DownloadFileTaskAsync((url), path);
            client.Dispose();
            await controller.CloseAsync();
        }

        private async void UpdateDevice()
        {
            _android = AndroidController.Instance;
            try
            {
                Log.AddLogItem("Detecting Device...", "DEVICE");
                await TaskEx.Run(() => _android.UpdateDeviceList());
                if (await TaskEx.Run(() => _android.HasConnectedDevices))
                {
                    _device = await TaskEx.Run(() => _android.GetConnectedDevice(_android.ConnectedDevices[0]));
                    switch (_device.State.ToString())
                    {
                        case "ONLINE":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Online";
                                statusEllipse.Fill = Brushes.Green;
                                Log.AddLogItem("Connected: Online.", "DEVICE");
                            });
                            break;

                        case "FASTBOOT":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Fastboot";
                                statusEllipse.Fill = Brushes.Blue;
                                Log.AddLogItem("Connected: Fastboot.", "DEVICE");
                            });
                            break;

                        case "RECOVERY":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Recovery";
                                statusEllipse.Fill = Brushes.Purple;
                                Log.AddLogItem("Connected: Recovery.", "DEVICE");
                            });
                            break;

                        case "SIDELOAD":
                            this.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                statusLabel.Content = "Sideload";
                                statusEllipse.Fill = Brushes.Orange;
                                Log.AddLogItem("Connected: Sideload.", "DEVICE");
                            });
                            break;

                        case "UNAUTHORIZED":
                            this.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                statusLabel.Content = "Unauthorized";
                                statusEllipse.Fill = Brushes.Orange;
                                Log.AddLogItem("Connected: Unauthorized.", "DEVICE");
                            });
                            break;

                        case "UNKNOWN":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Unknown";
                                statusEllipse.Fill = Brushes.Gray;
                                Log.AddLogItem("Connected: Unknown.", "DEVICE");
                            });
                            break;
                    }
                }
                else
                {
                    this.Dispatcher.BeginInvoke((Action)delegate()
                    {
                        statusLabel.Content = "Offline";
                        statusEllipse.Fill = Brushes.Red;
                        Log.AddLogItem("No Device Found.", "DEVICE");
                    });
                }
                await TaskEx.Run(() => _android.Dispose());
            }
            catch (Exception ex)
            {
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogBaseSystemInfo();

            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.Start("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Update.xml");
            Log.AddLogItem("AutoUpdater started.", "UPDATE");

            if (!Directory.Exists("./Data"))
            {
                CheckFileSystem();
            }

            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                if (!Directory.Exists("C:/Program Files (x86)/ClockworkMod/Universal Adb Driver") &&
                    !Directory.Exists("C:/Program Files/ClockworkMod/Universal Adb Driver"))
                {
                    if (Properties.Settings.Default["ADB"].ToString() == "Yes")
                    {
                        MessageDialogResult result = await this.ShowMessageAsync("ADB Drivers", "You are missing some ADB Drivers! They are required for your device to connect with your computer. Would you like to install them now?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        switch (result)
                        {
                            case MessageDialogResult.Affirmative:
                                await DownloadFile("Drivers", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/ADBDriver.msi", "./Data/Installers/ADBDriver.msi");
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/ADBDriver.msi");
                                break;

                            case MessageDialogResult.Negative:
                                MessageDialogResult result2 = await this.ShowMessageAsync("ADB Reminder", "Would you like to be reminded the next time you open the toolkit?",
                                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                                switch (result2)
                                {
                                    case MessageDialogResult.Affirmative:
                                        Properties.Settings.Default["ADB"] = "Yes";
                                        Properties.Settings.Default.Save();
                                        break;

                                    case MessageDialogResult.Negative:
                                        Properties.Settings.Default["ADB"] = "No";
                                        Properties.Settings.Default.Save();
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }

            if (Properties.Settings.Default["Device"].ToString() == "None")
            {
                var controller9 = await this.ShowProgressAsync("Detecting Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
                PhoneTextBox.Text = "Please choose your device!";
            }
            else
            {
                PhoneTextBox.Text = "Current Device: " + Properties.Settings.Default["Device"].ToString();
                getTokenIDButton.IsEnabled = true;
                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                gainRootButton.IsEnabled = true;
                switch (Properties.Settings.Default["Device"].ToString())
                {
                    case "Android One":
                    case "Droid DNA":
                    case "One Remix":
                    case "G Watch":
                    case "G Watch R":
                    case "Gear Live":
                    case "Moto 360":
                    case "Nexus 4":
                    case "Nexus 5":
                    case "Nexus 6":
                    case "Nexus 9":
                    case "Nexus 10":
                    case "OnePlus One":
                    case "OnePlus 2":
                    case "Shield":
                    case "Shield Tablet":
                    case "Smartwatch 3":
                    case "Tegra Note 7":
                    case "Xoom":
                    case "YU Yureka":
                    case "Zenfone 2":
                    case "Zenwatch":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Find 5":
                    case "Find 7/7a":
                    case "Mi 3":
                    case "Mi Note Pro":
                    case "Mi Pad":
                    case "Oppo N1":
                    case "Oppo N3":
                    case "Oppo R819":
                    case "Redmi 1S":
                    case "Redmi 2":
                    case "Redmi Note 2":
                    case "XOLO Q1010i":
                        {
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Moto E":
                    case "Moto E (2015)":
                    case "Moto G (2015)":
                    case "Moto Maxx":
                    case "Moto X":
                    case "Moto X Play":
                    case "Moto X Style (Pure)":
                    case "Photon Q":
                        {
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G":
                    case "Moto G (2014)":
                        {
                            getTokenIDButton.Content = "Get Unlock Key";
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "Desire 210":
                    case "Desire 616":
                    case "Desire SV":
                        {
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Butterfly":
                        {
                            recovery1Button.Content = "Flash TWRP (x920d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (x920e)";
                        }
                        break;

                    case "Desire 601":
                        {
                            recovery1Button.Content = "Flash TWRP (Zara)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (ZaraCL)";
                        }
                        break;

                    case "Desire 610":
                        {
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.Content = "Gain Root";
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            gainSuperCIDButton.IsEnabled = true;
                        }
                        break;

                    case "EVO 3D":
                        {
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "Galaxy Nexus":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Sprint)";
                        }
                        break;

                    case "Moto X (2014)":
                        {
                            getTokenIDButton.Content = "Get Unlock Key";
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "Nexus 7 (2012)":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (3G)";
                        }
                        break;

                    case "Nexus 7 (2013)":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                        }
                        break;

                    case "Nexus Player":
                        {
                            getTokenIDButton.IsEnabled = false;
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "Nexus S":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "One M7":
                        {
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "Verizon M7 Only";
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Sprint)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Verizon)";
                        }
                        break;

                    case "One M7 (Dual SIM)":
                        {
                            recovery1Button.Content = "Flash TWRP (802d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (802t)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (802w)";
                        }
                        break;

                    case "One M8":
                        {
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "Verizon M8 Only";
                        }
                        break;

                    case "One Max":
                        {
                            recovery1Button.Content = "Flash TWRP (Sprint)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                        }
                        break;

                    case "One S":
                        {
                            recovery1Button.Content = "Flash TWRP (S4)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (S3)";
                        }
                        break;

                    case "One SV":
                        {
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Boost)";
                        }
                        break;

                    case "One V":
                        {
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "One X":
                        {
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "AT&T One X Only";
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "One X+":
                        {
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "OneTouch Idol 3":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (6045)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (6039)";
                        }
                        break;

                    case "Redmi Note":
                        {
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;
                }
                Log.AddLogItem("Device: " + Properties.Settings.Default["Device"].ToString() + ".", "DEVICE");
                var controller = await this.ShowProgressAsync("Detecting Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
        }

        private async void phoneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                gainSuperCIDButton.IsEnabled = false;
                gainSuperCIDButton.Content = "Gain SuperCID";
                getTokenIDButton.IsEnabled = true;
                getTokenIDButton.Content = "Get Token ID";
                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                recovery2Button.IsEnabled = false;
                recovery2Button.Content = "Option Two";
                recovery3Button.IsEnabled = false;
                recovery3Button.Content = "Option Three";
                gainRootButton.IsEnabled = true;
                gainRootButton.Content = "Flash SuperSU";
                Thing lbi = ((sender as ListBox).SelectedItem as Thing);
                string device = lbi.Name.ToString();
                switch (device)
                {
                    case "Amaze":
                        {
                            await DownloadRecoveries("Amaze", 1);
                        }
                        break;

                    case "Android One":
                        {
                            await DownloadRecoveries("Android_One", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Butterfly":
                        {
                            await DownloadRecoveries("Butterfly", 2);
                            recovery1Button.Content = "Flash TWRP (x920d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (x920e)";
                        }
                        break;

                    case "Butterfly 2":
                        {
                            await DownloadRecoveries("Butterfly_2", 1);
                        }
                        break;

                    case "Butterfly S":
                        {
                            await DownloadRecoveries("Butterfly_S", 1);
                        }
                        break;

                    case "Desire 200":
                        {
                            await DownloadRecoveries("Desire_200", 1);
                        }
                        break;

                    case "Desire 210":
                        {
                            await DownloadRecoveries("Desire_210", 1);
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire 300":
                        {
                            await DownloadRecoveries("Desire_300", 1);
                        }
                        break;

                    case "Desire 500":
                        {
                            await DownloadRecoveries("Desire_500", 1);
                        }
                        break;

                    case "Desire 510":
                        {
                            await DownloadRecoveries("Desire_510", 1);
                        }
                        break;

                    case "Desire 601":
                        {
                            await DownloadRecoveries("Desire_601", 2);
                            recovery1Button.Content = "Flash TWRP (Zara)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (ZaraCL)";
                        }
                        break;

                    case "Desire 610":
                        {
                            await DownloadRecoveries("Desire_610", 1);
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.Content = "Gain Root";
                        }
                        break;

                    case "Desire 612":
                        {
                            await DownloadRecoveries("Desire_612", 1);
                        }
                        break;

                    case "Desire 616":
                        {
                            await DownloadRecoveries("Desire_616", 1);
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire 626":
                        {
                            await DownloadRecoveries("Desire_626", 1);
                        }
                        break;

                    case "Desire 816":
                        {
                            await DownloadRecoveries("Desire_816", 1);
                        }
                        break;

                    case "Desire 820":
                        {
                            await DownloadRecoveries("Desire_820", 1);
                        }
                        break;

                    case "Desire 826":
                        {
                            await DownloadRecoveries("Desire_826", 1);
                        }
                        break;

                    case "Desire C":
                        {
                            await DownloadRecoveries("Desire_C", 1);
                        }
                        break;

                    case "Desire Eye":
                        {
                            await DownloadRecoveries("Desire_Eye", 1);
                        }
                        break;

                    case "Desire HD":
                        {
                            await DownloadRecoveries("Desire_HD", 1);
                        }
                        break;

                    case "Desire S":
                        {
                            await DownloadRecoveries("Desire_S", 1);
                        }
                        break;

                    case "Desire SV":
                        {
                            await DownloadRecoveries("Desire_SV", 1);
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire V":
                        {
                            await DownloadRecoveries("Desire_V", 1);
                        }
                        break;

                    case "Desire X":
                        {
                            await DownloadRecoveries("Desire_X", 1);
                        }
                        break;

                    case "Droid DNA":
                        {
                            await DownloadRecoveries("Droid_DNA", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Droid Incredible":
                        {
                            await DownloadRecoveries("Droid_Incredible", 1);
                        }
                        break;

                    case "Droid Incredible 2":
                        {
                            await DownloadRecoveries("Droid_Incredible_2", 1);
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            await DownloadRecoveries("Droid_Incredible_4G_LTE", 1);
                            gainSuperCIDButton.IsEnabled = true;
                        }
                        break;

                    case "Droid Incredible S":
                        {
                            await DownloadRecoveries("Droid_Incredible_S", 1);
                        }
                        break;

                    case "EVO 3D":
                        {
                            await DownloadRecoveries("EVO_3D", 2);
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "EVO 4G":
                        {
                            await DownloadRecoveries("EVO_4G", 1);
                        }
                        break;

                    case "EVO 4G LTE":
                        {
                            await DownloadRecoveries("EVO_4G_LTE", 1);
                        }
                        break;

                    case "EVO Design":
                        {
                            await DownloadRecoveries("EVO_Design", 1);
                        }
                        break;

                    case "EVO Shift 4G":
                        {
                            await DownloadRecoveries("EVO_Shift_4G", 1);
                        }
                        break;

                    case "Explorer":
                        {
                            await DownloadRecoveries("Explorer", 1);
                        }
                        break;

                    case "First":
                        {
                            await DownloadRecoveries("First", 1);
                        }
                        break;

                    case "Find 5":
                        {
                            await DownloadRecoveries("Find_5", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Find 7/7a":
                        {
                            await DownloadRecoveries("Find_7", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "G Watch":
                        {
                            await DownloadRecoveries("G_Watch", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "G Watch R":
                        {
                            await DownloadRecoveries("G_Watch_R", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Galaxy Nexus":
                        {
                            await DownloadRecoveries("Galaxy_Nexus", 3);
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Sprint)";
                        }
                        break;

                    case "Gear Live":
                        {
                            await DownloadRecoveries("Gear_Live", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Mi 3":
                        {
                            await DownloadRecoveries("Mi_3", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Mi Note Pro":
                        {
                            await DownloadRecoveries("Mi_Note_Pro", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Mi Pad":
                        {
                            await DownloadRecoveries("Mi_Pad", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Moto 360":
                        {
                            await DownloadRecoveries("Moto_360", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Moto E":
                        {
                            await DownloadRecoveries("Moto_E", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto E (2015)":
                        {
                            await DownloadRecoveries("Moto_E_2015", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G":
                        {
                            await DownloadRecoveries("Moto_G", 2);
                            getTokenIDButton.Content = "Get Unlock Key";
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "Moto G (2014)":
                        {
                            await DownloadRecoveries("Moto_G_2014", 2);
                            getTokenIDButton.Content = "Get Unlock Key";
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                        }
                        break;

                    case "Moto G (2015)":
                        {
                            await DownloadRecoveries("Moto_G_2015", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto Maxx":
                        {
                            await DownloadRecoveries("Moto_Maxx", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X":
                        {
                            await DownloadRecoveries("Moto_X", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X (2014)":
                        {
                            await DownloadRecoveries("Moto_X_2014", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "Moto X Play":
                        {
                            await DownloadRecoveries("Moto_X_Play", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X Style (Pure)":
                        {
                            await DownloadRecoveries("Moto_X_Style", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "myTouch 4G Slide":
                        {
                            await DownloadRecoveries("myTouch_4G_Slide", 1);
                        }
                        break;

                    case "Nexus 4":
                        {
                            await DownloadRecoveries("Nexus_4", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 5":
                        {
                            await DownloadRecoveries("Nexus_5", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 6":
                        {
                            await DownloadRecoveries("Nexus_6", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 7 (2012)":
                        {
                            await DownloadRecoveries("Nexus_7_2012", 2);
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (3G)";
                        }
                        break;

                    case "Nexus 7 (2013)":
                        {
                            await DownloadRecoveries("Nexus_7_2013", 2);
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                        }
                        break;

                    case "Nexus 9":
                        {
                            await DownloadRecoveries("Nexus_9", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 10":
                        {
                            await DownloadRecoveries("Nexus_10", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus Player":
                        {
                            await DownloadRecoveries("Nexus_Player", 1);
                            getTokenIDButton.IsEnabled = false;
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "Nexus S":
                        {
                            await DownloadRecoveries("Nexus_S", 1);
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "One E8":
                        {
                            await DownloadRecoveries("One_E8", 1);
                        }
                        break;

                    case "One E9+":
                        {
                            await DownloadRecoveries("One_E9", 1);
                        }
                        break;

                    case "One J":
                        {
                            await DownloadRecoveries("One_J", 1);
                        }
                        break;

                    case "One M7":
                        {
                            await DownloadRecoveries("One_M7", 3);
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "Verizon M7 Only";
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Sprint)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Verizon)";
                        }
                        break;

                    case "One M7 (Dual SIM)":
                        {
                            await DownloadRecoveries("One_M7_Dual_SIM", 3);
                            recovery1Button.Content = "Flash TWRP (802d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (802t)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (802w)";
                        }
                        break;

                    case "One M8":
                        {
                            await DownloadRecoveries("One_M8", 1);
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "Verizon M8 Only";
                        }
                        break;

                    case "One M9":
                        {
                            await DownloadRecoveries("One_M9", 1);
                        }
                        break;

                    case "One Max":
                        {
                            await DownloadRecoveries("One_Max", 2);
                            recovery1Button.Content = "Flash TWRP (Sprint)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                        }
                        break;

                    case "One Mini":
                        {
                            await DownloadRecoveries("One_Mini", 1);
                        }
                        break;

                    case "One Mini 2":
                        {
                            await DownloadRecoveries("One_Mini_2", 1);
                        }
                        break;

                    case "One Remix":
                        {
                            await DownloadRecoveries("One_Remix", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "One S":
                        {
                            await DownloadRecoveries("One_S", 2);
                            recovery1Button.Content = "Flash TWRP (S4)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (S3)";
                        }
                        break;

                    case "One SV":
                        {
                            await DownloadRecoveries("One_SV", 3);
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Boost)";
                        }
                        break;

                    case "One V":
                        {
                            await DownloadRecoveries("One_V", 2);
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "One VX":
                        {
                            await DownloadRecoveries("One_VX", 1);
                            gainSuperCIDButton.IsEnabled = false;
                        }
                        break;

                    case "One X":
                        {
                            await DownloadRecoveries("One_X", 2);
                            gainSuperCIDButton.Content = "AT&T One X Only";
                            gainSuperCIDButton.IsEnabled = true;
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "One X+":
                        {
                            await DownloadRecoveries("One_X_Plus", 2);
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "OnePlus One":
                        {
                            await DownloadRecoveries("OnePlus_One", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "OnePlus 2":
                        {
                            await DownloadRecoveries("OnePlus_2", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "OneTouch Idol 3":
                        {
                            await DownloadRecoveries("OneTouch_Idol_3", 2);
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (6045)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (6039)";
                        }
                        break;

                    case "Oppo N1":
                        {
                            await DownloadRecoveries("Oppo_N1", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Oppo N3":
                        {
                            await DownloadRecoveries("Oppo_N3", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Oppo R819":
                        {
                            await DownloadRecoveries("Oppo_R819", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Photon Q":
                        {
                            await DownloadRecoveries("Photon_Q", 1);
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Redmi 1S":
                        {
                            await DownloadRecoveries("Redmi_1S", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Redmi 2":
                        {
                            await DownloadRecoveries("Redmi_2", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Redmi Note":
                        {
                            await DownloadRecoveries("Redmi_Note", 2);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "Redmi Note 2":
                        {
                            await DownloadRecoveries("Redmi_Note_2", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Rezound":
                        {
                            await DownloadRecoveries("Rezound", 1);
                        }
                        break;

                    case "Sensation":
                        {
                            await DownloadRecoveries("Sensation", 1);
                        }
                        break;

                    case "Sensation XL":
                        {
                            await DownloadRecoveries("Sensation_XL", 1);
                        }
                        break;

                    case "Shield":
                        {
                            await DownloadRecoveries("Shield", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Shield Tablet":
                        {
                            await DownloadRecoveries("Shield_Tablet", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Smartwatch 3":
                        {
                            await DownloadRecoveries("Smartwatch_3", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Tegra Note 7":
                        {
                            await DownloadRecoveries("Tegra_Note_7", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Vivid":
                        {
                            await DownloadRecoveries("One_E8", 1);
                        }
                        break;

                    case "Wildfire":
                        {
                            await DownloadRecoveries("Wildfire", 1);
                        }
                        break;

                    case "Wildfire S":
                        {
                            await DownloadRecoveries("Wildfire_S", 1);
                        }
                        break;

                    case "XOLO Q1010i":
                        {
                            await DownloadRecoveries("XOLO_Q1010i", 1);
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Xoom":
                        {
                            await DownloadRecoveries("Xoom", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "YU Yureka":
                        {
                            await DownloadRecoveries("YU_Yureka", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Zenfone 2":
                        {
                            await DownloadRecoveries("Zenfone_2", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Zenwatch":
                        {
                            await DownloadRecoveries("Zenwatch", 1);
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "None":
                        {
                            getTokenIDButton.IsEnabled = false;
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.IsEnabled = false;
                            gainRootButton.IsEnabled = false;
                        }
                        break;
                }
                Properties.Settings.Default["Device"] = device;
                Properties.Settings.Default.Save();
                ((Flyout)Flyouts.Items[0]).IsOpen = false;
                PhoneTextBox.Text = "Current Device: " + device;
                Log.AddLogItem("Device changed to " + device + ".", "DEVICE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void gainSuperCIDButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                var device = Properties.Settings.Default["Device"].ToString();
                if (device == "One M7" || device == "One M8")
                {
                    MessageDialogResult result = await this.ShowMessageAsync("Verizon Sucks!", "Due to restrictions put in place by Verizon, you must utilize another program to unlock your phone. Would you like to try this method?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start("http://theroot.ninja/");
                    }
                    Log.AddLogItem("One M7/M8 Verizon unlocking method clicked.", "SUPERCID");
                }
                else if (device == "One X")
                {
                    MessageDialogResult result = await this.ShowMessageAsync("AT&T Sucks!", "Due to restrictions put in place by AT&T, you must gain SuperCID on your phone through a special program. Would you like to download this program now?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "SUPERCID");
                            if (File.Exists("./run.bat"))
                            {
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/run.bat");
                                Log.AddLogItem("AT&T One X SuperCID program opened.", "SUPERCID");
                            }
                            else
                            {
                                await DownloadFile("AT&T One X SuperCID", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/One_X/SuperCID.zip", "./Data/Installers/SuperCID.zip");
                                using (ZipFile zip = ZipFile.Read("./Data/Installers/SuperCID.zip"))
                                {
                                    zip.ExtractAll("./");
                                    Log.AddLogItem("AT&T One X SuperCID zip extracted.", "SUPERCID");
                                }
                                File.Delete("./Data/Installers/SuperCID.zip");
                                Log.AddLogItem("AT&T One X SuperCID zip deleted.", "SUPERCID");
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/run.bat");
                                Log.AddLogItem("AT&T One X SuperCID program opened.", "SUPERCID");
                            }
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "SUPERCID");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                        }
                    }
                }
                else if (device == "Droid Incredible 4G LTE")
                {
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "SUPERCID");
                        MessageDialogResult result = await this.ShowMessageAsync("Verizon Sucks!", "Due to restrictions put in place by Verizon, you must gain SuperCID on your phone through a special program. Would you like to download this program now?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            if (File.Exists("./RunMe.bat"))
                            {
                                await this.ShowMessageAsync("Heads Up!", "You only need to complete Step 1. After that, you can continue on to the next steps in the toolkit.",
                                    MessageDialogStyle.Affirmative);
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/RunMe.bat");
                                Log.AddLogItem("DI 4G LTE SuperCID program opened.", "SUPERCID");
                            }
                            else
                            {
                                await DownloadFile("DI 4G LTE SuperCID", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Droid_Incredible_4G_LTE/FireballUnlock.zip", "./Data/Installers/FireballUnlock.zip");
                                using (ZipFile zip = ZipFile.Read("./Data/Installers/FireballUnlock.zip"))
                                {
                                    zip.ExtractAll("./");
                                    Log.AddLogItem("DI 4G LTE SuperCID program extracted.", "SUPERCID");
                                }
                                File.Delete("./Data/Installers/FireballUnlock.zip");
                                Log.AddLogItem("DI 4G LTE SuperCID zip deleted.", "SUPERCID");
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/RunMe.bat");
                                Log.AddLogItem("DI 4G LTE SuperCID program opened.", "SUPERCID");
                                await this.ShowMessageAsync("Heads Up!", "You only need to complete Step 1. After that, you can continue on to the next steps in the toolkit.",
                                    MessageDialogStyle.Affirmative);
                            }
                        }
                        else
                        {
                            Process.Start("http://forum.xda-developers.com/showthread.php?t=2214653");
                            Log.AddLogItem("DI 4G LTE SuperCID link opened.", "SUPERCID");
                        }
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "SUPERCID");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                                    MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private void getTokenIDButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string device = Properties.Settings.Default["Device"].ToString();
                if (device == "Moto E" || device == "Moto E (2015)" || device == "Moto G" || device == "Moto G (2014)" || device == "Moto G (2015)" || device == "Moto Maxx" || device == "Moto X" || device == "Moto X (2014)" || device == "Moto X Play" || device == "Moto X Style (Pure)" || device == "Photon Q")
                {
                    MotorolaUnlockKey();
                }
                else
                {
                    HTCTokenID();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void HTCTokenID()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("It's Token Time!", "This command will get your device's Token ID, then open a text file with your token and further instructions. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                Log.AddLogItem("HTC Token ID command started.", "HTCTOKEN");
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    Log.AddLogItem("Connected: Online.", "HTCTOKEN");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    Log.AddLogItem("Rebooting to bootloader.", "HTCTOKEN");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Getting Token ID...");
                    using (StreamWriter sw = File.CreateText("./Data/token.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "HTCTOKEN");
                        string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_identifier_token")));
                        Log.AddLogItem("Getting token ID.", "HTCTOKEN");
                        string rawToken = GetStringBetween(rawReturn, "< Please cut following message >\r\n",
                            "\r\nOKAY");
                        string cleanedToken = rawToken.Replace("(bootloader) ", "");
                        sw.WriteLine(cleanedToken);
                        sw.WriteLine("Please copy everything above this line!");
                        sw.WriteLine(" ");
                        sw.WriteLine("Next, sign into your HTC Dev account on the webpage that just opened.");
                        sw.WriteLine("If you do not have an account, create and activate an account with your email, then come back to this link.");
                        sw.WriteLine("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        sw.WriteLine("Then, paste the Token ID you just copied at the bottom of the webpage.");
                        sw.WriteLine("Hit submit, and wait for the email with the unlock file.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Once you have received the unlock file, download it and continue on to the next step, unlocking your bootloader.");
                        sw.WriteLine("This file is saved as token.txt in the Data folder if you need it in the future.");
                        Log.AddLogItem("Writing token ID to text file.", "HTCTOKEN");
                    }
                    await TaskEx.Run(() => UpdateDevice());
                    await controller.CloseAsync();
                    MessageDialogResult result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    Log.AddLogItem("Token ID retrieval successful.", "HTCTOKEN");
                    if (result2 == MessageDialogResult.Affirmative)
                    {
                        var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                        Log.AddLogItem("Rebooting device.", "HTCTOKEN");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller2.CloseAsync();
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        Log.AddLogItem("Token ID text file opened.", "HTCTOKEN");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                    MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        Log.AddLogItem("Token ID text file opened.", "HTCTOKEN");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                    MessageDialogStyle.Affirmative);
                    }
                }
                else if (statusLabel.Content.ToString() == "Fastboot")
                {
                    Log.AddLogItem("Connected: Fastboot.", "HTCTOKEN");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Getting Token ID...");
                    using (StreamWriter sw = File.CreateText("./Data/token.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "HTCTOKEN");
                        string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_identifier_token")));
                        Log.AddLogItem("Getting token ID.", "HTCTOKEN");
                        string rawToken = GetStringBetween(rawReturn, "< Please cut following message >\r\n",
                            "\r\nOKAY");
                        string cleanedToken = rawToken.Replace("(bootloader) ", "");
                        sw.WriteLine(cleanedToken);
                        sw.WriteLine("Please copy everything above this line!");
                        sw.WriteLine(" ");
                        sw.WriteLine("Next, sign into your HTC Dev account on the webpage that just opened.");
                        sw.WriteLine("If you do not have an account, create and activate an account with your email, then come back to this link.");
                        sw.WriteLine("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        sw.WriteLine("Then, paste the Token ID you just copied at the bottom of the webpage.");
                        sw.WriteLine("Hit submit, and wait for the email with the unlock file.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Once you have received the unlock file, download it and continue on to the next step, unlocking your bootloader.");
                        sw.WriteLine("This file is saved as token.txt in the Data folder if you need it in the future.");
                        Log.AddLogItem("Writing token ID to text file.", "HTCTOKEN");
                    }
                    await TaskEx.Run(() => UpdateDevice());
                    await controller.CloseAsync();
                    MessageDialogResult result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    Log.AddLogItem("Token ID retrieval successful.", "HTCTOKEN");
                    if (result2 == MessageDialogResult.Affirmative)
                    {
                        var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                        Log.AddLogItem("Rebooting device.", "HTCTOKEN");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller2.CloseAsync();
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                    MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                    MessageDialogStyle.Affirmative);
                    }
                }
                else
                {
                    Log.AddLogItem("No device found.", "HTCTOKEN");
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                        MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void MotorolaUnlockKey()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Get Unlock String!", "This command will get your device's unlock string, then open a text file with your string and further instructions. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                Log.AddLogItem("Motorola Unlock Key command started.", "MOTOKEY");
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    Log.AddLogItem("Connected: Online.", "MOTOKEY");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    Log.AddLogItem("Rebooting to bootloader.", "MOTOKEY");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Getting Unlock String...");
                    using (StreamWriter sw = File.CreateText("./Data/unlockstring.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "MOTOKEY");
                        string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_unlock_data")));
                        Log.AddLogItem("Getting unlock string.", "MOTOKEY");
                        string rawToken = GetStringBetween(rawReturn, "...\r\n",
                            "\r\nOKAY");
                        string firstCleanedToken = rawToken.Replace("(bootloader) ", "");
                        File.WriteAllText("./Data/unlockstringRAW.txt", firstCleanedToken);
                        Log.AddLogItem("Writing raw unlock string.", "MOTOKEY");
                        string secondCleanedToken = File.ReadAllText(@"./Data/unlockstringRAW.txt").Replace(Environment.NewLine, " ");
                        string finalCleanedToken = secondCleanedToken.Replace(" ", "");
                        sw.WriteLine(finalCleanedToken);
                        sw.WriteLine("Please copy the entire code above this line!");
                        sw.WriteLine(" ");
                        sw.WriteLine("Next, sign into your Google or Motorola account on the webpage that just opened.");
                        sw.WriteLine("If you have not logged in before, activate your account then come back to this link.");
                        sw.WriteLine("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        sw.WriteLine("Then, paste the unlock string you just copied near the bottom of the webpage under Step 6.");
                        sw.WriteLine("Click on 'Can my device be unlocked?', then check 'I Agree' under the legal agreement.");
                        sw.WriteLine("Click on 'Request Unlock Key' at the bottom of the page, then wait for the email with your unlock key.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Once you have received the unlock key in your email, copy it and continue on to the next step, unlocking your bootloader.");
                        sw.WriteLine("This file is saved as 'unlockstring.txt' in the Data folder if you need it in the future.");
                        Log.AddLogItem("Writing unlock string to text file.", "MOTOKEY");
                    }
                    await TaskEx.Run(() => UpdateDevice());
                    await controller.CloseAsync();
                    File.Delete("./Data/unlockstringRAW.txt");
                    MessageDialogResult result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    Log.AddLogItem("Unlock string retrieval successful.", "MOTOKEY");
                    if (result2 == MessageDialogResult.Affirmative)
                    {
                        var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                        Log.AddLogItem("Rebooting device.", "MOTOKEY");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller2.CloseAsync();
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlocks string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlocks string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                    }
                }
                else if (statusLabel.Content.ToString() == "Fastboot")
                {
                    Log.AddLogItem("Connected: Fastboot.", "MOTOKEY");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Getting Unlock String...");
                    using (StreamWriter sw = File.CreateText("./Data/unlockstring.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "MOTOKEY");
                        string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_unlock_data")));
                        Log.AddLogItem("Getting unlock string.", "MOTOKEY");
                        string rawToken = GetStringBetween(rawReturn, "...\r\n",
                            "\r\nOKAY");
                        string firstCleanedToken = rawToken.Replace("(bootloader) ", "");
                        File.WriteAllText("./Data/unlockstringRAW.txt", firstCleanedToken);
                        Log.AddLogItem("Writing raw unlock string.", "MOTOKEY");
                        string secondCleanedToken = File.ReadAllText(@"./Data/unlockstringRAW.txt").Replace(Environment.NewLine, " ");
                        string finalCleanedToken = secondCleanedToken.Replace(" ", "");
                        sw.WriteLine(finalCleanedToken);
                        sw.WriteLine("Please copy the entire code above this line!");
                        sw.WriteLine(" ");
                        sw.WriteLine("Next, sign into your Google or Motorola account on the webpage that just opened.");
                        sw.WriteLine("If you have not logged in before, activate your account then come back to this link.");
                        sw.WriteLine("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        sw.WriteLine("Then, paste the unlock string you just copied near the bottom of the webpage under Step 6.");
                        sw.WriteLine("Click on 'Can my device be unlocked?', then check 'I Agree' under the legal agreement.");
                        sw.WriteLine("Click on 'Request Unlock Key' at the bottom of the page, then wait for the email with your unlock key.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Once you have received the unlock key in your email, copy it and continue on to the next step, unlocking your bootloader.");
                        sw.WriteLine("This file is saved as 'unlockstring.txt' in the Data folder if you need it in the future.");
                        Log.AddLogItem("Writing unlock string to text file.", "MOTOKEY");
                    }
                    await TaskEx.Run(() => UpdateDevice());
                    await controller.CloseAsync();
                    File.Delete("./Data/unlockstringRAW.txt");
                    MessageDialogResult result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    Log.AddLogItem("Unlock string retrieval successful.", "MOTOKEY");
                    if (result2 == MessageDialogResult.Affirmative)
                    {
                        var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                        Log.AddLogItem("Rebooting device.", "MOTOKEY");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller2.CloseAsync();
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlocks string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlocks string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                    }
                }
                else
                {
                    Log.AddLogItem("No device found.", "MOTOKEY");
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                        MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void unlockBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            try
            {
                string device = Properties.Settings.Default["Device"].ToString();
                if (device == "Droid DNA" || device == "One Remix")
                {
                    MessageDialogResult result = await this.ShowMessageAsync("Verizon Sucks!", "Your device cannot be unlocked through this toolkit. However, you can utilize another method to unlock your device. Would you like to try this alternate method now?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start("http://theroot.ninja/");
                        Log.AddLogItem("Sunshine website opened.", "VERIZON");
                    }
                }
                else if (device == "Moto E" || device == "Moto E (2015)" || device == "Moto G" || device == "Moto G (2014)" || device == "Moto G (2015)" || device == "Moto Maxx" || device == "Moto X" || device == "Moto X (2014)" || device == "Moto X Play" || device == "Moto X Style (Pure)" || device == "Photon Q")
                {
                    MotorolaUnlock();
                }
                else if (device == "Android One" || device == "G Watch" || device == "G Watch R" || device == "Galaxy Nexus" || device == "Gear Live" || device == "Moto 360" || device == "Nexus 4" || device == "Nexus 5" || device == "Nexus 6" || device == "Nexus 7 (2012)" || device == "Nexus 7 (2013)" || device == "Nexus 9" || device == "Nexus 10" || device == "Nexus Player" || device == "Nexus S" || device == "OnePlus One" || device == "OneTouch Idol 3" || device == "OnePlus 2" || device == "Shield" || device == "Shield Tablet" || device == "Smartwatch 3" || device == "Tegra Note 7" || device == "Xoom" || device == "YU Yureka" || device == "Zenwatch")
                {
                    AOSPDeviceUnlock();
                }
                else if (device == "Zenfone 2")
                {
                    ZenfoneUnlock();
                }
                else
                {
                    HTCDeviceUnlock();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void MotorolaUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe your device. You must have the unlock key from the email from Motorola. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                Log.AddLogItem("Motorola unlock command started.", "MOTOUNLOCK");
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    Log.AddLogItem("Connected: Online.", "MOTOUNLOCK");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    Log.AddLogItem("Rebooting device to bootloader.", "MOTOUNLOCK");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    await controller.CloseAsync();
                    var unlockKey = await this.ShowInputAsync("Paste Unlock Key", "Please paste the unlock key exactly as given from the email sent to you by Motorola. It is case-sensitive, and should contain no spaces.");
                    if (unlockKey == null)
                        return;
                    Log.AddLogItem("User pasting unlock key.", "MOTOUNLOCK");
                    var controller2 = await this.ShowProgressAsync("Unlocking Bootloader...", "");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock " + unlockKey)));
                    Log.AddLogItem("Unlocking bootloader.", "MOTOUNLOCK");
                    await TaskEx.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                    Log.AddLogItem("Bootloader unlock successful.", "MOTOUNLOCK");
                }
                else if (statusLabel.Content.ToString() == "Fastboot")
                {
                    Log.AddLogItem("Connected: Fastboot.", "MOTOUNLOCK");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    await controller.CloseAsync();
                    var unlockKey = await this.ShowInputAsync("Paste Unlock Key", "Please paste the unlock key exactly as given from the email sent to you by Motorola. It is case-sensitive, and should contain no spaces.");
                    if (unlockKey == null)
                        return;
                    Log.AddLogItem("User pasting unlock key.", "MOTOUNLOCK");
                    var controller2 = await this.ShowProgressAsync("Unlocking Bootloader...", "");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock " + unlockKey)));
                    Log.AddLogItem("Unlocking bootloader.", "MOTOUNLOCK");
                    await TaskEx.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                    Log.AddLogItem("Bootloader unlock successful.", "MOTOUNLOCK");
                }
                else
                {
                    Log.AddLogItem("No device found.", "MOTOUNLOCK");
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                        MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void AOSPDeviceUnlock()
        {
            string device = Properties.Settings.Default["Device"].ToString();
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock!", "This will unlock your bootloader and completely wipe your device. Please ensure that you have backed up all necessary files. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                Log.AddLogItem("AOSP unlock command started.", "AOSPUNLOCK");
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    Log.AddLogItem("Connected: Online.", "AOSPUNLOCK");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    Log.AddLogItem("Rebooting device to bootloader.", "AOSPUNLOCK");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Unlocking Bootloader...");
                    controller.SetMessage("Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button confirm. Once you've done this, click 'Ok'.");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    if (device == "Nexus Player")
                    {
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                        Log.AddLogItem("Unlocking bootloader.", "ATVUNLOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "ATVUNLOCK");
                    }
                    else
                    {
                        Log.AddLogItem("Unlocking bootloader.", "AOSPUNLOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "AOSPUNLOCK");
                    }
                }
                else if (statusLabel.Content.ToString() == "Fastboot")
                {
                    Log.AddLogItem("Connected: Fastboot.", "AOSPUNLOCK");
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Unlocking Bootloader...");
                    controller.SetMessage("Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button confirm. Once you've done this, click 'Ok'.");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    if (device == "Nexus Player")
                    {
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                        Log.AddLogItem("Unlocking bootloader.", "ATVUNLOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "ATVUNLOCK");
                    }
                    else
                    {
                        Log.AddLogItem("Unlocking bootloader.", "AOSPUNLOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "AOSPUNLOCK");
                    }
                }
                else
                {
                    Log.AddLogItem("No device found.", "AOSPUNLOCK");
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void HTCDeviceUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe your device. You must have downloaded the unlock_code.bin file from HTC. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                Log.AddLogItem("HTC unlock command started.", "HTCUNLOCK");
                OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "HTC Unlock Files (*.bin*)|*.bin*", Multiselect = false };
                ofd.ShowDialog();
                if (File.Exists(ofd.FileName))
                {
                    Log.AddLogItem("HTC unlock token selected.", "HTCUNLOCK");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "HTCUNLOCK");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        Log.AddLogItem("Rebooting device to bootloader.", "HTCUNLOCK");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing Unlock File...");
                        controller.SetMessage("Your device will now display a screen asking you about unlocking. Please read it carefully. Use your volume buttons to choose Yes, then press the power button confirm the unlock.");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlocktoken " + ofd.FileName)));
                        Log.AddLogItem("Unlocking bootloader.", "HTCUNLOCK");
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "HTCUNLOCK");
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        Log.AddLogItem("Connected: Fastboot.", "HTCUNLOCK");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing Unlock File...");
                        controller.SetMessage("Your device will now display a screen asking you about unlocking. Please read it carefully. Use your volume buttons to choose Yes, then press the power button confirm the unlock.");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlocktoken " + ofd.FileName)));
                        Log.AddLogItem("Unlocking bootloader.", "HTCUNLOCK");
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader unlock successful.", "HTCUNLOCK");
                    }
                    else
                    {
                        Log.AddLogItem("No Device Found.", "HTCUNLOCK");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
        }

        private async void ZenfoneUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock?", "This will download and run a program that will unlock your bootloader and completely wipe your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online" || statusLabel.Content.ToString() == "Fastboot")
                {
                    Log.AddLogItem("Connected: Online.", "UNLOCK");
                    if (File.Exists("./unlock_one_click_v2.bat"))
                    {
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/unlock_one_click_v2.bat");
                        Log.AddLogItem("Zenfone 2 unlock program started.", "UNLOCK");
                    }
                    else
                    {
                        await DownloadFile("Zenfone 2 Unlock Program", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Zenfone_2/Unlock.zip", "./Unlock.zip");
                        using (ZipFile zip = ZipFile.Read("./Unlock.zip"))
                        {
                            zip.ExtractAll("./");
                            Log.AddLogItem("Zenfone 2 unlock zip extracted.", "UNLOCK");
                        }
                        File.Delete("./Unlock.zip");
                        Log.AddLogItem("Zenfone 2 unlock zip deleted.", "UNLOCK");
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/unlock_one_click_v2.bat");
                        Log.AddLogItem("Zenfone 2 unlock program opened.", "UNLOCK");
                    }
                }
                else
                {
                    Log.AddLogItem("No device found.", "UNLOCK");
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                MessageDialogStyle.Affirmative);
                }
            }
        }

        private void firstRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.AddLogItem("First recovery flash clicked.", "RECOVERY");
                FlashRecovery("Recovery1.img");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private void secondRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.AddLogItem("Second recovery flash started.", "RECOVERY");
                FlashRecovery("Recovery2.img");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private void thirdRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.AddLogItem("Third recovery flash started.", "RECOVERY");
                FlashRecovery("Recovery3.img");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void FlashRecovery(string recovery)
        {
            if (File.Exists("./Data/Recoveries/" + recovery))
            {
                string device = Properties.Settings.Default["Device"].ToString();
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Recovery flash command started.", "RECOVERY");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash the TWRP recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "RECOVERY");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        if (device == "One M9")
                        {
                            await TaskEx.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                            Log.AddLogItem("Rebooting device to download mode.", "RECOVERY M9");
                        }
                        else
                        {
                            await TaskEx.Run(() => _device.RebootBootloader());
                            Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                        }
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing TWRP...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + "./Data/Recoveries/" + recovery)));
                        Log.AddLogItem("Flashing recovery.", "RECOVERY");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        Log.AddLogItem("Recovery flash successful.", "RECOVERY");
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            Log.AddLogItem("Rebooting device.", "RECOVERY");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await controller2.CloseAsync();
                        }
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "You can now move on to Step 3, gaining root.", MessageDialogStyle.Affirmative);
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        Log.AddLogItem("Connected: Fastboot.", "RECOVERY");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing TWRP...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + "./Data/Recoveries/" + recovery)));
                        Log.AddLogItem("Flashing recovery.", "RECOVERY");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        Log.AddLogItem("Recovery flash successful.", "RECOVERY");
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            Log.AddLogItem("Rebooting device.", "RECOVERY");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await controller2.CloseAsync();
                        }
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "You can now move on to Step 3, gaining root.", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "RECOVERY");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
            else
            {
                Log.AddLogItem("Missing recovery.", "RECOVERY");
                await this.ShowMessageAsync("Missing Recovery!", "This recovery appears to be missing from the Data folder! Please redownload the recoveries for your device.",
                                            MessageDialogStyle.Affirmative);
            }
        }

        private async void gainRootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                string device = Properties.Settings.Default["Device"].ToString();
                if (device == "Desire 610")
                {
                    Log.AddLogItem("Desire 610 root command started.", "ROOT");
                    MessageDialogResult result = await this.ShowMessageAsync("AT&T Sucks!", "Due to restrictions put in place by AT&T, you must flash a recovery and gain root through a special program. Would you like to download it now?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Desire610Root();
                    }
                }
                else if (device == "Moto X (2014)" || device == "Nexus Player")
                {
                    Log.AddLogItem("Boot root command started.", "ROOT");
                    MessageDialogResult result = await this.ShowMessageAsync("Ready To Root?", "This command will download the rooted boot image, reboot you device into fastboot, then flash the rooted boot image. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        BootRoot();
                    }
                }
                else
                {
                    Log.AddLogItem("SuperSU root command started.", "ROOT");
                    MessageDialogResult result = await this.ShowMessageAsync("Ready To Root?", "This command will download SuperSU, reboot you device into recovery, then allow you to flash SuperSU to permanently gain root. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Root();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void Root()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            string device = Properties.Settings.Default["Device"].ToString();
            var controller9 = await this.ShowProgressAsync("Checking Device...", "");
            await TaskEx.Run(() => UpdateDevice());
            await controller9.CloseAsync();
            if (statusLabel.Content.ToString() == "Online")
            {
                Log.AddLogItem("Connected: Online.", "ROOT");
                if (_device.HasRoot.ToString() == "True")
                {
                    Log.AddLogItem("Device is already rooted.", "ROOT");
                    MessageDialogResult result2 = await this.ShowMessageAsync("Already Rooted!", "It appears that your device is already rooted! Would you still like to continue with flashing SuperSU?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result2 == MessageDialogResult.Affirmative)
                    {
                        if (File.Exists("./Data/Installers/SuperSU.zip"))
                        {
                            SuperSU();
                        }
                        else
                        {
                            if (device == "G Watch" || device == "G Watch R" || device == "Gear Live" || device == "Moto 360" || device == "Smartwatch 3" || device == "Zenwatch")
                            {
                                await DownloadFile("SuperSU", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/WearSuperSU.zip", "./Data/Installers/SuperSU.zip");
                                Log.AddLogItem("Wear SuperSU pushing started.", "ROOT");
                                SuperSU();
                            }
                            else
                            {
                                await DownloadFile("SuperSU", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/SuperSU.zip", "./Data/Installers/SuperSU.zip");
                                Log.AddLogItem("SuperSU pushing started.", "ROOT");
                                SuperSU();
                            }
                        }
                    }
                }
                else
                {
                    Log.AddLogItem("Device is not already rooted.", "ROOT");
                    if (File.Exists("./Data/Installers/SuperSU.zip"))
                    {
                        SuperSU();
                    }
                    else
                    {
                        if (device == "G Watch" || device == "G Watch R" || device == "Gear Live" || device == "Moto 360" || device == "Smartwatch 3" || device == "Zenwatch")
                        {
                            await DownloadFile("SuperSU", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/WearSuperSU.zip", "./Data/Installers/SuperSU.zip");
                            Log.AddLogItem("Wear SuperSU pushing started.", "ROOT");
                            SuperSU();
                        }
                        else
                        {
                            await DownloadFile("SuperSU", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/SuperSU.zip", "./Data/Installers/SuperSU.zip");
                            Log.AddLogItem("SuperSU pushing started.", "ROOT");
                            SuperSU();
                        }
                    }
                }
            }
            else
            {
                Log.AddLogItem("No device found.", "ROOT");
                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                        MessageDialogStyle.Affirmative);
            }
        }

        private async void SuperSU()
        {
            var controller = await this.ShowProgressAsync("Pushing SuperSU...", "");
            if (await TaskEx.Run(() => _device.PushFile("./Data/Installers/SuperSU.zip", "/sdcard/SuperSU.zip").ToString() == "True"))
            {
                Log.AddLogItem("SuperSU push successful.", "ROOT");
                controller.SetTitle("Rebooting Device...");
                await TaskEx.Run(() => _device.RebootRecovery());
                Log.AddLogItem("Rebooting device to recovery.", "ROOT");
                await controller.CloseAsync();
                await this.ShowMessageAsync("Rebooting Device...", "Once in TWRP, tap on 'Install' in the top left corner. Then, scroll until you find 'SuperSU.zip'. Finally, tap on it, then swipe to to confirm flash. Once you have finished, click 'Ok'.",
                MessageDialogStyle.Affirmative);
                await this.ShowMessageAsync("Congratulations!", "You are all done with unlocking and rooting process! Hit the 'Reboot System' buttom in the bottom right corner in TWRP, and your device will boot up with root privileges.",
                MessageDialogStyle.Affirmative);
            }
            else
            {
                Log.AddLogItem("SuperSU push failed.", "ROOT");
                await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push SuperSU.zip. Please restart the toolkit and try again in a few minutes.",
                                MessageDialogStyle.Affirmative);
            }
        }

        private async void Desire610Root()
        {
            var controller9 = await this.ShowProgressAsync("Checking Device...", "");
            await TaskEx.Run(() => UpdateDevice());
            await controller9.CloseAsync();
            if (statusLabel.Content.ToString() == "Online")
            {
                Log.AddLogItem("Connected: Online.", "ROOT");
                if (File.Exists("./Data/Installers/Desire_610.exe"))
                {
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/Desire_610.exe");
                    Log.AddLogItem("Desire 610 root program started.", "ROOT");
                }
                else
                {
                    await DownloadFile("Desire 610 Root Program", "https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Desire_610/Desire_610.exe", "./Data/Installers/Desire_610.exe");
                    Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/Desire_610.exe");
                    Log.AddLogItem("Desire 610 root program started.", "ROOT");
                }
            }
            else
            {
                Log.AddLogItem("No device found.", "ROOT");
                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                            MessageDialogStyle.Affirmative);
            }
        }

        private async void BootRoot()
        {
            string device = Properties.Settings.Default["Device"].ToString();
            if (device == "Moto X (2014)")
            {
                await DownloadFile("Moto X (2014) Root Image", "https://basketbuild.com/dl/devs?dev=WindyCityRockr&dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Moto_X_2014/Root.img", "./Root.img");
            }
            else if (device == "Nexus Player")
            {
                await DownloadFile("Nexus Player Root Image", "https://basketbuild.com/dl/devs?dev=WindyCityRockr&dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones/Nexus_Player/Root.img", "./Root.img");
            }
            var controller9 = await this.ShowProgressAsync("Checking Device...", "");
            await TaskEx.Run(() => UpdateDevice());
            await controller9.CloseAsync();
            if (statusLabel.Content.ToString() == "Online")
            {
                Log.AddLogItem("Connected: Online.", "ROOT");
                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                await TaskEx.Run(() => _android.WaitForDevice());
                controller.SetTitle("Rebooting Device...");
                await TaskEx.Run(() => _device.RebootBootloader());
                Log.AddLogItem("Rebooting device to bootloader.", "ROOT");
                await TaskEx.Run(() => _android.WaitForDevice());
                controller.SetTitle("Flashing Root...");
                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "boot", "./Root.img")));
                Log.AddLogItem("Flashing rooted boot image.", "ROOT");
                await TaskEx.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
            if (statusLabel.Content.ToString() == "Fastboot")
            {
                Log.AddLogItem("Connected: Fastboot.", "ROOT");
                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                await TaskEx.Run(() => _android.WaitForDevice());
                controller.SetTitle("Flashing Root...");
                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "boot", "./Root.img")));
                Log.AddLogItem("Flashing rooted boot image.", "ROOT");
                await TaskEx.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
            else
            {
                Log.AddLogItem("No device found.", "ROOT");
                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                        MessageDialogStyle.Affirmative);
            }
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.AddLogItem("Refresh button clicked.", "DEVICE");
                var controller = await this.ShowProgressAsync("Detecting Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void rebootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "REBOOT");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.Reboot());
                        Log.AddLogItem("Device rebooting.", "REBOOT");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void rebootRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device into recovery mode. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Recovery reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "REBOOT");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootRecovery());
                        Log.AddLogItem("Device rebooting to recovery.", "REBOOT");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                                MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void rebootBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device into bootloader mode. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Bootloader reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "REBOOT");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        Log.AddLogItem("Device rebooting to bootloader.", "REBOOT");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                                MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void flashKernelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash your chosen kernel. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Flash kernel button clicked.", "KERNEL");
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Kernel Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Kernel file selected.", "KERNEL");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "KERNEL");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Rebooting Device...");
                            await TaskEx.Run(() => _device.RebootBootloader());
                            Log.AddLogItem("Rebooting device to bootloader.", "KERNEL");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Kernel...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "boot " + ofd.FileName)));
                            Log.AddLogItem("Flashing kernel.", "KERNEL");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            Log.AddLogItem("Kernel flash successful.", "KERNEL");
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                Log.AddLogItem("Rebooting device.", "KERNEL");
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else if (statusLabel.Content.ToString() == "Fastboot")
                        {
                            Log.AddLogItem("Connected: Fastboot.", "KERNEL");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Kernel...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "boot " + ofd.FileName)));
                            Log.AddLogItem("Flashing kernel.", "KERNEL");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            Log.AddLogItem("Kernel flash successful.", "KERNEL");
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                Log.AddLogItem("Rebooting device.", "KERNEL");
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "KERNEL");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void flashRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string device = Properties.Settings.Default["Device"].ToString();
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Flash recovery button clicked.", "RECOVERY");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash your chosen recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Recovery Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Recovery file selected.", "RECOVERY");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "RECOVERY");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Rebooting Device...");
                            if (device == "One M9")
                            {
                                await TaskEx.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                                Log.AddLogItem("Rebooting device to download mode.", "RECOVERY M9");
                            }
                            else
                            {
                                await TaskEx.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                            }
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Recovery...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + ofd.FileName)));
                            Log.AddLogItem("Flashing recovery.", "RECOVERY");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            Log.AddLogItem("Recovery flash successful.", "RECOVERY");
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                Log.AddLogItem("Rebooting device.", "RECOVERY");
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else if (statusLabel.Content.ToString() == "Fastboot")
                        {
                            Log.AddLogItem("Connected: Fastboot.", "RECOVERY");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Recovery...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + ofd.FileName)));
                            Log.AddLogItem("Flashing recovery.", "RECOVERY");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            Log.AddLogItem("Recovery flash successful.", "RECOVERY");
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                Log.AddLogItem("Rebooting device.", "RECOVERY");
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "RECOVERY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void sideloadROMButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Sideload buttom clicked.", "SIDELOAD");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Sideload?", "This will push a ROM or other zip file of your choosing to your device using ADB Sideload. You must have TWRP flashed on your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "ZIP Files (*.zip*)|*.ZIP*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Sideload file selected.", "SIDELOAD");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Sideload")
                        {
                            var controller = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient, and do not disconnect your device.");
                            await TaskEx.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("sideload", ofd.FileName)));
                            Log.AddLogItem("Sideloading file.", "SIDELOAD");
                            await TaskEx.Run(() => UpdateDevice());
                            await TaskEx.Run(() => _android.Dispose());
                            await controller.CloseAsync();
                            await this.ShowMessageAsync("Sideload Successful!", "The zip file should have been flashed. After it flashes, you can reboot your device, or continue using features in the recovery.", MessageDialogStyle.Affirmative);
                            Log.AddLogItem("Sideload successful.", "SIDELOAD");
                        }
                        else if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "SIDELOAD");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Rebooting Device...");
                            await TaskEx.Run(() => _device.RebootRecovery());
                            Log.AddLogItem("Rebooting device to recovery.", "SIDELOAD");
                            await controller.CloseAsync();
                            await this.ShowMessageAsync("Activate Sideloading", "Once in TWRP, tap on 'Advanced' in the bottom left corner. Then, tap on ADB Sideload in the same spot. Finally, swipe to to start sideload. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative);
                            var controller8 = await this.ShowProgressAsync("Checking Device...", "");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller8.CloseAsync();
                            if (statusLabel.Content.ToString() == "Sideload")
                            {
                                var controller7 = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient, and do not disconnect your device.");
                                await TaskEx.Run(() => Adb.ExecuteAdbCommand(Adb.FormAdbCommand("sideload", ofd.FileName)));
                                Log.AddLogItem("Sideloading file.", "SIDELOAD");
                                await TaskEx.Run(() => UpdateDevice());
                                await TaskEx.Run(() => _android.Dispose());
                                await controller7.CloseAsync();
                                await this.ShowMessageAsync("Sideload Successful!", "The zip file should have been flashed. After it flashes, you can reboot your device, or continue using features in the recovery.", MessageDialogStyle.Affirmative);
                                Log.AddLogItem("Sideload successful.", "SIDELOAD");
                            }
                            else
                            {
                                Log.AddLogItem("No device found.", "SIDELOAD");
                                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                            }
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "SIDELOAD");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                        MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void pushFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Push file button selected.", "PUSH");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Push?", "This will push a file your choosing to your device's storage. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "All Files (*.*)|*.*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("File selected.", "PUSH");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "PUSH");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "Depending on the size of the file being pushed, this process can take awhile. Please be patient, and do not disconnect your device.");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Pushing File...");
                            Log.AddLogItem("Pushing file.", "PUSH");
                            if (await TaskEx.Run(() => _device.PushFile(ofd.FileName, "/sdcard/" + ofd.SafeFileName).ToString() == "True"))
                            {
                                await TaskEx.Run(() => UpdateDevice());
                                await TaskEx.Run(() => _android.Dispose());
                                await controller.CloseAsync();
                                await this.ShowMessageAsync("Push Successful!", ofd.SafeFileName + " was successfully pushed!", MessageDialogStyle.Affirmative);
                                Log.AddLogItem("File push successful.", "PUSH");
                            }
                            else
                            {
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("File Push Failed!", "An issue occurred while attempting to push " + ofd.SafeFileName + " Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative);
                                Log.AddLogItem("File push failed.", "PUSH");
                            }
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "PUSH");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                                MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void installAppButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Install app button selected.", "APP");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Install?", "This will install an app of your choosing to your device. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "APK Files (*.apk*)|*.APK*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("APK file selected.", "APP");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            Log.AddLogItem("Connected: Online.", "APP");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "If the install process takes more than a minute, please check your device as you may have to accept a notice from Google.");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Installing App...");
                            Log.AddLogItem("Installing app.", "APP");
                            if (await TaskEx.Run(() => _device.InstallApk(ofd.FileName).ToString()) == "True")
                            {
                                await TaskEx.Run(() => UpdateDevice());
                                await controller.CloseAsync();
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("Install Successful!", ofd.SafeFileName + " was successfully installed!", MessageDialogStyle.Affirmative);
                                Log.AddLogItem("App install successful.", "APP");
                            }
                            else
                            {
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("App Install Failed!", "An issue occurred while attempting to install " + ofd.SafeFileName, MessageDialogStyle.Affirmative);
                                Log.AddLogItem("App install failed.", "APP");
                            }
                        }
                        else
                        {
                            Log.AddLogItem("No device found.", "APP");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                                 MessageDialogStyle.Affirmative);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void relockBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                Log.AddLogItem("Relock bootloader button clicked.", "RELOCK");
                MessageDialogResult result = await this.ShowMessageAsync("Ready To Relock?", "This will relock your device's bootloader. You will lose root and custom recovery. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        Log.AddLogItem("Connected: Online.", "RELOCK");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        Log.AddLogItem("Rebooting device to bootloader.", "RELOCK");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Relocking Bootloader...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                        Log.AddLogItem("Relocking bootloader.", "RELOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.",
                          MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader relock successful.", "RELOCK");
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        Log.AddLogItem("Connected: Fastboot.", "RELOCK");
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Relocking Bootloader...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                        Log.AddLogItem("Relocking bootloader.", "RELOCK");
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.",
                          MessageDialogStyle.Affirmative);
                        Log.AddLogItem("Bootloader relock successful.", "RELOCK");
                    }
                    else
                    {
                        Log.AddLogItem("No device found.", "RELOCK");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/showpost.php?p=52041197&postcount=2");
                Log.AddLogItem("Help button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void requestsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://docs.google.com/forms/d/1fBsKXhHilnwtqDQdJuJ9dDpxSB5cZMu5zAEWsM_ogGE/viewform?usp=send_form");
                Log.AddLogItem("Requests button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void devicesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://docs.google.com/spreadsheets/d/1i_0zalrctxe4nQ7JmigLv4JgJjy9w8QZVXka1iMYEa4/edit?usp=sharing");
                Log.AddLogItem("Devices button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void toolkitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/devdb/project/?id=1314#downloads");
                Log.AddLogItem("Toolkit button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void recoveriesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://s.basketbuild.com/devs/WindyCityRockr/WinDroid_Universal_Android_Toolkit/Phones");
                Log.AddLogItem("Recoveries button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void ruuButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://androidruu.com/");
                Log.AddLogItem("RUU button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void xdaButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/private.php?do=newpm&u=4485224");
                Log.AddLogItem("PM button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/sendmessage.php?do=mailmember&u=4485224");
                Log.AddLogItem("Email button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void twitterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://twitter.com/WindyCityRockr");
                Log.AddLogItem("Twitter button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }
    }
}