using AutoUpdaterDotNET;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using RegawMOD.Android;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using WinDroid_Toolkit.Models;
using WinDroid_Toolkit.Properties;

namespace WinDroid_Toolkit
{
    public partial class MainWindow
    {
        private AndroidController _android;
        private Device _device;

        public class Thing
        {
            public string Name { get; set; }
            public string Manufacturer { get; set; }
        }

        private readonly ObservableCollection<Thing> _deviceList;

        public MainWindow()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show(
                    "You can only have one instance of the toolkit running at a time!",
                    "Why would you ever need more than one open I mean seriously wtf.", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            InitializeComponent();
            logBox.DataContext = Log;

            _deviceList = new ObservableCollection<Thing>
            {
                new Thing{ Name="OneTouch Idol 3", Manufacturer="Alcatel"},
                new Thing{ Name="OneTouch Idol X", Manufacturer="Alcatel"},
                new Thing{ Name="OneTouch Pixi 3", Manufacturer="Alcatel"},
                new Thing{ Name="ZenFone 2", Manufacturer="Asus"},
                new Thing{ Name="ZenFone 5", Manufacturer="Asus"},
                new Thing{ Name="ZenFone 6", Manufacturer="Asus"},
                new Thing{ Name="ZenFone Go", Manufacturer="Asus"},
                new Thing{ Name="ZenFone Laser", Manufacturer="Asus"},
                new Thing{ Name="ZenFone Selfie", Manufacturer="Asus"},
                new Thing{ Name="ZenWatch", Manufacturer="Asus"},
                new Thing{ Name="B15q", Manufacturer="CAT"},
                new Thing{ Name="Pure XL", Manufacturer="BLU"},
                new Thing{ Name="Studio 5", Manufacturer="BLU"},
                new Thing{ Name="Studio G", Manufacturer="BLU"},
                new Thing{ Name="Vivo Air", Manufacturer="BLU"},
                new Thing{ Name="Aquaris A4.5", Manufacturer="BQ"},
                new Thing{ Name="Aquaris E4", Manufacturer="BQ"},
                new Thing{ Name="Aquaris E5", Manufacturer="BQ"},
                new Thing{ Name="Aquaris E6", Manufacturer="BQ"},
                new Thing{ Name="Aquaris E10", Manufacturer="BQ"},
                new Thing{ Name="Aquaris M4.5", Manufacturer="BQ"},
                new Thing{ Name="Aquaris M5", Manufacturer="BQ"},
                new Thing{ Name="Aquaris M5.5", Manufacturer="BQ"},
                new Thing{ Name="Aquaris X5", Manufacturer="BQ"},
                new Thing{ Name="Curie 2 QC", Manufacturer="BQ"},
                new Thing{ Name="Edison 2 QC", Manufacturer="BQ"},
                new Thing{ Name="Edison 3 Mini", Manufacturer="BQ"},
                new Thing{ Name="Maxwell 2", Manufacturer="BQ"},
                new Thing{ Name="Maxwell 2 Lite", Manufacturer="BQ"},
                new Thing{ Name="Maxwell 2 Plus", Manufacturer="BQ"},
                new Thing{ Name="Maxwell 2 QC", Manufacturer="BQ"},
                new Thing{ Name="P8000", Manufacturer="Elephone"},
                new Thing{ Name="Android One", Manufacturer="Google"},
                new Thing{ Name="Galaxy Nexus", Manufacturer="Google"},
                new Thing{ Name="Nexus 4", Manufacturer="Google"},
                new Thing{ Name="Nexus 5", Manufacturer="Google"},
                new Thing{ Name="Nexus 5X", Manufacturer="Google"},
                new Thing{ Name="Nexus 6", Manufacturer="Google"},
                new Thing{ Name="Nexus 6P", Manufacturer="Google"},
                new Thing{ Name="Nexus 7 (2012)", Manufacturer="Google"},
                new Thing{ Name="Nexus 7 (2013)", Manufacturer="Google"},
                new Thing{ Name="Nexus 9", Manufacturer="Google"},
                new Thing{ Name="Nexus 10", Manufacturer="Google"},
                new Thing{ Name="Nexus S", Manufacturer="Google"},
                new Thing{ Name="Nexus Player", Manufacturer="Google"},
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
                new Thing{ Name="Desire 626G+", Manufacturer="HTC"},
                new Thing{ Name="Desire 626s", Manufacturer="HTC"},
                new Thing{ Name="Desire 700", Manufacturer="HTC"},
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
                new Thing{ Name="Explorer", Manufacturer="HTC"},
                new Thing{ Name="First", Manufacturer="HTC"},
                new Thing{ Name="myTouch 4G Slide", Manufacturer="HTC"},
                new Thing{ Name="One A9", Manufacturer="HTC"},
                new Thing{ Name="One E8", Manufacturer="HTC"},
                new Thing{ Name="One E9+", Manufacturer="HTC"},
                new Thing{ Name="One J", Manufacturer="HTC"},
                new Thing{ Name="One M7", Manufacturer="HTC"},
                new Thing{ Name="One M7 (Dual SIM)", Manufacturer="HTC"},
                new Thing{ Name="One M8", Manufacturer="HTC"},
                new Thing{ Name="One M8 Eye", Manufacturer="HTC"},
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
                new Thing{ Name="Rhyme", Manufacturer="HTC"},
                new Thing{ Name="Sensation", Manufacturer="HTC"},
                new Thing{ Name="Sensation XL", Manufacturer="HTC"},
                new Thing{ Name="Vivid", Manufacturer="HTC"},
                new Thing{ Name="Wildfire", Manufacturer="HTC"},
                new Thing{ Name="Wildfire S", Manufacturer="HTC"},
                new Thing{ Name="Honor 5X", Manufacturer="Huawei"},
                new Thing{ Name="Mate 8", Manufacturer="Huawei"},
                new Thing{ Name="P8", Manufacturer="Huawei"},
                new Thing{ Name="Watch", Manufacturer="Huawei"},
                new Thing{ Name="Y635", Manufacturer="Huawei"},
                new Thing{ Name="Thunder Q4.5", Manufacturer="Kazam"},
                new Thing{ Name="Tornado 348", Manufacturer="Kazam"},
                new Thing{ Name="Iris X8", Manufacturer="Lava"},
                new Thing{ Name="Zuk Z1", Manufacturer="Lenovo"},
                new Thing{ Name="G4", Manufacturer="LG"},
                new Thing{ Name="G Watch", Manufacturer="LG"},
                new Thing{ Name="G Watch R", Manufacturer="LG"},
                new Thing{ Name="Watch Urbane", Manufacturer="LG"},
                new Thing{ Name="Moto 360", Manufacturer="Motorola"},
                new Thing{ Name="Moto E", Manufacturer="Motorola"},
                new Thing{ Name="Moto E (2015)", Manufacturer="Motorola"},
                new Thing{ Name="Moto G", Manufacturer="Motorola"},
                new Thing{ Name="Moto G (2014)", Manufacturer="Motorola"},
                new Thing{ Name="Moto G (2015)", Manufacturer="Motorola"},
                new Thing{ Name="Moto G Force", Manufacturer="Motorola"},
                new Thing{ Name="Moto Maxx", Manufacturer="Motorola"},
                new Thing{ Name="Moto X", Manufacturer="Motorola"},
                new Thing{ Name="Moto X (2014)", Manufacturer="Motorola"},
                new Thing{ Name="Moto X Force", Manufacturer="Motorola"},
                new Thing{ Name="Moto X Play", Manufacturer="Motorola"},
                new Thing{ Name="Moto X Style (Pure)", Manufacturer="Motorola"},
                new Thing{ Name="Photon Q", Manufacturer="Motorola"},
                new Thing{ Name="Xoom", Manufacturer="Motorola"},
                new Thing{ Name="Shield", Manufacturer="Nvidia"},
                new Thing{ Name="Shield Tablet", Manufacturer="Nvidia"},
                new Thing{ Name="Shield TV", Manufacturer="Nvidia"},
                new Thing{ Name="Tegra Note 7", Manufacturer="Nvidia"},
                new Thing{ Name="OnePlus One", Manufacturer="OnePlus"},
                new Thing{ Name="OnePlus 2", Manufacturer="OnePlus"},
                new Thing{ Name="OnePlus X", Manufacturer="OnePlus"},
                new Thing{ Name="Find 5", Manufacturer="Oppo"},
                new Thing{ Name="Find 7", Manufacturer="Oppo"},
                new Thing{ Name="N1", Manufacturer="Oppo"},
                new Thing{ Name="N3", Manufacturer="Oppo"},
                new Thing{ Name="R7", Manufacturer="Oppo"},
                new Thing{ Name="R819", Manufacturer="Oppo"},
                new Thing{ Name="Gear Live", Manufacturer="Samsung"},
                new Thing{ Name="SmartWatch 3", Manufacturer="Sony"},
                new Thing{ Name="Xperia M4 Aqua", Manufacturer="Sony"},
                new Thing{ Name="Xperia Z5", Manufacturer="Sony"},
                new Thing{ Name="Xperia Z5 Compact", Manufacturer="Sony"},
                new Thing{ Name="Xperia Z5 Premium", Manufacturer="Sony"},
                new Thing{ Name="Storm", Manufacturer="Wileyfox"},
                new Thing{ Name="Swift", Manufacturer="Wileyfox"},
                new Thing{ Name="Mi 3", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi 4c", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi Note Pro", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi Pad", Manufacturer="Xiaomi"},
                new Thing{ Name="Mi Pad 2", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi 1S", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi 2", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi Note", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi Note 2", Manufacturer="Xiaomi"},
                new Thing{ Name="Redmi Note 3", Manufacturer="Xiaomi"},
                new Thing{ Name="Q1010i", Manufacturer="XOLO"},
                new Thing{ Name="Q3000", Manufacturer="XOLO"},
                new Thing{ Name="Yunique", Manufacturer="YU"},
                new Thing{ Name="Yuphoria", Manufacturer="YU"},
                new Thing{ Name="Yureka", Manufacturer="YU"},
                new Thing{ Name="None", Manufacturer="Other"},
                new Thing{ Name="iPhone 6S", Manufacturer="Other"}
            };
            var view = CollectionViewSource.GetDefaultView(_deviceList);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Manufacturer"));
            view.SortDescriptions.Add(new SortDescription("Manufacturer", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            deviceListBox.ItemsSource = view;
        }

        public LogModel Log { get; set; } = new LogModel();

        private static string GetStringBetween(string source, string start, string end)
        {
            var startIndex = source.IndexOf(start, StringComparison.InvariantCulture) + start.Length;
            var endIndex = source.IndexOf(end, startIndex, StringComparison.InvariantCulture);
            var length = endIndex - startIndex;
            return source.Substring(startIndex, length);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void CheckFileSystem()
        {
            try
            {
                string[] neededDirectories = { "Data/", "Data/Downloads", "Data/Logs", "Data/Recoveries" };

                foreach (var dir in neededDirectories)
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void LogBaseSystemInfo()
        {
            Log.AddLogItem(Environment.OSVersion + " " + (Environment.Is64BitOperatingSystem ? "64Bit" : "32bit"),
                "INFO");
            Log.AddLogItem(
                Assembly.GetExecutingAssembly().GetName().Name + " " + Assembly.GetExecutingAssembly().GetName().Version,
                "INFO");
            foreach (var referencedAssembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Log.AddLogItem(referencedAssembly.Name + " " + referencedAssembly.Version, "INFO");
            }
        }

        private void ToggleDeviceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
        }

        private void ToggleLogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((Flyout)Flyouts.Items[1]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
        }

        private async void SaveLogCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dictionary = new ResourceDictionary();
            dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");
            var mySettings = new MetroDialogSettings
            {
                SuppressDefaultResources = true,
                CustomResourceDictionary = dictionary
            };

            Log.AddLogItem("Log saved.", "LOG");
            var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
            var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
            file.WriteLine(logBox.Text);
            file.Close();
            await this.ShowMessageAsync("Log Saved!", "A copy of this log has been saved in the Logs folder.",
                        MessageDialogStyle.Affirmative, mySettings);
        }

        private async Task DownloadRecoveries(string manufacturer, string device, int variants)
        {
            Log.AddLogItem("Recovery download queued.", "DOWNLOAD");
            var controller = await this.ShowProgressAsync("Downloading...", "The latest recoveries are being queued for download. Depending on how many versions of your device there are and your internet speed, this may take a few minutes.");
            controller.SetIndeterminate();
            controller.SetCancelable(false);

            var client = new WebClient();
            var client2 = new WebClient();
            var client3 = new WebClient();

            client3.DownloadProgressChanged += (s, e) =>
            {
                controller.SetMessage(e.ProgressPercentage + "% Completed.");
                double progress = 0;
                if (!(e.ProgressPercentage > progress)) return;
                controller.SetProgress(e.ProgressPercentage / 100.0d);
            };

            client.DownloadFileCompleted += async (s, e) =>
            {
                if (e.Error != null)
                {
                    await controller.CloseAsync();
                    Log.AddLogItem("Recovery download error.", "DOWNLOAD");
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
                    Log.AddLogItem("First recovery download completed.", "DOWNLOAD");
                }
            };

            client2.DownloadFileCompleted += async (s, e) =>
            {
                if (e.Error != null)
                {
                    await controller.CloseAsync();
                    Log.AddLogItem("Recovery download error.", "DOWNLOAD");
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
                    Log.AddLogItem("Second recovery download completed.", "DOWNLOAD");
                }
            };

            client3.DownloadFileCompleted += async (s, e) =>
            {
                await controller.CloseAsync();
                if (e.Error != null)
                {
                    Log.AddLogItem("Recovery download error.", "DOWNLOAD");
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
                    Log.AddLogItem("Third ecovery download completed.", "DOWNLOAD");
                }
            };

            Log.AddLogItem("Recovery download started.", "DOWNLOAD");
            if (variants == 1)
            {
                await client3.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery.img"), "./Data/Recoveries/Recovery1.img");
                client3.Dispose();
            }
            else if (variants == 2)
            {
                await client2.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery1.img"), "./Data/Recoveries/Recovery1.img");
                await client3.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery2.img"), "./Data/Recoveries/Recovery2.img");
                client2.Dispose();
                client3.Dispose();
            }
            else if (variants == 3)
            {
                await client.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery1.img"), "./Data/Recoveries/Recovery1.img");
                await client2.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery2.img"), "./Data/Recoveries/Recovery2.img");
                await client3.DownloadFileTaskAsync(("https://s3.amazonaws.com/windroid/Devices/" + manufacturer + "/" + device + "/Recovery3.img"), "./Data/Recoveries/Recovery3.img");
                client.Dispose();
                client2.Dispose();
                client3.Dispose();
            }
        }

        private async Task DownloadFile(string name, string url, string path)
        {
            Log.AddLogItem("Download of " + name + " started.", "DOWNLOAD");
            var controller = await this.ShowProgressAsync("Downloading File...", "");
            controller.SetCancelable(false);

            var client = new WebClient();
            client.DownloadProgressChanged += (s, e) =>
            {
                controller.SetMessage(e.ProgressPercentage + "% Completed.");
                double progress = 0;
                if (!(e.ProgressPercentage > progress)) return;
                controller.SetProgress(e.ProgressPercentage / 100.0d);
            };

            client.DownloadFileCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Detecting Device...", "DEVICE");
                await Task.Run(() => _android.UpdateDeviceList());
                if (await Task.Run(() => _android.HasConnectedDevices))
                {
                    _device = await Task.Run(() => _android.GetConnectedDevice(_android.ConnectedDevices[0]));
                    switch (_device.State.ToString())
                    {
                        case "ONLINE":
                            await Dispatcher.BeginInvoke((Action)delegate
                            {
                                statusLabel.Content = "Online";
                                statusEllipse.Fill = Brushes.Green;
                                Log.AddLogItem("Connected: Online.", "DEVICE");
                            });
                            break;

                        case "FASTBOOT":
                            await Dispatcher.BeginInvoke((Action)delegate
                            {
                                statusLabel.Content = "Fastboot";
                                statusEllipse.Fill = Brushes.Blue;
                                Log.AddLogItem("Connected: Fastboot.", "DEVICE");
                            });
                            break;

                        case "RECOVERY":
                            await Dispatcher.BeginInvoke((Action)delegate
                            {
                                statusLabel.Content = "Recovery";
                                statusEllipse.Fill = Brushes.Purple;
                                Log.AddLogItem("Connected: Recovery.", "DEVICE");
                            });
                            break;

                        case "SIDELOAD":
                            await Dispatcher.BeginInvoke((Action)delegate
                            {
                                statusLabel.Content = "Sideload";
                                statusEllipse.Fill = Brushes.Orange;
                                Log.AddLogItem("Connected: Sideload.", "DEVICE");
                            });
                            break;

                        case "UNAUTHORIZED":
                            await Dispatcher.BeginInvoke((Action)delegate
                            {
                                statusLabel.Content = "Unauthorized";
                                statusEllipse.Fill = Brushes.Orange;
                                Log.AddLogItem("Connected: Unauthorized.", "DEVICE");
                                this.ShowMessageAsync("Error!", "Please unlock your device and allow USB Debugging with your computer.", MessageDialogStyle.Affirmative, mySettings);
                            });
                            break;

                        case "UNKNOWN":
                            await Dispatcher.BeginInvoke((Action)delegate
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
                    await Dispatcher.BeginInvoke((Action)delegate
                    {
                        statusLabel.Content = "Offline";
                        statusEllipse.Fill = Brushes.Red;
                        Log.AddLogItem("No Device Found.", "DEVICE");
                    });
                }
                await Task.Run(() => _android.Dispose());
            }
            catch (Exception)
            {
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LogBaseSystemInfo();

            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.Start("https://s3.amazonaws.com/windroid/Update/Update.xml");
            Log.AddLogItem("AutoUpdater started.", "UPDATE");

            if (!Directory.Exists("./Data"))
            {
                CheckFileSystem();
            }
            else if (Directory.Exists("./Data/Installers"))
            {
                Directory.Delete("./Data/Installers");
                Directory.CreateDirectory("./Data/Downloads");
            }

            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                if (!Directory.Exists("C:/Program Files (x86)/ClockworkMod/Universal Adb Driver") &&
                    !Directory.Exists("C:/Program Files/ClockworkMod/Universal Adb Driver"))
                {
                    if (Settings.Default["ADB"].ToString() == "Yes")
                    {
                        var result = await this.ShowMessageAsync("ADB Drivers!", "You are missing some ADB drivers! They are required for your device to work with the toolkit and your PC. Would you like to install them now?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        switch (result)
                        {
                            case MessageDialogResult.Affirmative:
                                await DownloadFile("Drivers", "https://s3.amazonaws.com/windroid/Drivers/UniversalDriver.msi", "./Data/Downloads/ADBDriver.msi");
                                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/Downloads/ADBDriver.msi");
                                break;

                            case MessageDialogResult.Negative:
                                var result2 = await this.ShowMessageAsync("ADB Reminder", "Would you like to be reminded the next time you open the toolkit?",
                                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                                switch (result2)
                                {
                                    case MessageDialogResult.Affirmative:
                                        Settings.Default["ADB"] = "Yes";
                                        Settings.Default.Save();
                                        break;

                                    case MessageDialogResult.Negative:
                                        Settings.Default["ADB"] = "No";
                                        Settings.Default.Save();
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }

            var manufacturer = Settings.Default["Manufacturer"].ToString();
            var device = Settings.Default["Device"].ToString();
            if (device == "None")
            {
                ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
                DeviceTextBox.Text = "Please choose your device!";
            }
            else
            {
                if (manufacturer == "Other" || manufacturer == "OnePlus" || device == "Android One")
                {
                    DeviceTextBox.Text = "Current Device: " + Settings.Default["Device"];
                }
                else
                {
                    DeviceTextBox.Text = "Current Device: " + Settings.Default["Manufacturer"] + " " + Settings.Default["Device"];
                }

                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                gainRootButton.IsEnabled = true;
                if (manufacturer == "Alcatel")
                {
                    if (device == "OneTouch Idol X")
                    {
                        recovery1Button.Content = "Flash CWM";
                    }
                    else if (device == "OneTouch Idol 3")
                    {
                        recovery1Button.Content = "TWRP (6045)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (6039)";
                    }
                    else if (device == "OneTouch Pixi 3")
                    {
                        recovery1Button.Content = "CWM (3.5)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "CWM (4)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "CWM (4.5)";
                    }
                }
                else if (manufacturer == "Asus")
                {
                    if (device == "ZenFone Laser")
                    {
                        recovery1Button.Content = "TWRP (ZE550KL)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (ZE551KL)";
                        gainRootButton.Content = "Flash Root Files";
                    }
                }
                else if (manufacturer == "BLU" || manufacturer == "BQ" || manufacturer == "CAT" || manufacturer == "Elephone" || manufacturer == "Kazam" || manufacturer == "Lava" || manufacturer == "XOLO")
                {
                    unlockBootloaderButton.IsEnabled = false;
                    if (device == "Aquaris 4.5")
                    {
                        recovery1Button.Content = "TWRP (4G)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (HD)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "TWRP (FHD)";
                    }
                }
                else if (manufacturer == "Google")
                {
                    if (device == "Android One")
                    {
                        recovery1Button.Content = "TWRP (1st Gen)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (2nd Gen)";
                    }
                    if (device == "Galaxy Nexus")
                    {
                        recovery1Button.Content = "TWRP (GSM)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (Verizon)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "TWRP (Sprint)";
                    }
                    else if (device == "Nexus 7 (2012)")
                    {
                        recovery1Button.Content = "TWRP (WiFi)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (3G)";
                    }
                    else if (device == "Nexus 7 (2013)")
                    {
                        recovery1Button.Content = "TWRP (WiFi)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (LTE)";
                    }
                    else if (device == "Nexus Player")
                    {
                        gainRootButton.Content = "Flash Root Files";
                    }
                    else if (device == "Nexus S")
                    {
                        recovery1Button.Content = "TWRP (3G)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (4G)";
                    }
                }
                else if (manufacturer == "HTC")
                {
                    secondStepButton.IsEnabled = true;
                    secondStepButton.Content = "Get Token ID";
                    if (device == "Desire 210" || device == "Desire 616" || device == "Desire 700" || device == "Desire SV" || device == "Rhyme")
                    {
                        recovery1Button.Content = "Flash CWM";
                    }
                    else if (device == "Droid DNA" || device == "Droid Incredible 4G LTE" || device == "One Remix")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Second Step";
                    }
                    else if (device == "Butterfly")
                    {
                        recovery1Button.Content = "TWRP (x920d)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (x920e)";
                    }
                    else if (device == "Desire 510")
                    {
                        recovery1Button.Content = "TWRP (32Bit)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (64Bit)";
                    }
                    else if (device == "Desire 601")
                    {
                        recovery1Button.Content = "TWRP (Zara)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (ZaraCL)";
                    }
                    else if (device == "EVO 3D")
                    {
                        recovery1Button.Content = "TWRP (GSM)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (CDMA)";
                    }
                    else if (device == "One M7")
                    {
                        firstStepButton.IsEnabled = true;
                        firstStepButton.Content = "Verizon M7 Only";
                        recovery1Button.Content = "TWRP (GSM)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (Sprint)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "TWRP (Verizon)";
                    }
                    else if (device == "One M7 (Dual SIM)")
                    {
                        recovery1Button.Content = "TWRP (802d)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (802t)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "TWRP (802w)";
                    }
                    else if (device == "One M8")
                    {
                        firstStepButton.Content = "Verizon M8 Only";
                    }
                    else if (device == "One Max")
                    {
                        recovery1Button.Content = "TWRP (Sprint)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (Verizon)";
                    }
                    else if (device == "One S")
                    {
                        recovery1Button.Content = "TWRP (S4)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (S3)";
                    }
                    else if (device == "One SV")
                    {
                        recovery1Button.Content = "TWRP (GSM)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (LTE)";
                        recovery3Button.IsEnabled = true;
                        recovery3Button.Content = "TWRP (Boost)";
                    }
                    else if (device == "One V")
                    {
                        recovery1Button.Content = "TWRP (GSM)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (CDMA)";
                    }
                    else if (device == "One X")
                    {
                        firstStepButton.IsEnabled = true;
                        firstStepButton.Content = "AT&T One X Only";
                        recovery1Button.Content = "TWRP (Global)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (AT&T)";
                    }
                    else if (device == "One X+")
                    {
                        recovery1Button.Content = "TWRP (Global)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (AT&T)";
                    }
                }
                else if (manufacturer == "Huawei")
                {
                    if (device != "Watch")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Get Unlock Code";
                    }
                }
                else if (manufacturer == "LG")
                {
                    if (device == "G4")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Get Device ID";
                    }
                }
                else if (manufacturer == "Motorola")
                {
                    if (device != "Moto 360")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Get Unlock Key";
                        if (device == "Moto G (2014)" || device == "Moto E (2015)")
                        {
                            recovery1Button.Content = "TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (4G)";
                        }
                    }
                }
                else if (manufacturer == "Oppo")
                {
                    if (device == "R7")
                    {
                        recovery1Button.Content = "TWRP (R7)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (R7 Plus)";
                    }
                }
                else if (manufacturer == "Sony")
                {
                    if (device != "SmartWatch 3")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Get Unlock Code";
                        gainRootButton.Content = "Flash Root Files";
                        if (device == "Xperia M4 Aqua")
                        {
                            gainRootButton.Content = "Flash SuperSU";
                        }
                    }
                }
                else if (manufacturer == "Xiaomi")
                {
                    unlockBootloaderButton.IsEnabled = false;
                    if (device == "Mi Pad 2")
                    {
                        unlockBootloaderButton.IsEnabled = true;
                    }
                    else if (device == "Redmi Note")
                    {
                        recovery1Button.Content = "TWRP (3G)";
                        recovery2Button.IsEnabled = true;
                        recovery2Button.Content = "TWRP (4G)";
                    }
                    else if (device == "Mi 4c" || device == "Mi Note Pro" || device == "Redmi Note 3")
                    {
                        secondStepButton.IsEnabled = true;
                        secondStepButton.Content = "Request Unlock";
                        unlockBootloaderButton.IsEnabled = true;

                    }
                }
                Log.AddLogItem("Device: " + manufacturer + " " + device, "DEVICE");
            }
        }

        private async void deviceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                firstStepButton.IsEnabled = false;
                firstStepButton.Content = "First Step";
                secondStepButton.IsEnabled = false;
                secondStepButton.Content = "Second Step";
                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                recovery2Button.IsEnabled = false;
                recovery2Button.Content = "Option Two";
                recovery3Button.IsEnabled = false;
                recovery3Button.Content = "Option Three";
                gainRootButton.IsEnabled = true;
                gainRootButton.Content = "Flash SuperSU";
                var lbi = (((ListBox)sender).SelectedItem as Thing);
                var device = lbi.Name;
                var manufacturer = lbi.Manufacturer;
                switch (device)
                {
                    //Alcatel Devices
                    case "OneTouch Idol 3":
                        {
                            await DownloadRecoveries("Alcatel", "OneTouch_Idol_3", 2);
                            recovery1Button.Content = "TWRP (6045)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (6039)";
                        }
                        break;

                    case "OneTouch Idol X":
                        {
                            await DownloadRecoveries("Alcatel", "OneTouch_Idol_X", 1);
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "OneTouch Pixi 3":
                        {
                            await DownloadRecoveries("Alcatel", "OneTouch_Pixi_3", 3);
                            recovery1Button.Content = "CWM (3.5)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "CWM (4)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "CWM (4.5)";
                        }
                        break;

                    //Asus Devices
                    case "ZenFone 2":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_2", 1);
                        }
                        break;

                    case "ZenFone 5":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_5", 1);
                        }
                        break;

                    case "ZenFone 6":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_6", 1);
                        }
                        break;

                    case "ZenFone Go":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_Go", 1);
                        }
                        break;

                    case "ZenFone Laser":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_Laser", 2);
                            recovery1Button.Content = "TWRP (ZE550KL)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (ZE551KL)";
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "ZenFone Selfie":
                        {
                            await DownloadRecoveries("Asus", "ZenFone_Selfie", 1);
                        }
                        break;

                    case "ZenWatch":
                        {
                            await DownloadRecoveries("Asus", "ZenWatch", 1);
                        }
                        break;

                    //BLU Devices
                    case "Pure XL":
                        {
                            await DownloadRecoveries("BLU", "Pure_XL", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Studio 5":
                        {
                            await DownloadRecoveries("BLU", "Studio_5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Studio G":
                        {
                            await DownloadRecoveries("BLU", "Studio_G", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Vivo Air":
                        {
                            await DownloadRecoveries("BLU", "Vivo_Air", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //BQ Devices
                    case "Aquaris A4.5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_A4.5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris E4":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_E4", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris E5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_E5", 3);
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.Content = "TWRP (4G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (HD)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "TWRP (FHD)";
                        }
                        break;

                    case "Aquaris E6":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_E6", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris E10":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_E10", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris M4.5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_M4.5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris M5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_M5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris M5.5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_M5.5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Aquaris X5":
                        {
                            await DownloadRecoveries("BQ", "Aquaris_X5", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Curie 2 QC":
                        {
                            await DownloadRecoveries("BQ", "Curie_2_QC", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Edison 2 QC":
                        {
                            await DownloadRecoveries("BQ", "Edison_2_QC", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Edison 3 Mini":
                        {
                            await DownloadRecoveries("BQ", "Edison_3_Mini", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Maxwell 2":
                        {
                            await DownloadRecoveries("BQ", "Maxwell_2", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Maxwell 2 Lite":
                        {
                            await DownloadRecoveries("BQ", "Maxwell_2_Lite", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Maxwell 2 Plus":
                        {
                            await DownloadRecoveries("BQ", "Maxwell_2_Plus", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Maxwell 2 QC":
                        {
                            await DownloadRecoveries("BQ", "Maxwell_2_QC", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //CAT Devices
                    case "B15q":
                        {
                            await DownloadRecoveries("CAT", "B15q", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //Elephone Devices
                    case "P8000":
                        {
                            await DownloadRecoveries("Elephone", "P8000", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //Google Devices
                    case "Android One":
                        {
                            await DownloadRecoveries("Google", "Android_One", 2);
                            recovery1Button.Content = "TWRP (1st Gen)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (2nd Gen)";
                        }
                        break;

                    case "Galaxy Nexus":
                        {
                            await DownloadRecoveries("Google", "Galaxy_Nexus", 3);
                            recovery1Button.Content = "TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (Verizon)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "TWRP (Sprint)";
                        }
                        break;

                    case "Nexus 4":
                        {
                            await DownloadRecoveries("Google", "Nexus_4", 1);
                        }
                        break;

                    case "Nexus 5":
                        {
                            await DownloadRecoveries("Google", "Nexus_5", 1);
                        }
                        break;

                    case "Nexus 5X":
                        {
                            await DownloadRecoveries("Google", "Nexus_5X", 1);
                        }
                        break;

                    case "Nexus 6":
                        {
                            await DownloadRecoveries("Google", "Nexus_6", 1);
                        }
                        break;

                    case "Nexus 6P":
                        {
                            await DownloadRecoveries("Google", "Nexus_6P", 1);
                        }
                        break;

                    case "Nexus 7 (2012)":
                        {
                            await DownloadRecoveries("Google", "Nexus_7_2012", 2);
                            recovery1Button.Content = "TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (3G)";
                        }
                        break;

                    case "Nexus 7 (2013)":
                        {
                            await DownloadRecoveries("Google", "Nexus_7_2013", 2);
                            recovery1Button.Content = "TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (LTE)";
                        }
                        break;

                    case "Nexus 9":
                        {
                            await DownloadRecoveries("Google", "Nexus_9", 1);
                        }
                        break;

                    case "Nexus 10":
                        {
                            await DownloadRecoveries("Google", "Nexus_10", 1);
                        }
                        break;

                    case "Nexus Player":
                        {
                            await DownloadRecoveries("Google", "Nexus_Player", 1);
                            gainRootButton.Content = "Flash Root Image";
                        }
                        break;

                    case "Nexus S":
                        {
                            await DownloadRecoveries("Google", "Nexus_S", 2);
                            recovery1Button.Content = "TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (4G)";
                        }
                        break;

                    //HTC Devices
                    case "Amaze":
                        {
                            await DownloadRecoveries("HTC", "Amaze", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Butterfly":
                        {
                            await DownloadRecoveries("HTC", "Butterfly", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (x920d)";
                            recovery1Button.FontSize = 12;
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (x920e)";
                            recovery2Button.FontSize = 12;
                        }
                        break;

                    case "Butterfly 2":
                        {
                            await DownloadRecoveries("HTC", "Butterfly_2", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Butterfly S":
                        {
                            await DownloadRecoveries("HTC", "Butterfly_S", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 200":
                        {
                            await DownloadRecoveries("HTC", "Desire_200", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 210":
                        {
                            await DownloadRecoveries("HTC", "Desire_210", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire 300":
                        {
                            await DownloadRecoveries("HTC", "Desire_300", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 500":
                        {
                            await DownloadRecoveries("HTC", "Desire_500", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 510":
                        {
                            await DownloadRecoveries("HTC", "Desire_510", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (32Bit)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (64Bit)";
                        }
                        break;

                    case "Desire 601":
                        {
                            await DownloadRecoveries("HTC", "Desire_601", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (Zara)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (ZaraCL)";
                        }
                        break;

                    case "Desire 610":
                        {
                            await DownloadRecoveries("HTC", "Desire_610", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 612":
                        {
                            await DownloadRecoveries("HTC", "Desire_612", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 616":
                        {
                            await DownloadRecoveries("HTC", "Desire_616", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire 626":
                        {
                            await DownloadRecoveries("HTC", "Desire_626", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 626G+":
                        {
                            await DownloadRecoveries("HTC", "Desire_626G", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 626s":
                        {
                            await DownloadRecoveries("HTC", "Desire_626s", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 700":
                        {
                            await DownloadRecoveries("HTC", "Desire_700", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire 816":
                        {
                            await DownloadRecoveries("HTC", "Desire_816", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 820":
                        {
                            await DownloadRecoveries("HTC", "Desire_820", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire 826":
                        {
                            await DownloadRecoveries("HTC", "Desire_826", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire C":
                        {
                            await DownloadRecoveries("HTC", "Desire_C", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire Eye":
                        {
                            await DownloadRecoveries("HTC", "Desire_Eye", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire HD":
                        {
                            await DownloadRecoveries("HTC", "Desire_HD", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire S":
                        {
                            await DownloadRecoveries("HTC", "Desire_S", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire SV":
                        {
                            await DownloadRecoveries("HTC", "Desire_SV", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Desire V":
                        {
                            await DownloadRecoveries("HTC", "Desire_V", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Desire X":
                        {
                            await DownloadRecoveries("HTC", "Desire_X", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Droid DNA":
                        {
                            await DownloadRecoveries("HTC", "Droid_DNA", 1);
                        }
                        break;

                    case "Droid Incredible":
                        {
                            await DownloadRecoveries("HTC", "Droid_Incredible", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Droid Incredible 2":
                        {
                            await DownloadRecoveries("HTC", "Droid_Incredible_2", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            await DownloadRecoveries("HTC", "Droid_Incredible_4G_LTE", 1);
                        }
                        break;

                    case "Droid Incredible S":
                        {
                            await DownloadRecoveries("HTC", "Droid_Incredible_S", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "EVO 3D":
                        {
                            await DownloadRecoveries("HTC", "EVO_3D", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (CDMA)";
                        }
                        break;

                    case "EVO 4G":
                        {
                            await DownloadRecoveries("HTC", "EVO_4G", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "EVO 4G LTE":
                        {
                            await DownloadRecoveries("HTC", "EVO_4G_LTE", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "EVO Design":
                        {
                            await DownloadRecoveries("HTC", "EVO_Design", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "EVO Shift 4G":
                        {
                            await DownloadRecoveries("HTC", "EVO_Shift_4G", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Explorer":
                        {
                            await DownloadRecoveries("HTC", "Explorer", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "First":
                        {
                            await DownloadRecoveries("HTC", "First", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "myTouch 4G Slide":
                        {
                            await DownloadRecoveries("HTC", "myTouch_4G_Slide", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One A9":
                        {
                            await DownloadRecoveries("HTC", "One_A9", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One E8":
                        {
                            await DownloadRecoveries("HTC", "One_E8", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One E9+":
                        {
                            await DownloadRecoveries("HTC", "One_E9", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One J":
                        {
                            await DownloadRecoveries("HTC", "One_J", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One M7":
                        {
                            await DownloadRecoveries("HTC", "One_M7", 3);
                            firstStepButton.IsEnabled = true;
                            firstStepButton.Content = "Verizon M7 Only";
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (Sprint)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "TWRP (Verizon)";
                        }
                        break;

                    case "One M7 (Dual SIM)":
                        {
                            await DownloadRecoveries("HTC", "One_M7_Dual_SIM", 3);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (802d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (802t)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "TWRP (802w)";
                        }
                        break;

                    case "One M8":
                        {
                            await DownloadRecoveries("HTC", "One_M8", 1);
                            firstStepButton.IsEnabled = true;
                            firstStepButton.Content = "Verizon M8 Only";
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One M8 Eye":
                        {
                            await DownloadRecoveries("HTC", "One_M8_Eye", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One M9":
                        {
                            await DownloadRecoveries("HTC", "One_M9", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One Max":
                        {
                            await DownloadRecoveries("HTC", "One_Max", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (Sprint)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (Verizon)";
                        }
                        break;

                    case "One Mini":
                        {
                            await DownloadRecoveries("HTC", "One_Mini", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One Mini 2":
                        {
                            await DownloadRecoveries("HTC", "One_Mini_2", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One Remix":
                        {
                            await DownloadRecoveries("HTC", "One_Remix", 1);
                        }
                        break;

                    case "One S":
                        {
                            await DownloadRecoveries("HTC", "One_S", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (S4)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (S3)";
                        }
                        break;

                    case "One SV":
                        {
                            await DownloadRecoveries("HTC", "One_SV", 3);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (LTE)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "TWRP (Boost)";
                        }
                        break;

                    case "One V":
                        {
                            await DownloadRecoveries("HTC", "One_V", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (CDMA)";
                        }
                        break;

                    case "One VX":
                        {
                            await DownloadRecoveries("HTC", "One_VX", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "One X":
                        {
                            await DownloadRecoveries("HTC", "One_X", 2);
                            firstStepButton.Content = "AT&T One X Only";
                            firstStepButton.IsEnabled = true;
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (AT&T)";
                        }
                        break;

                    case "One X+":
                        {
                            await DownloadRecoveries("HTC", "One_X_Plus", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (AT&T)";
                        }
                        break;

                    case "Rezound":
                        {
                            await DownloadRecoveries("HTC", "Rezound", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Rhyme":
                        {
                            await DownloadRecoveries("HTC", "Rhyme", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                            recovery1Button.Content = "Flash CWM";
                        }
                        break;

                    case "Sensation":
                        {
                            await DownloadRecoveries("HTC", "Sensation", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Sensation XL":
                        {
                            await DownloadRecoveries("HTC", "Sensation_XL", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Vivid":
                        {
                            await DownloadRecoveries("HTC", "Vivid", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Wildfire":
                        {
                            await DownloadRecoveries("HTC", "Wildfire", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    case "Wildfire S":
                        {
                            await DownloadRecoveries("HTC", "Wildfire_S", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Token ID";
                        }
                        break;

                    //Huawei Devices
                    case "Honor 5X":
                        {
                            await DownloadRecoveries("Huawei", "Honor_5X", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                        }
                        break;

                    case "Mate 8":
                        {
                            await DownloadRecoveries("Huawei", "Mate_8", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                        }
                        break;

                    case "P8":
                        {
                            await DownloadRecoveries("Huawei", "P8", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                        }
                        break;

                    case "Watch":
                        {
                            await DownloadRecoveries("Huawei", "Watch", 1);
                        }
                        break;

                    case "Y635":
                        {
                            await DownloadRecoveries("Huawei", "Y635", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                        }
                        break;

                    //Kazam Devices
                    case "Thunder Q4.5":
                        {
                            await DownloadRecoveries("Kazam", "Thunder_Q45", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Tornado 348":
                        {
                            await DownloadRecoveries("Kazam", "Tornado_348", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //Lava Devices
                    case "Iris X8":
                        {
                            await DownloadRecoveries("Lava", "Iris_X8", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //Lenovo Devices
                    case "Zuk Z1":
                        {
                            await DownloadRecoveries("Lenovo", "Zuk_Z1", 1);
                        }
                        break;

                    //LG Devices
                    case "G4":
                        {
                            await DownloadRecoveries("LG", "G4", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Device ID";
                        }
                        break;

                    case "G Watch":
                        {
                            await DownloadRecoveries("LG", "G_Watch", 1);
                        }
                        break;

                    case "G Watch R":
                        {
                            await DownloadRecoveries("LG", "G_Watch_R", 1);
                        }
                        break;

                    case "Watch Urbane":
                        {
                            await DownloadRecoveries("LG", "Watch_Urbane", 1);
                        }
                        break;

                    //Motorola Devices
                    case "Moto 360":
                        {
                            await DownloadRecoveries("Motorola", "Moto_360", 1);
                        }
                        break;

                    case "Moto E":
                        {
                            await DownloadRecoveries("Motorola", "Moto_E", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto E (2015)":
                        {
                            await DownloadRecoveries("Motorola", "Moto_E_2015", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                            recovery1Button.Content = "TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (LTE)";
                        }
                        break;

                    case "Moto G":
                        {
                            await DownloadRecoveries("Motorola", "Moto_G", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G (2014)":
                        {
                            await DownloadRecoveries("Motorola", "Moto_G_2014", 2);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                            recovery1Button.Content = "TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (LTE)";
                        }
                        break;

                    case "Moto G (2015)":
                        {
                            await DownloadRecoveries("Motorola", "Moto_G_2015", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G Force":
                        {
                            await DownloadRecoveries("Motorola", "Moto_G_Force", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto Maxx":
                        {
                            await DownloadRecoveries("Motorola", "Moto_Maxx", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X":
                        {
                            await DownloadRecoveries("Motorola", "Moto_X", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X (2014)":
                        {
                            await DownloadRecoveries("Motorola", "Moto_X_2014", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X Force":
                        {
                            await DownloadRecoveries("Motorola", "Moto_X_Force", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X Play":
                        {
                            await DownloadRecoveries("Motorola", "Moto_X_Play", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto X Style (Pure)":
                        {
                            await DownloadRecoveries("Motorola", "Moto_X_Style", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Photon Q":
                        {
                            await DownloadRecoveries("Motorola", "Photon_Q", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Xoom":
                        {
                            await DownloadRecoveries("Motorola", "Xoom", 1);
                        }
                        break;

                    //Nvidia Devices
                    case "Shield":
                        {
                            await DownloadRecoveries("Nvidia", "Shield", 1);
                        }
                        break;

                    case "Shield Tablet":
                        {
                            await DownloadRecoveries("Nvidia", "Shield_Tablet", 1);
                        }
                        break;

                    case "Shield TV":
                        {
                            await DownloadRecoveries("Nvidia", "Shield_TV", 1);
                        }
                        break;

                    case "Tegra Note 7":
                        {
                            await DownloadRecoveries("Nvidia", "Tegra_Note_7", 1);
                        }
                        break;

                    //OnePlus Devices
                    case "OnePlus One":
                        {
                            await DownloadRecoveries("OnePlus", "One", 1);
                        }
                        break;

                    case "OnePlus 2":
                        {
                            await DownloadRecoveries("OnePlus", "Two", 1);
                        }
                        break;

                    case "OnePlus X":
                        {
                            await DownloadRecoveries("OnePlus", "X", 1);
                        }
                        break;

                    //Oppo Devices
                    case "Find 5":
                        {
                            await DownloadRecoveries("Oppo", "Find_5", 1);
                        }
                        break;

                    case "Find 7":
                        {
                            await DownloadRecoveries("Oppo", "Find_7", 1);
                        }
                        break;

                    case "N1":
                        {
                            await DownloadRecoveries("Oppo", "N1", 1);
                        }
                        break;

                    case "N3":
                        {
                            await DownloadRecoveries("Oppo", "N3", 1);
                        }
                        break;

                    case "R7":
                        {
                            await DownloadRecoveries("Oppo", "R7", 2);
                            recovery1Button.Content = "TWRP (R7)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (R7 Plus)";

                        }
                        break;

                    case "R819":
                        {
                            await DownloadRecoveries("Oppo", "R819", 1);
                        }
                        break;

                    //Samsung Devices
                    case "Gear Live":
                        {
                            await DownloadRecoveries("Samsung", "Gear_Live", 1);
                        }
                        break;

                    //Sony Devices
                    case "SmartWatch 3":
                        {
                            await DownloadRecoveries("Sony", "SmartWatch_3", 1);
                        }
                        break;

                    case "Xperia M4 Aqua":
                        {
                            await DownloadRecoveries("Sony", "Xperia_M4_Aqua", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                        }
                        break;

                    case "Xperia Z5":
                        {
                            await DownloadRecoveries("Sony", "Xperia_Z5", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                            gainRootButton.Content = "Flash Root Files";
                        }
                        break;

                    case "Xperia Z5 Compact":
                        {
                            await DownloadRecoveries("Sony", "Xperia_Z5_Compact", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                            gainRootButton.Content = "Flash Root Files";
                        }
                        break;

                    case "Xperia Z5 Premium":
                        {
                            await DownloadRecoveries("Sony", "Xperia_Z5_Premium", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Get Unlock Code";
                            gainRootButton.Content = "Flash Root Files";
                        }
                        break;

                    //Wileyfox Devices
                    case "Storm":
                        {
                            await DownloadRecoveries("Wileyfox", "Storm", 1);
                        }
                        break;

                    case "Swift":
                        {
                            await DownloadRecoveries("Wileyfox", "Swift", 1);
                        }
                        break;

                    //Xiaomi Devices
                    case "Mi 3":
                        {
                            await DownloadRecoveries("Xiaomi", "Mi_3", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Mi 4c":
                        {
                            await DownloadRecoveries("Xiaomi", "Mi_4c", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Request Unlock";
                            await this.ShowMessageAsync("Unlock Update!", "You only need to unlock your bootloader if you're running MIUI 6.1.12 or later. If not, you can skip that part and go right to flashing a recovery and gaining root.", MessageDialogStyle.Affirmative, mySettings);
                        }
                        break;

                    case "Mi Note Pro":
                        {
                            await DownloadRecoveries("Xiaomi", "Mi_Note_Pro", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Request Unlock";
                            await this.ShowMessageAsync("Unlock Update!", "You only need to unlock your bootloader if you're running MIUI 6.1.12 or later. If not, you can skip that part and go right to flashing a recovery and gaining root.", MessageDialogStyle.Affirmative, mySettings);
                        }
                        break;

                    case "Mi Pad":
                        {
                            await DownloadRecoveries("Xiaomi", "Mi_Pad", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Mi Pad 2":
                        {
                            await DownloadRecoveries("Xiaomi", "Mi_Pad_2", 1);
                        }
                        break;

                    case "Redmi 1S":
                        {
                            await DownloadRecoveries("Xiaomi", "Redmi_1S", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Redmi 2":
                        {
                            await DownloadRecoveries("Xiaomi", "Redmi_2", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Redmi Note":
                        {
                            await DownloadRecoveries("Xiaomi", "Redmi_Note", 2);
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.Content = "TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "TWRP (4G)";
                        }
                        break;

                    case "Redmi Note 2":
                        {
                            await DownloadRecoveries("Xiaomi", "Redmi_Note_2", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Redmi Note 3":
                        {
                            await DownloadRecoveries("Xiaomi", "Redmi_Note_3", 1);
                            secondStepButton.IsEnabled = true;
                            secondStepButton.Content = "Request Unlock";
                            await this.ShowMessageAsync("Unlock Update!", "You only need to unlock your bootloader if you're running MIUI 6.1.12 or later. If not, you can skip that part and go right to flashing a recovery and gaining root.", MessageDialogStyle.Affirmative, mySettings);
                        }
                        break;

                    //XOLO Devices
                    case "Q1010i":
                        {
                            await DownloadRecoveries("XOLO", "Q1010i", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    case "Q3000":
                        {
                            await DownloadRecoveries("XOLO", "Q3000", 1);
                            unlockBootloaderButton.IsEnabled = false;
                        }
                        break;

                    //YU Devices
                    case "Yunique":
                        {
                            await DownloadRecoveries("YU", "Yunique", 1);
                        }
                        break;

                    case "Yuphoria":
                        {
                            await DownloadRecoveries("YU", "Yuphoria", 1);
                        }
                        break;

                    case "Yureka":
                        {
                            await DownloadRecoveries("YU", "Yureka", 1);
                        }
                        break;

                    //Other
                    case "iPhone 6S":
                        {
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.IsEnabled = false;
                        }
                        break;

                    case "None":
                        {
                            unlockBootloaderButton.IsEnabled = false;
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.IsEnabled = false;
                        }
                        break;
                }
                Settings.Default["Device"] = device;
                Settings.Default["Manufacturer"] = manufacturer;
                Settings.Default.Save();
                ((Flyout)Flyouts.Items[0]).IsOpen = false;
                if (manufacturer == "Other" || manufacturer == "OnePlus" || device == "Android One")
                {
                    DeviceTextBox.Text = "Current Device: " + device;
                    if (device == "iPhone 6S")
                    {
                        Settings.Default["Device"] = "None";
                        Settings.Default.Save();
                        await this.ShowMessageAsync("Not Gonna Happen!", "Steve Jobs would rise from the dead and destroy Apple before iPhones could actually work with this toolkit.", MessageDialogStyle.Affirmative, mySettings);
                        DeviceTextBox.Text = "Current Device: How about no.";
                    }
                }
                else
                {
                    DeviceTextBox.Text = "Current Device: " + manufacturer + " " + device;
                }
                Log.AddLogItem("Device changed to " + manufacturer + " " + device + ".", "DEVICE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void firstStepButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var device = Settings.Default["Device"].ToString();
                if (device == "One M7" || device == "One M8")
                {
                    var result = await this.ShowMessageAsync("Verizon Sucks!", "Due to restrictions put in place by Verizon, you must use another program to unlock your phone. Would you like to try this method?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start("http://theroot.ninja/");
                    }
                    Log.AddLogItem("One M7/M8 Verizon unlocking method clicked.", "SUPERCID");
                }
                else if (device == "One X")
                {
                    var result = await this.ShowMessageAsync("AT&T Sucks!", "Due to restrictions put in place by AT&T, you must use another program to unlock your phone. Would you like to try this method?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {

                        Process.Start("http://rumrunner.us/downloads-2/");
                        Log.AddLogItem("Rumrunner website opened.", "ONEXATT");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void secondStepButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Huawei",
                    NegativeButtonText = "DC",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings3 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var manufacturer = Settings.Default["Manufacturer"].ToString();
                var device = Settings.Default["Device"].ToString();
                if (manufacturer == "HTC")
                {
                    var result = await this.ShowMessageAsync("It's Token Time!", "This command will get your device's Token ID, then open a text file with your token and further instructions. Are you ready to continue?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("HTC Token ID command started.", "HTCTOKEN");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "HTCTOKEN");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting to bootloader.", "HTCTOKEN");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                GetUnlockString();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                GetUnlockString();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "HTCTOKEN");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings3);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "HTCTOKEN");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings3);
                        }
                    }
                }
                else if (manufacturer == "Huawei")
                {
                    if (device == "Y635" || device == "Honor 5X")
                    {
                        var result = await this.ShowMessageAsync("Get Unlock Code!", "This command will offer some options to help get your unlock code. Are you ready to continue?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            await this.ShowMessageAsync("Huawei!", "On the page that's about to open, you must sign up for a Huawei account and follow the instructions they gibe. Then, you will be able to get your unlock code from the site. Click OK when you're ready to continue.", MessageDialogStyle.Affirmative, mySettings3);
                            Process.Start("https://emui.huawei.com/en/plugin.php?id=unlock&mod=detail");
                            Log.AddLogItem("Huawei website opened.", "Y635");
                        }
                    }

                    if (device == "Mate 8" || device == "P8")
                    {
                        var result = await this.ShowMessageAsync("Get Unlock String!", "This command will offer some options to help get your unlock code. Are you ready to continue?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            var result2 = await this.ShowMessageAsync("Unlock Options!", "You have 2 options to unlock your device's bootloader. You can use the official unlock with Huawei for free, but it could take up to 14 days to get your unlock code. The other option is to use DC-Unlocker, which will get your code immediately, but it costs $5. Which option would you like to use?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                await this.ShowMessageAsync("Huawei!", "On the page that's about to open, you must sign up for a Huawei account, then login with it on your phone for 14 days. Then, you will be able to get your unlock code from the site. Click OK when you're ready to continue.", MessageDialogStyle.Affirmative, mySettings3);
                                Process.Start("https://uniportal.huawei.com/accounts/register.do?method=toRegister");
                                Log.AddLogItem("Huawei website opened.", "MATE8");
                            }
                            else if (result2 == MessageDialogResult.Negative)
                            {
                                await this.ShowMessageAsync("DC-Unlocker!", "On the page that's about to open, you must buy 4 credits. Then, download the DC Unlocker software and follow the instructions in their tutorial. Click OK when you're ready to continue.", MessageDialogStyle.Affirmative, mySettings3);
                                Process.Start("https://www.dc-unlocker.com/buy");
                                Log.AddLogItem("DC-Unlocker website opened.", "MATE8");
                            }
                        }
                    }
                }
                else if (manufacturer == "LG")
                {
                    var result = await this.ShowMessageAsync("Get Device ID!", "This command will get your device's Device ID, then open a text file with your ID and further instructions. Are you ready to continue?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("LG Device ID command started.", "LGID");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "LGID");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting to bootloader.", "LGID");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                GetUnlockString();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                GetUnlockString();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "LGID");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings3);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "LGID");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings3);
                        }
                    }
                }
                else if (manufacturer == "Motorola")
                {
                    var result = await this.ShowMessageAsync("Get Unlock String!", "This command will get your device's unlock string, then open a text file with your string and further instructions. Are you ready to continue?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("Motorola Unlock Key command started.", "MOTOKEY");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "MOTOKEY");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting to bootloader.", "MOTOKEY");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                GetUnlockString();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                GetUnlockString();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "MOTOKEY");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings3);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "MOTOKEY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings3);
                        }
                    }
                }
                else if (manufacturer == "Sony")
                {
                    var result = await this.ShowMessageAsync("Get Unlock Code!", "This command will help you get the unlock code needed to unlock your bootloader. Are you ready to continue?",
                                MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("Sony Unlock Code command started.", "SONYCODE");
                        await this.ShowMessageAsync("Sony Unlock Site!", "A webpage will now open that asks for your email address. This is how Sony makes you get your unlock code, so follow the steps on the site. Once you get your unlock code, continue on to 'Unlock Bootloader'.", MessageDialogStyle.Affirmative, mySettings3);
                        Process.Start("http://developer.sonymobile.com/unlockbootloader/email-verification/");
                    }
                }
                else if (manufacturer == "Xiaomi")
                {
                    var result = await this.ShowMessageAsync("Request Unlock!", "This command will help you send an unlock request to Xiaomi so you can unlock your bootloader. Are you ready to continue?",
                                MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("Xiaomi unlock request command started.", "XIAOMI");
                        await this.ShowMessageAsync("Xiaomi Website!", "The Xiaomi Unlock website will now open. Click on 'Unlock Now' in the center of the page, and follow the given instructions. You may have to translate the page from Chinese. Your unlock request can take up to 2 weeks to be approved. Once your request have been approved, continue on to 'Unlock Bootloader'.", MessageDialogStyle.Affirmative, mySettings3);
                        Process.Start("http://en.miui.com/unlock/");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void GetUnlockString()
        {
            try
            {
                var manufacturer = Settings.Default["Manufacturer"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Connected: Fastboot.", "STRING");
                if (manufacturer == "HTC")
                {
                    var controller2 = await this.ShowProgressAsync("Getting Token ID...", "");
                    controller2.SetIndeterminate();
                    using (var sw = File.CreateText("./Data/token.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "HTCTOKEN");
                        var rawReturn = await Task.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_identifier_token")));
                        Log.AddLogItem("Getting token ID.", "HTCTOKEN");
                        var rawToken = GetStringBetween(rawReturn, "< Please cut following message >\r\n",
                            "\r\nOKAY");
                        var cleanedToken = rawToken.Replace("(bootloader) ", "");
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
                        await Task.Run(() => UpdateDevice());
                        await controller2.CloseAsync();
                    }
                }
                else if (manufacturer == "LG")
                {
                    var controller2 = await this.ShowProgressAsync("Getting Device ID...", "");
                    controller2.SetIndeterminate();
                    using (var sw = File.CreateText("./Data/deviceid.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "LGID");
                        var rawReturn = await Task.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "device-id")));
                        Log.AddLogItem("Getting device id.", "LGID");
                        var cleanedID = rawReturn.Replace("(bootloader) ", "");
                        sw.WriteLine(cleanedID);
                        sw.WriteLine(" ");
                        sw.WriteLine("Please copy the two lines of letters and numbers below 'Device-ID'. Combine them into one line.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Next, sign into your LG Developer account on the webpage that just opened.");
                        sw.WriteLine("If you do not have an account, create and activate an account with your email, then come back to this link.");
                        sw.WriteLine("https://developer.lge.com/secure/Login.dev");
                        sw.WriteLine("Then, go through the steps on the LG website.");
                        sw.WriteLine("You will need your IMEI for this. To get your IMEI, go to Settings>About Phone>Status on your phone.");
                        sw.WriteLine("Once you input your IMEI, paste the Device ID from the top of this document to the website. Make sure it is all in one line with no spaces.");
                        sw.WriteLine("Once you have inputted everything correctly, click 'Confirm'. You will be sent an email with your unlock file.");
                        sw.WriteLine(" ");
                        sw.WriteLine("Once you have received the unlock file, download it and continue on to the next step, unlocking your bootloader.");
                        sw.WriteLine("This file is saved as deviceid.txt in the Data folder if you need it in the future.");
                        Log.AddLogItem("Writing device ID to text file.", "LGID");
                        await Task.Run(() => UpdateDevice());
                        await controller2.CloseAsync();
                    }
                }
                else if (manufacturer == "Motorola")
                {
                    var controller2 = await this.ShowProgressAsync("Getting Unlock Key...", "");
                    using (var sw = File.CreateText("./Data/unlockstring.txt"))
                    {
                        Log.AddLogItem("Creating text file.", "MOTOKEY");
                        var rawReturn = await Task.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_unlock_data")));
                        Log.AddLogItem("Getting unlock string.", "MOTOKEY");
                        var rawToken = GetStringBetween(rawReturn, "...\r\n",
                            "\r\nOKAY");
                        var firstCleanedToken = rawToken.Replace("(bootloader) ", "");
                        File.WriteAllText("./Data/unlockstringRAW.txt", firstCleanedToken);
                        Log.AddLogItem("Writing raw unlock string.", "MOTOKEY");
                        var secondCleanedToken = File.ReadAllText(@"./Data/unlockstringRAW.txt").Replace(Environment.NewLine, " ");
                        var finalCleanedToken = secondCleanedToken.Replace(" ", "");
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
                        File.Delete("./Data/unlockstringRAW.txt");
                        await Task.Run(() => UpdateDevice());
                        await controller2.CloseAsync();
                    }
                }
                var result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                Log.AddLogItem("Unlock string retrieval completed.", "STRING");
                if (result2 == MessageDialogResult.Affirmative)
                {
                    var controller3 = await this.ShowProgressAsync("Rebooting Device...", "");
                    controller3.SetIndeterminate();
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                    Log.AddLogItem("Rebooting device.", "STRING");
                    await Task.Run(() => _android.WaitForDevice());
                    await Task.Run(() => UpdateDevice());
                    await Task.Run(() => _android.Dispose());
                    await controller3.CloseAsync();
                    if (manufacturer == "HTC")
                    {
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        Log.AddLogItem("Token ID text file opened.", "HTCTOKEN");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                    else if (manufacturer == "LG")
                    {
                        Process.Start("https://developer.lge.com/secure/Login.dev");
                        Log.AddLogItem("LG Dev website opened.", "LGID");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/deviceid.txt");
                        Log.AddLogItem("LG ID text file opened.", "LGID");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from LG, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                    else if (manufacturer == "Motorola")
                    {
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlock string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
                else
                {
                    if (manufacturer == "HTC")
                    {
                        Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                        Log.AddLogItem("HTC Dev website opened.", "HTCTOKEN");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        Log.AddLogItem("Token ID text file opened.", "LGID");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from HTC, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                    else if (manufacturer == "LG")
                    {
                        Process.Start("https://developer.lge.com/secure/Login.dev");
                        Log.AddLogItem("LG Dev website opened.", "LGID");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/deviceid.txt");
                        Log.AddLogItem("LG ID text file opened.", "LGID");
                        await this.ShowMessageAsync("Next Step!", "Once you have received the unlock file from LG, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                    else if (manufacturer == "Motorola")
                    {
                        Process.Start("https://motorola-global-portal.custhelp.com/app/standalone/bootloader/unlock-your-device-b");
                        Log.AddLogItem("Motorola website opened.", "MOTOKEY");
                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/unlockstring.txt");
                        Log.AddLogItem("Unlock string text file opened.", "MOTOKEY");
                        await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock key from Motorola, you can move on to the next step, unlocking your bootloader!", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void unlockBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var device = Settings.Default["Device"].ToString();
                var manufacturer = Settings.Default["Manufacturer"].ToString();
                if (device == "Droid DNA" || device == "One Remix")
                {
                    var result = await this.ShowMessageAsync("Verizon Sucks!", "Your device cannot be unlocked through this toolkit. However, you can utilize another method to unlock your device. Would you like to try this alternate method now?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start("http://theroot.ninja/");
                        Log.AddLogItem("Sunshine website opened.", "VERIZON");
                    }
                }
                else if (device == "Droid Incredible 4G LTE")
                {
                    var result = await this.ShowMessageAsync("Verizon Sucks!", "Your device cannot be unlocked through this toolkit. However, you can utilize another method to unlock your device. Would you like to try this alternate method now?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start("http://rumrunner.us/downloads-2/");
                        Log.AddLogItem("Rumrunner website opened.", "VERIZON");
                    }
                }
                else if (manufacturer == "Alcatel" || manufacturer == "Google" || manufacturer == "Lenovo" || manufacturer == "Nvidia" || manufacturer == "OnePlus" || manufacturer == "Oppo" || manufacturer == "Wileyfox" || manufacturer == "YU" || device == "G Watch" || device == "G Watch R" || device == "Gear Live" || device == "Mi Pad 2" || device == "Moto 360" || device == "SmartWatch 3" || device == "Watch" || device == "Watch Urbane" || device == "Xoom" || device == "ZenFone Go" || device == "ZenWatch")
                {
                    var result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe your device. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("Basic unlock command started.", "BASICUNLOCK");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            await this.ShowMessageAsync("Double Check!", "Some newer devices require 'Enable OEM Unlock' to be checked in Settings>Developer Options before you can unlock your bootloader. Please check this before continuing!", MessageDialogStyle.Affirmative, mySettings2);
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "BASICUNLOCK");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "BASICUNLOCK");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                BasicUnlock();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                BasicUnlock();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "BASICUNLOCK");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "BASICUNLOCK");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
                else if (manufacturer == "Asus")
                {
                    if (device == "ZenFone 2" || device == "ZenFone 5" || device == "ZenFone 6" || device == "ZenFone Laser" || device == "ZenFone Selfie")
                    {
                        ZenFoneUnlock();
                    }
                }
                else if (manufacturer == "HTC" || manufacturer == "LG")
                {
                    var result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe your device. You must have downloaded & saved the unlock file you got from your email. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("File unlock command started.", "FILEUNLOCK");
                        Log.AddLogItem("Unlock file selected.", "FILEUNLOCK");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            await this.ShowMessageAsync("Double Check!", "Some newer devices require 'Enable OEM Unlock' to be checked in Settings>Developer Options before you can unlock your bootloader. Please check this before continuing!", MessageDialogStyle.Affirmative, mySettings2);
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "FILEUNLOCK");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "FILEUNLOCK");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FileUnlock();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FileUnlock();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "FILEUNLOCK");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No Device Found.", "FILEUNLOCK");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
                else if (manufacturer == "Huawei" || manufacturer == "Motorola" || manufacturer == "Sony")
                {
                    var result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe your device. You must have your unlock key from the previous step ready. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Log.AddLogItem("General unlock key command started.", "KEYUNLOCK");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            await this.ShowMessageAsync("Double Check!", "Some newer devices require 'Enable OEM Unlock' to be checked in Settings>Developer Options before you can unlock your bootloader. Please check this before continuing!", MessageDialogStyle.Affirmative, mySettings2);
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "MOTOUNLOCK");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "KEYUNLOCK");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                CodeUnlock();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                CodeUnlock();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "KEYUNLOCK");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "KEYUNLOCK");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
                else if (manufacturer == "Xiaomi")
                {
                    if (device == "Mi 4c" || device == "Mi Note Pro" || device == "Redmi Note 3")
                    {
                        var result = await this.ShowMessageAsync("Ready To Unlock?", "This will download and run a program that will unlock your bootloader and completely wipe your device. Are you ready to continue?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                            controller9.SetIndeterminate();
                            await Task.Run(() => UpdateDevice());
                            if (await Task.Run(() => _android.HasConnectedDevices))
                            {
                                await controller9.CloseAsync();
                                if (_device.State.ToString() == "ONLINE")
                                {
                                    if (File.Exists("./Data/Downloads/UnlockXiaomi/MiFlashUnlock.exe"))
                                    {
                                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/Downloads/UnlockXiaomi/MiFlashUnlock.exe");
                                    }
                                    else
                                    {
                                        Log.AddLogItem("Connected: Online.", "UNLOCK");
                                        await DownloadFile("Xiaomi Unlock Program", "https://s3.amazonaws.com/windroid/Devices/Xiaomi/Unlock.zip", "./Data/Downloads/Unlock.zip");
                                        ZipFile.ExtractToDirectory("./Data/Downloads/Unlock.zip", "./Data/Downloads/UnlockXiaomi");
                                        File.Delete("./Data/Downloads/Unlock.zip");
                                        Log.AddLogItem("Xiaomi unlock zip deleted.", "UNLOCK");
                                        Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/Downloads/UnlockXiaomi/MiFlashUnlock.exe");
                                        Log.AddLogItem("Xiaomi unlock program opened.", "UNLOCK");
                                    }
                                }
                                else
                                {
                                    Log.AddLogItem("Wrong device state.", "UNLOCK");
                                    await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                                }
                            }
                            else
                            {
                                await controller9.CloseAsync();
                                Log.AddLogItem("No device found.", "UNLOCK");
                                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void BasicUnlock()
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Connected: Fastboot.", "BASICUNLOCK");
                var controller2 = await this.ShowProgressAsync("Unlocking Device...", "Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button to confirm.");
                controller2.SetIndeterminate();
                if (device == "Nexus 6P")
                {
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flashing", "unlock")));
                    Log.AddLogItem("Unlocking bootloader.", "BASICUNLOCK");
                    await Task.Run(() => UpdateDevice());
                    await Task.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging when your device finishes rebooting..", MessageDialogStyle.Affirmative, mySettings);
                    Log.AddLogItem("Bootloader unlock successful.", "BASICUNLOCK");
                }
                else if (device == "Nexus Player")
                {
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    Log.AddLogItem("Unlocking bootloader.", "ATVUNLOCK");
                    await Task.Run(() => UpdateDevice());
                    await Task.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging when your device finishes rebooting..", MessageDialogStyle.Affirmative, mySettings);
                    Log.AddLogItem("Bootloader unlock successful.", "ATVUNLOCK");
                }
                else
                {
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    Log.AddLogItem("Unlocking bootloader.", "BASICUNLOCK");
                    await Task.Run(() => UpdateDevice());
                    await Task.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging when your device finishes rebooting..", MessageDialogStyle.Affirmative, mySettings);
                    Log.AddLogItem("Bootloader unlock successful.", "BASICUNLOCK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void FileUnlock()
        {
            try
            {
                var manufacturer = Settings.Default["Manufacturer"].ToString();
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Unlock Files (*.bin*)|*.bin*", Multiselect = false };
                ofd.ShowDialog();
                if (File.Exists(ofd.FileName))
                {
                    Log.AddLogItem("Connected: Fastboot.", "FILEUNLOCK");
                    var controller2 = await this.ShowProgressAsync("Unlocking Device...", "");
                    controller2.SetIndeterminate();
                    if (manufacturer == "HTC")
                    {
                        controller2.SetMessage("Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button to confirm the unlock.");
                        await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlocktoken " + ofd.FileName)));
                    }
                    else if (manufacturer == "LG")
                    {
                        await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlock " + ofd.FileName)));
                    }
                    Log.AddLogItem("Unlocking bootloader.", "FILEUNLOCK");
                    await Task.Run(() => _android.Dispose());
                    await controller2.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative, mySettings);
                    Log.AddLogItem("Bootloader unlock successful.", "FILEUNLOCK");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void CodeUnlock()
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Connected: Fastboot.", "KEYUNLOCK");
                var unlockKey = await this.ShowInputAsync("Paste Unlock Code", "Please paste the unlock code exactly as you received it. It is case-sensitive, and should contain no spaces.");
                if (unlockKey == null)
                    return;
                Log.AddLogItem("User pasting unlock key.", "KEYUNLOCK");
                var controller2 = await this.ShowProgressAsync("Unlocking Device...", "");
                controller2.SetIndeterminate();
                await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock " + unlockKey)));
                Log.AddLogItem("Unlocking bootloader.", "KEYUNLOCK");
                await Task.Run(() => _android.Dispose());
                await controller2.CloseAsync();
                await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative, mySettings);
                Log.AddLogItem("Bootloader unlock successful.", "KEYUNLOCK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void ZenFoneUnlock()
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    AffirmativeButtonText = "ZE550KL",
                    NegativeButtonText = "ZE551KL",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings3 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                if (device == "ZenFone 2" || device == "ZenFone 5" || device == "ZenFone 6")
                {
                    var result = await this.ShowMessageAsync("Ready To Unlock?", "This will download and run a program that will unlock your bootloader and completely wipe your device. Are you ready to continue?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "UNLOCK");
                                if (File.Exists("./Data/Downloads/UnlockAsus/Unlock.bat"))
                                {
                                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/Downloads/UnlockAsus/Unlock.bat");
                                    Log.AddLogItem("ZenFone unlock program opened.", "UNLOCK");
                                }
                                else
                                {
                                    if (device == "ZenFone 2")
                                    {
                                        await DownloadFile("ZenFone 2 Unlock Program", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_2/Unlock.zip", "./Data/Downloads/Unlock.zip");
                                    }
                                    else if (device == "ZenFone 5" || device == "ZenFone 6")
                                    {
                                        await DownloadFile("ZenFone 5/6 Unlock Program", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_5/Unlock.zip", "./Data/Downloads/Unlock.zip");
                                    }
                                    ZipFile.ExtractToDirectory("./Data/Downloads/Unlock.zip", "./Data/Downloads/UnlockAsus");
                                    File.Delete("./Data/Downloads/Unlock.zip");
                                    Log.AddLogItem("ZenFone unlock zip deleted.", "UNLOCK");
                                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "/Data/Downloads/UnlockAsus/Unlock.bat");
                                    Log.AddLogItem("ZenFone unlock program opened.", "UNLOCK");
                                }
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "UNLOCK");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings3);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "UNLOCK");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings3);
                        }
                    }
                }
                
                else if (device == "ZenFone Laser" || device == "ZenFone Selfie")
                {
                    var result = await this.ShowMessageAsync("Ready To Unlock?", "This will download and install an app on your phone that will allow you to unlock your bootloader. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "UNLOCK");
                                if (device == "ZenFone Laser")
                                {
                                    var result2 = await this.ShowMessageAsync("Device Check!", "Which version of the ZenFone Laser do you have?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                                    if (result2 == MessageDialogResult.Affirmative)
                                    {
                                        await DownloadFile("ZenFone Laser ZE550KL Unlock App", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_Laser/ZE550KL_Unlock.apk", "./Data/Downloads/Unlock.apk");
                                    }
                                    else if (result2 == MessageDialogResult.Negative)
                                    {
                                        await DownloadFile("ZenFone Laser ZE551KL Unlock App", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_Laser/ZE551KL_Unlock.apk", "./Data/Downloads/Unlock.apk");
                                    }
                                }
                                else if (device == "ZenFone Selfie")
                                {
                                    await DownloadFile("ZenFone Selfie Unlock App", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_Selfie/Selfie_Unlock.apk", "./Data/Downloads/Unlock.apk");
                                }
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Installing App...", "If the install process takes more than a minute, please check your device as you may have to accept a notice from Google.");
                                Log.AddLogItem("Installing app.", "UNLOCK");
                                if (await Task.Run(() => _device.InstallApk("./Data/Downloads/Unlock.apk").ToString()) == "True")
                                {
                                    await Task.Run(() => UpdateDevice());
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("Install Successful!", "The app was successfully installed!");
                                    Log.AddLogItem("App install successful.", "UNLOCK");
                                }
                                else
                                {
                                    await Task.Run(() => UpdateDevice());
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("App Install Failed!", "An issue occurred while attempting to install the app.", MessageDialogStyle.Affirmative, mySettings3);
                                    Log.AddLogItem("App install failed.", "APP");
                                }

                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "UNLOCK");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings3);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "UNLOCK");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void firstRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("First recovery flash clicked.", "RECOVERY");
                var result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash the TWRP recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    if (File.Exists("./Data/Recoveries/Recovery1.img"))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "RECOVERY");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                if (device == "One M9" || device == "One A9" || device == "Desire 626s")
                                {
                                    await Task.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                                    Log.AddLogItem("Rebooting device to download mode.", "RECOVERYNEWHTC");
                                }
                                else
                                {
                                    await Task.Run(() => _device.RebootBootloader());
                                    Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                                }
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FlashRecovery("Recovery1.img", true);
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FlashRecovery("Recovery1.img", true);
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "RECOVERY");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "RECOVERY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        Log.AddLogItem("Missing recovery.", "RECOVERY");
                        await this.ShowMessageAsync("Missing Recovery!", "This recovery appears to be missing from the Data folder! Please redownload the recoveries for your device.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void secondRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Second recovery flash clicked.", "RECOVERY");
                var result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash the TWRP recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    if (File.Exists("./Data/Recoveries/Recovery2.img"))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "RECOVERY");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                if (device == "One M9" || device == "One A9" || device == "Desire 626s")
                                {
                                    await Task.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                                    Log.AddLogItem("Rebooting device to download mode.", "RECOVERYNEWHTC");
                                }
                                else
                                {
                                    await Task.Run(() => _device.RebootBootloader());
                                    Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                                }
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FlashRecovery("Recovery2.img", true);
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FlashRecovery("Recovery2.img", true);
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "RECOVERY");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "RECOVERY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        Log.AddLogItem("Missing recovery.", "RECOVERY");
                        await this.ShowMessageAsync("Missing Recovery!", "This recovery appears to be missing from the Data folder! Please redownload the recoveries for your device.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void thirdRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Third recovery flash clicked.", "RECOVERY");
                var result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash the TWRP recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    if (File.Exists("./Data/Recoveries/Recovery3.img"))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "RECOVERY");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                if (device == "One M9" || device == "One A9" || device == "Desire 626s")
                                {
                                    await Task.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                                    Log.AddLogItem("Rebooting device to download mode.", "RECOVERYNEWHTC");
                                }
                                else
                                {
                                    await Task.Run(() => _device.RebootBootloader());
                                    Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                                }
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FlashRecovery("Recovery3.img", true);
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FlashRecovery("Recovery3.img", true);
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "RECOVERY");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "RECOVERY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        Log.AddLogItem("Missing recovery.", "RECOVERY");
                        await this.ShowMessageAsync("Missing Recovery!", "This recovery appears to be missing from the Data folder! Please redownload the recoveries for your device.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void gainRootButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var device = Settings.Default["Device"].ToString();
                if (device == "Nexus Player" || device == "ZenFone Laser")
                {
                    Log.AddLogItem("Boot root command started.", "ROOT");
                    var result = await this.ShowMessageAsync("Ready To Root?", "This command will download the rooted boot image, reboot your device into fastboot, then flash the rooted boot image. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "ROOT");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "ROOT");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                BootRoot();
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                BootRoot();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "ROOT");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "ROOT");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
                else if (device == "Xperia Z5" || device == "Xperia Z5 Compact" || device == "Xperia Z5 Premium")
                {
                    Log.AddLogItem("Sony root command started.", "ROOT");
                    var result = await this.ShowMessageAsync("Ready To Root?", "This command will download and push the necessary files to root your device, then reboot to TWRP so you can flash them. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "ROOT");
                                SonyRoot();
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "ROOT");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "ROOT");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
                else
                {
                    Log.AddLogItem("SuperSU root command started.", "ROOT");
                    var result = await this.ShowMessageAsync("Ready To Root?", "This command will download SuperSU, reboot your device into recovery, then allow you to flash SuperSU to permanently gain root. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "ROOT");
                                if (File.Exists("./Data/Downloads/SuperSU.zip"))
                                {
                                    SuperSU();
                                }
                                else
                                {
                                    if (device == "G Watch" || device == "G Watch R" || device == "Gear Live" || device == "Watch" || device == "Moto 360" || device == "SmartWatch 3" || device == "Watch Urbane" || device == "ZenWatch")
                                    {
                                        await DownloadFile("SuperSU", "https://s3.amazonaws.com/windroid/Root/WearSuperSU.zip", "./Data/Downloads/SuperSU.zip");
                                        Log.AddLogItem("Wear SuperSU pushing started.", "ROOT");
                                        SuperSU();
                                    }
                                    else
                                    {
                                        await DownloadFile("SuperSU", "https://s3.amazonaws.com/windroid/Root/SuperSU.zip", "./Data/Downloads/SuperSU.zip");
                                        Log.AddLogItem("SuperSU pushing started.", "ROOT");
                                        SuperSU();
                                    }
                                }
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "ROOT");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "ROOT");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void SuperSU()
        {
            try
            {
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var controller = await this.ShowProgressAsync("Pushing SuperSU...", "");
                controller.SetIndeterminate();
                if (await Task.Run(() => _device.PushFile("./Data/Downloads/SuperSU.zip", "/sdcard/SuperSU.zip").ToString() == "True"))
                {
                    await controller.CloseAsync();
                    Log.AddLogItem("SuperSU push successful.", "ROOT");
                    var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                    controller2.SetIndeterminate();
                    await Task.Run(() => _device.RebootRecovery());
                    Log.AddLogItem("Rebooting device to recovery.", "ROOT");
                    await Task.Delay(5000);
                    await controller2.CloseAsync();
                    if (device == "One A9")
                    {
                        await this.ShowMessageAsync("Encyption!", "Before you flash SuperSU, you must wipe your data partition in TWRP. To do this, tap on 'Wipe' in the top right corner, then tap on 'Advanced Wipe'. From the list, make sure only Data is checked, then perform the wipe. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                        await this.ShowMessageAsync("Flash Time!", "Now go back to the main screen, then tap on 'Install' in the top left corner. Then, scroll until you find 'SuperSU.zip'. Finally, tap on it, then swipe to confirm flash. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                        await this.ShowMessageAsync("Congratulations!", "You are all done with unlocking and rooting process! Hit the 'Reboot System' buttom in the bottom right corner in TWRP, and your device will boot up with root privileges.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                    else
                    {
                        await this.ShowMessageAsync("Your Turn!", "Once in TWRP, tap on 'Install' in the top left corner. Then, scroll until you find 'SuperSU.zip'. Finally, tap on it, then swipe to confirm flash. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                        await this.ShowMessageAsync("Congratulations!", "You are all done with unlocking and rooting process! Hit the 'Reboot System' buttom in the bottom right corner in TWRP, and your device will boot up with root privileges.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
                else
                {
                    Log.AddLogItem("SuperSU push failed.", "ROOT");
                    await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push SuperSU.zip. Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative, mySettings2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void SonyRoot()
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var device = Settings.Default["Device"].ToString();
                await DownloadFile("SuperSU", "https://s3.amazonaws.com/windroid/Root/SuperSU.zip", "./Data/Downloads/SuperSU.zip");
                await DownloadFile("DRMRestore", "https://s3.amazonaws.com/windroid/Devices/Sony/Xperia_Z5/DRMRestore.zip", "./Data/Downloads/DRMRestore.zip");
                switch (Settings.Default["Device"].ToString())
                {
                    case "Xperia Z5":
                        {
                            await DownloadFile("Root Image", "https://s3.amazonaws.com/windroid/Devices/Sony/Xperia_Z5/Root.img", "./Data/Downloads/Root.img");
                        }
                        break;

                    case "Xperia Z5 Compact":
                        {
                            await DownloadFile("Root Image", "https://s3.amazonaws.com/windroid/Devices/Sony/Xperia_Z5_Compact/Root.img", "./Data/Downloads/Root.img");
                        }
                        break;

                    case "Xperia Z5 Premium":
                        {
                            await DownloadFile("Root Image", "https://s3.amazonaws.com/windroid/Devices/Sony/Xperia_Z5_Premium/Root.img", "./Data/Downloads/Root.img");
                        }
                        break;
                }
                var controller = await this.ShowProgressAsync("Pushing SuperSU...", "");
                controller.SetIndeterminate();
                if (await Task.Run(() => _device.PushFile("./Data/Downloads/SuperSU.zip", "/sdcard/SuperSU.zip").ToString() == "True"))
                {
                    Log.AddLogItem("SuperSU push successful.", "SONYROOT");
                    controller.SetTitle("Pushing DRM ZIP...");
                    if (await Task.Run(() => _device.PushFile("./Data/Downloads/DRMRestore.zip", "/sdcard/DRMRestore.zip").ToString() == "True"))
                    {
                        Log.AddLogItem("DRMRestore push successful.", "SONYROOT");
                        controller.SetTitle("Pushing Root.img...");
                        if (await Task.Run(() => _device.PushFile("./Data/Downloads/Root.img", "/sdcard/Root.img").ToString() == "True"))
                        {
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.RebootRecovery());
                            Log.AddLogItem("Rebooting device to recovery.", "ROOT");
                            await Task.Delay(5000);
                            await controller2.CloseAsync();
                            await this.ShowMessageAsync("Your Turn!", "Tap on 'Install' in the top left corner. Then, tap on 'Images' in the bottom right corner. Then, scroll until you find 'Root.img'. Tap on it, then choose 'Boot' at the top. Finally, swipe to confirm flash. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                            await this.ShowMessageAsync("Almost There!", "Now, go back to the main screen, and tap on 'Install' again. Then, scroll down until you find 'SuperSU.zip'. Finally, tap on it, then swipe to confirm flash. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                            await this.ShowMessageAsync("Last One!", "Now, go back to the main screen, and tap on 'Install' again. Then, scroll down until you find 'DRMRestore.zip'. Finally, tap on it, then swipe to confirm flash. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                            await this.ShowMessageAsync("Congratulations!", "You are all done with unlocking and rooting process! Hit the 'Reboot System' buttom in the bottom right corner in TWRP, and your device will boot up with root privileges.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                        else
                        {
                            await controller.CloseAsync();
                            Log.AddLogItem("Root.img push failed.", "ROOT");
                            await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push Root.img. Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        await controller.CloseAsync();
                        Log.AddLogItem("DRMRestore push failed.", "ROOT");
                        await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push DRMRestore.zip. Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
                else
                {
                    await controller.CloseAsync();
                    Log.AddLogItem("SuperSU push failed.", "ROOT");
                    await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push SuperSU.zip. Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative, mySettings2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void BootRoot()
        {
            try
            {
                var device = Settings.Default["Device"].ToString();
                if (device == "Nexus Player")
                {
                    await DownloadFile("Nexus Player Root Image", "https://s3.amazonaws.com/windroid/Devices/Google/Nexus_Player/Root.img", "./Root.img");
                }
                else if (device == "ZenFone Laser")
                {
                    await DownloadFile("ZenFone Laser Root Image", "https://s3.amazonaws.com/windroid/Devices/Asus/ZenFone_Laser/Root.img", "./Root.img");
                }
                Log.AddLogItem("Connected: Fastboot.", "ROOT");
                var controller2 = await this.ShowProgressAsync("Flashing Root.img...", "");
                controller2.SetIndeterminate();
                await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "boot", "./Root.img")));
                Log.AddLogItem("Flashing rooted boot image.", "ROOT");
                await Task.Run(() => UpdateDevice());
                await controller2.CloseAsync();
                var controller3 = await this.ShowProgressAsync("Rebooting Device...", "");
                controller3.SetIndeterminate();
                await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                await Task.Run(() => _android.WaitForDevice());
                await controller3.CloseAsync();
                await DownloadFile("SuperSU", "https://s3.amazonaws.com/windroid/Root/SuperSU.zip", "./Data/Downloads/SuperSU.zip");
                SuperSU();
                Log.AddLogItem("SuperSU pushing started.", "ROOT");

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.AddLogItem("Refresh button clicked.", "DEVICE");
                var controller = await this.ShowProgressAsync("Detecting Device...", "");
                controller.SetIndeterminate();
                await Task.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    controller9.SetIndeterminate();
                    await Task.Run(() => UpdateDevice());
                    if (await Task.Run(() => _android.HasConnectedDevices))
                    {
                        await controller9.CloseAsync();
                        if (_device.State.ToString() == "ONLINE")
                        {
                            Log.AddLogItem("Connected: Online.", "REBOOT");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await Task.Run(() => _android.WaitForDevice());
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.Reboot());
                            Log.AddLogItem("Device rebooting.", "REBOOT");
                            await Task.Run(() => _android.WaitForDevice());
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                        }
                        else
                        {
                            Log.AddLogItem("Wrong device state.", "REBOOT");
                            await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        await controller9.CloseAsync();
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device into recovery mode. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Recovery reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    controller9.SetIndeterminate();
                    await Task.Run(() => UpdateDevice());
                    if (await Task.Run(() => _android.HasConnectedDevices))
                    {
                        await controller9.CloseAsync();
                        if (_device.State.ToString() == "ONLINE")
                        {
                            Log.AddLogItem("Connected: Online.", "REBOOT");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await Task.Run(() => _android.WaitForDevice());
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.RebootRecovery());
                            Log.AddLogItem("Device rebooting to recovery.", "REBOOT");
                            await Task.Run(() => _android.WaitForDevice());
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                        }
                        else
                        {
                            Log.AddLogItem("Wrong device state.", "REBOOT");
                            await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        await controller9.CloseAsync();
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var result = await this.ShowMessageAsync("Ready To Reboot?", "This command will reboot your device into or from bootloader mode. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Bootloader reboot button clicked.", "REBOOT");
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    controller9.SetIndeterminate();
                    await Task.Run(() => UpdateDevice());
                    if (await Task.Run(() => _android.HasConnectedDevices))
                    {
                        await controller9.CloseAsync();
                        if (_device.State.ToString() == "ONLINE")
                        {
                            Log.AddLogItem("Connected: Online.", "REBOOT");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await Task.Run(() => _android.WaitForDevice());
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.RebootBootloader());
                            Log.AddLogItem("Device rebooting to bootloader.", "REBOOT");
                            await Task.Run(() => _android.WaitForDevice());
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                        }
                        else if (_device.State.ToString() == "FASTBOOT")
                        {
                            Log.AddLogItem("Connected: Fastboot.", "REBOOT");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await Task.Run(() => _android.WaitForDevice());
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.FastbootReboot());
                            Log.AddLogItem("Device rebooting from bootloader.", "REBOOT");
                            await Task.Run(() => _android.WaitForDevice());
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                        }
                        else
                        {
                            Log.AddLogItem("Wrong device state.", "REBOOT");
                            await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        await controller9.CloseAsync();
                        Log.AddLogItem("No device found.", "REBOOT");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot then flash a kernel of your choosing. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Log.AddLogItem("Flash kernel button clicked.", "KERNEL");
                    var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Kernel Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Kernel file selected.", "KERNEL");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "KERNEL");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                await Task.Run(() => _device.RebootBootloader());
                                Log.AddLogItem("Rebooting device to bootloader.", "KERNEL");
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FlashKernel(ofd.FileName);
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FlashKernel(ofd.FileName);
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "KERNEL");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "KERNEL");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void FlashKernel(string kernel)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Connected: Fastboot.", "KERNEL");
                var controller2 = await this.ShowProgressAsync("Flashing Kernel...", "");
                controller2.SetIndeterminate();
                await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "boot " + kernel)));
                Log.AddLogItem("Flashing kernel.", "KERNEL");
                await controller2.CloseAsync();
                var result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                Log.AddLogItem("Kernel flash successful.", "KERNEL");
                if (result2 == MessageDialogResult.Affirmative)
                {
                    var controller3 = await this.ShowProgressAsync("Rebooting Device...", "");
                    controller3.SetIndeterminate();
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                    Log.AddLogItem("Rebooting device.", "KERNEL");
                    await Task.Run(() => _android.WaitForDevice());
                    await Task.Run(() => UpdateDevice());
                    await controller3.CloseAsync();
                }
                await Task.Run(() => _android.Dispose());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var device = Settings.Default["Device"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Flash recovery button clicked.", "RECOVERY");
                var result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot then flash your chosen recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Recovery Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Recovery file selected.", "RECOVERY");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "RECOVERY");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller2.SetIndeterminate();
                                if (device == "One M9" || device == "One A9" || device == "Desire 626s")
                                {
                                    await Task.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("reboot download")));
                                    Log.AddLogItem("Rebooting device to download mode.", "RECOVERYNEWHTC");
                                }
                                else
                                {
                                    await Task.Run(() => _device.RebootBootloader());
                                    Log.AddLogItem("Rebooting device to bootloader.", "RECOVERY");
                                }
                                await Task.Run(() => _android.WaitForDevice());
                                await controller2.CloseAsync();
                                FlashRecovery(ofd.FileName, false);
                            }
                            else if (_device.State.ToString() == "FASTBOOT")
                            {
                                FlashRecovery(ofd.FileName, false);
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "RECOVERY");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Fastboot mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "RECOVERY");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void FlashRecovery(string recovery, bool setup)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Connected: Fastboot.", "RECOVERY");
                var controller2 = await this.ShowProgressAsync("Flashing Recovery...", "");
                controller2.SetIndeterminate();
                if (setup == true)
                {
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + "./Data/Recoveries/" + recovery)));
                }
                else if (setup == false)
                {
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + recovery)));
                }
                Log.AddLogItem("Flashing recovery.", "RECOVERY");
                await Task.Run(() => UpdateDevice());
                await controller2.CloseAsync();
                var result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                Log.AddLogItem("Recovery flash successful.", "RECOVERY");
                if (result2 == MessageDialogResult.Affirmative)
                {
                    var controller3 = await this.ShowProgressAsync("Rebooting Device...", "");
                    controller3.SetIndeterminate();
                    await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                    Log.AddLogItem("Rebooting device.", "RECOVERY");
                    await Task.Run(() => _android.WaitForDevice());
                    await Task.Run(() => UpdateDevice());
                    await controller3.CloseAsync();
                }
                await Task.Run(() => _android.Dispose());
                if (setup == true)
                {
                    await this.ShowMessageAsync("Next Step!", "You can now move on to Step 3, gaining root.", MessageDialogStyle.Affirmative, mySettings2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.WriteLine(" ");
                file.WriteLine(logBox.Text);
                file.Close();
            }
        }

        private async void sideloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Sideload buttom clicked.", "SIDELOAD");
                var result = await this.ShowMessageAsync("Ready To Sideload?", "This will push a ROM or other zip file to your device using ADB Sideload. You must have TWRP on your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "ZIP Files (*.zip*)|*.ZIP*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("Sideload file selected.", "SIDELOAD");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "SIDELOAD")
                            {
                                var controller = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient and do not disconnect your device.");
                                controller.SetIndeterminate();
                                await Task.Run(() => Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand("sideload", ofd.FileName)));
                                Log.AddLogItem("Sideloading file.", "SIDELOAD");
                                await Task.Run(() => UpdateDevice());
                                await Task.Run(() => _android.Dispose());
                                await controller.CloseAsync();
                                await this.ShowMessageAsync("Sideload Successful!", "The zip file should have been flashed. After it flashes, you can reboot your device, or continue using features in the recovery.", MessageDialogStyle.Affirmative, mySettings2);
                                Log.AddLogItem("Sideload successful.", "SIDELOAD");
                            }
                            else if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "SIDELOAD");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _device.RebootRecovery());
                                Log.AddLogItem("Rebooting device to recovery.", "SIDELOAD");
                                await Task.Delay(5000);
                                await controller2.CloseAsync();
                                await this.ShowMessageAsync("Activate Sideloading", "Once in TWRP, tap on 'Advanced' in the bottom left corner. Then, tap on ADB Sideload in the same spot. Finally, swipe to to start sideload. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative, mySettings2);
                                var controller8 = await this.ShowProgressAsync("Checking Device...", "");
                                controller8.SetIndeterminate();
                                await Task.Run(() => UpdateDevice());
                                await controller8.CloseAsync();
                                if (_device.State.ToString() == "SIDELOAD")
                                {
                                    var controller7 = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient, and do not disconnect your device.");
                                    controller7.SetIndeterminate();
                                    await Task.Run(() => Adb.ExecuteAdbCommand(Adb.FormAdbCommand("sideload", ofd.FileName)));
                                    Log.AddLogItem("Sideloading file.", "SIDELOAD");
                                    await Task.Run(() => UpdateDevice());
                                    await Task.Run(() => _android.Dispose());
                                    await controller7.CloseAsync();
                                    await this.ShowMessageAsync("Sideload Successful!", "The zip file should have been flashed. After it flashes, you can reboot your device, or continue using features in the recovery.", MessageDialogStyle.Affirmative, mySettings2);
                                    Log.AddLogItem("Sideload successful.", "SIDELOAD");
                                }
                                else
                                {
                                    Log.AddLogItem("No device found.", "SIDELOAD");
                                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                                }
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "SIDELOAD");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Sideload mode!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "SIDELOAD");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Push file button selected.", "PUSH");
                var result = await this.ShowMessageAsync("Ready To Push?", "This will push a file of your choosing to your device's storage. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "All Files (*.*)|*.*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("File selected.", "PUSH");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "PUSH");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Pushing File...", "Depending on the size of the file being pushed, this process can take awhile. Please be patient, and do not disconnect your device.");
                                controller2.SetIndeterminate();
                                Log.AddLogItem("Pushing file.", "PUSH");
                                if (await Task.Run(() => _device.PushFile(ofd.FileName, "/sdcard/" + ofd.SafeFileName).ToString() == "True"))
                                {
                                    await Task.Run(() => UpdateDevice());
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("Push Successful!", ofd.SafeFileName + " was successfully pushed!", MessageDialogStyle.Affirmative, mySettings2);
                                    Log.AddLogItem("File push successful.", "PUSH");
                                }
                                else
                                {
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("File Push Failed!", "An issue occurred while attempting to push " + ofd.SafeFileName, MessageDialogStyle.Affirmative, mySettings2);
                                    Log.AddLogItem("File push failed.", "PUSH");
                                }
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "PUSH");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "PUSH");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Install app button selected.", "APP");
                var result = await this.ShowMessageAsync("Ready To Install?", "This will install an app of your choosing to your device. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var ofd = new OpenFileDialog { CheckFileExists = true, Filter = "APK Files (*.apk*)|*.APK*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        Log.AddLogItem("APK file selected.", "APP");
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        controller9.SetIndeterminate();
                        await Task.Run(() => UpdateDevice());
                        if (await Task.Run(() => _android.HasConnectedDevices))
                        {
                            await controller9.CloseAsync();
                            if (_device.State.ToString() == "ONLINE")
                            {
                                Log.AddLogItem("Connected: Online.", "APP");
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                controller.SetIndeterminate();
                                await Task.Run(() => _android.WaitForDevice());
                                await controller.CloseAsync();
                                var controller2 = await this.ShowProgressAsync("Installing App...", "If the install process takes more than a minute, please check your device as you may have to accept a notice from Google.");
                                controller2.SetIndeterminate();
                                Log.AddLogItem("Installing app.", "APP");
                                if (await Task.Run(() => _device.InstallApk(ofd.FileName).ToString()) == "True")
                                {
                                    await Task.Run(() => UpdateDevice());
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("Install Successful!", ofd.SafeFileName + " was successfully installed!", MessageDialogStyle.Affirmative, mySettings2);
                                    Log.AddLogItem("App install successful.", "APP");
                                }
                                else
                                {
                                    await Task.Run(() => _android.Dispose());
                                    await controller2.CloseAsync();
                                    await this.ShowMessageAsync("App Install Failed!", "An issue occurred while attempting to install " + ofd.SafeFileName, MessageDialogStyle.Affirmative, mySettings2);
                                    Log.AddLogItem("App install failed.", "APP");
                                }
                            }
                            else
                            {
                                Log.AddLogItem("Wrong device state.", "APP");
                                await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is in Android!", MessageDialogStyle.Affirmative, mySettings2);
                            }
                        }
                        else
                        {
                            await controller9.CloseAsync();
                            Log.AddLogItem("No device found.", "APP");
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var manufacturer = Settings.Default["Manufacturer"].ToString();

                var dictionary = new ResourceDictionary();
                dictionary.Source = new Uri("pack://application:,,,/MaterialDesignThemes.MahApps;component/Themes/MaterialDesignTheme.MahApps.Dialogs.xaml");

                var mySettings = new MetroDialogSettings
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                var mySettings2 = new MetroDialogSettings
                {
                    SuppressDefaultResources = true,
                    CustomResourceDictionary = dictionary
                };

                Log.AddLogItem("Relock bootloader button clicked.", "RELOCK");
                var result = await this.ShowMessageAsync("Ready To Relock?", "This will relock your device's bootloader. You will lose root and custom recovery. This is not guarenteed to work on every device. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    controller9.SetIndeterminate();
                    await Task.Run(() => UpdateDevice());
                    if (await Task.Run(() => _android.HasConnectedDevices))
                    {
                        await controller9.CloseAsync();
                        if (_device.State.ToString() == "ONLINE")
                        {
                            Log.AddLogItem("Connected: Online.", "RELOCK");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await Task.Run(() => _android.WaitForDevice());
                            await controller.CloseAsync();
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => _device.RebootBootloader());
                            Log.AddLogItem("Rebooting device to bootloader.", "RELOCK");
                            await Task.Run(() => _android.WaitForDevice());
                            await controller2.CloseAsync();
                            var controller3 = await this.ShowProgressAsync("Relocking Device...", "");
                            controller3.SetIndeterminate();
                            await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                            Log.AddLogItem("Relocking bootloader.", "RELOCK");
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller3.CloseAsync();
                            Log.AddLogItem("Bootloader relock successful.", "RELOCK");
                            await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                        else if (_device.State.ToString() == "FASTBOOT")
                        {
                            Log.AddLogItem("Connected: Fastboot.", "RELOCK");
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            controller.SetIndeterminate();
                            await controller.CloseAsync();
                            await Task.Run(() => _android.WaitForDevice());
                            var controller2 = await this.ShowProgressAsync("Relocking Device...", "");
                            controller2.SetIndeterminate();
                            await Task.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                            Log.AddLogItem("Relocking bootloader.", "RELOCK");
                            await Task.Run(() => UpdateDevice());
                            await Task.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                            Log.AddLogItem("Bootloader relock successful.", "RELOCK");
                            await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.", MessageDialogStyle.Affirmative, mySettings2);
                        }
                        else
                        {
                            Log.AddLogItem("Wrong device state.", "RELOCK");
                            await this.ShowMessageAsync("Wrong Device State!", "Please ensure that your device is either in Android or Sideload mode!", MessageDialogStyle.Affirmative, mySettings2);
                        }
                    }
                    else
                    {
                        await controller9.CloseAsync();
                        Log.AddLogItem("No device found.", "RELOCK");
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.", MessageDialogStyle.Affirmative, mySettings2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void recoveriesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://drive.google.com/folderview?id=0BzIE430dYN2CZDdXejgzWUdKRzQ&usp=sharing");
                Log.AddLogItem("Recoveries button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void sourceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/Rapscallion16/WinDroid_Toolkit");
                Log.AddLogItem("Source button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void twitterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://twitter.com/DylanDoppelt");
                Log.AddLogItem("Twitter button clicked.", "ABOUT");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error has occured! A log file has been placed in the Logs folder. Please report this error, with the log file, in the toolkit thread on XDA.",
                    "Houston, we have a problem!", MessageBoxButton.OK, MessageBoxImage.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
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
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }
    }
}

//Dylan Doppelt (Rapscallion16)