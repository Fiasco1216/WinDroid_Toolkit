using AutoUpdaterDotNET;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using RegawMOD.Android;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace WinDroid_Universal_Android_Toolkit
{
    public partial class MainWindow : MetroWindow
    {
        private AndroidController _android;
        private Device _device;

        public MainWindow()
        {
            InitializeComponent();
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

        private async void UpdateDevice()
        {
            _android = AndroidController.Instance;
            try
            {
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
                            });
                            break;

                        case "FASTBOOT":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Fastboot";
                                statusEllipse.Fill = Brushes.Blue;
                            });
                            break;

                        case "RECOVERY":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Recovery";
                                statusEllipse.Fill = Brushes.Purple;
                            });
                            break;

                        case "UNKNOWN":
                            this.Dispatcher.BeginInvoke((Action)delegate()
                            {
                                statusLabel.Content = "Unknown";
                                statusEllipse.Fill = Brushes.Gray;
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
                    });
                }
                await TaskEx.Run(() => _android.Dispose());
            }
            catch (Exception ex)
            {
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.Start("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/Update.xml");

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
                                var phoneDownload = new DownloadWindow();
                                WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Selector = "ADB";
                                phoneDownload.ShowDialog();
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
                file.Close();
            }

            if (Properties.Settings.Default["Device"].ToString() == "None")
            {
                var controller9 = await this.ShowProgressAsync("Finding Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
                PhoneTextBox.Text = "Please choose your phone!";
            }
            else
            {
                PhoneTextBox.Text = "Current Phone: " + Properties.Settings.Default["Device"].ToString();
                getTokenIDButton.IsEnabled = true;
                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                gainRootButton.IsEnabled = true;
                switch (Properties.Settings.Default["Device"].ToString())
                {
                    case "Butterfly":
                        {
                            recovery1Button.Content = "Flash TWRP (x920d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (x920e)";
                        }
                        break;

                    case "Desire 610":
                        {
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.Content = "Gain Root";
                        }
                        break;

                    case "Droid DNA":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "EVO 3D":
                        {
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "Moto E":
                        {
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G":
                        {
                            getTokenIDButton.Content = "Get Unlock Key";
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

                    case "Nexus S":
                        {
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "Nexus 4":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 5":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 6":
                        {
                            getTokenIDButton.IsEnabled = false;
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

                    case "Nexus 9":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 10":
                        {
                            getTokenIDButton.IsEnabled = false;
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
                            recovery1Button.Content = "Flash TWRP (Europe)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Boost)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Cricket)";
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

                    case "OnePlus One":
                        {
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;
                }
                var controller = await this.ShowProgressAsync("Finding Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller.CloseAsync();
            }
        }

        private void phoneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var phoneDownload = new DownloadWindow();
                gainSuperCIDButton.IsEnabled = false;
                gainSuperCIDButton.Content = "Gain SuperCID";
                getTokenIDButton.IsEnabled = true;
                unlockBootloaderButton.IsEnabled = true;
                recovery1Button.IsEnabled = true;
                recovery1Button.Content = "Flash TWRP";
                recovery2Button.IsEnabled = false;
                recovery2Button.Content = "Option Two";
                recovery3Button.IsEnabled = false;
                recovery3Button.Content = "Option Three";
                gainRootButton.IsEnabled = true;
                ListBoxItem device = ((sender as ListBox).SelectedItem as ListBoxItem);
                switch (device.Content.ToString())
                {
                    case "Amaze":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Amaze";
                        }
                        break;

                    case "Butterfly":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Butterfly";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (x920d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (x920e)";
                        }
                        break;

                    case "Butterfly S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Butterfly_S";
                        }
                        break;

                    case "Desire 200":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_200";
                        }
                        break;

                    case "Desire 300":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_300";
                        }
                        break;

                    case "Desire 500":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_500";
                        }
                        break;

                    case "Desire 510":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_510";
                        }
                        break;

                    case "Desire 601":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_601";
                        }
                        break;

                    case "Desire 610":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_610";
                            recovery1Button.IsEnabled = false;
                            recovery1Button.Content = "Option One";
                            gainRootButton.Content = "Gain Root";
                        }
                        break;

                    case "Desire 816":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_816";
                        }
                        break;

                    case "Desire 820":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_820";
                        }
                        break;

                    case "Desire C":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_C";
                        }
                        break;

                    case "Desire Eye":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_Eye";
                        }
                        break;

                    case "Desire HD":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_HD";
                        }
                        break;

                    case "Desire S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_S";
                        }
                        break;

                    case "Desire SV":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_SV";
                        }
                        break;

                    case "Desire V":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_V";
                        }
                        break;

                    case "Desire X":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Desire_X";
                        }
                        break;

                    case "Droid DNA":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Droid_DNA";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Droid Incredible":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Droid_Incredible";
                        }
                        break;

                    case "Droid Incredible S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Droid_Incredible_S";
                        }
                        break;

                    case "Droid Incredible 2":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Droid_Incredible_2";
                        }
                        break;

                    case "Droid Incredible 4G LTE":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Droid_Incredible_4G_LTE";
                        }
                        break;

                    case "EVO 3D":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "EVO_3D";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "EVO 4G":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "EVO_4G";
                        }
                        break;

                    case "EVO 4G LTE":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "EVO_4G_LTE";
                        }
                        break;

                    case "EVO Design":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "EVO_Design";
                        }
                        break;

                    case "EVO Shift 4G":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "EVO_Shift_4G";
                        }
                        break;

                    case "Explorer":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Explorer";
                        }
                        break;

                    case "First":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "First";
                        }
                        break;

                    case "Moto E":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Moto_E";
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "Moto G":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Moto_G";
                            getTokenIDButton.Content = "Get Unlock Key";
                        }
                        break;

                    case "myTouch 4G Slide":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "myTouch_4G_Slide";
                        }
                        break;

                    case "Galaxy Nexus":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Galaxy_Nexus";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.ThreeRecoveries = true;
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Sprint)";
                        }
                        break;

                    case "Nexus S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_S";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (3G)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (4G)";
                        }
                        break;

                    case "Nexus 4":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_4";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 5":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_5";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 6":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_6";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 7 (2012)":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_7_2012";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (3G)";
                        }
                        break;

                    case "Nexus 7 (2013)":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_7_2013";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            getTokenIDButton.IsEnabled = false;
                            recovery1Button.Content = "Flash TWRP (WiFi)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (LTE)";
                        }
                        break;

                    case "Nexus 9":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_9";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Nexus 10":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Nexus_10";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "One E8":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_E8";
                        }
                        break;

                    case "One J":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_J";
                        }
                        break;

                    case "One M7":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_M7";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.ThreeRecoveries = true;
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
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_M7_Dual_SIM";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.ThreeRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (802d)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (802t)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (802w)";
                        }
                        break;

                    case "One M8":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_M8";
                            gainSuperCIDButton.IsEnabled = true;
                            gainSuperCIDButton.Content = "Verizon M8 Only";
                        }
                        break;

                    case "One Max":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_Max";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (Sprint)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Verizon)";
                        }
                        break;

                    case "One Mini":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_Mini";
                        }
                        break;

                    case "One Mini 2":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_Mini_2";
                        }
                        break;

                    case "One S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_S";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (S4)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (S3)";
                        }
                        break;

                    case "One SV":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_SV";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.ThreeRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (Europe)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (Boost)";
                            recovery3Button.IsEnabled = true;
                            recovery3Button.Content = "Flash TWRP (Cricket)";
                        }
                        break;

                    case "One V":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_V";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (GSM)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (CDMA)";
                        }
                        break;

                    case "One VX":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_VX";
                            gainSuperCIDButton.IsEnabled = false;
                        }
                        break;

                    case "One X":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_X";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            gainSuperCIDButton.Content = "AT&T One X Only";
                            gainSuperCIDButton.IsEnabled = true;
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "One X+":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "One_X_Plus";
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.TwoRecoveries = true;
                            recovery1Button.Content = "Flash TWRP (Global)";
                            recovery2Button.IsEnabled = true;
                            recovery2Button.Content = "Flash TWRP (AT&T)";
                        }
                        break;

                    case "OnePlus One":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "OnePlus_One";
                            getTokenIDButton.IsEnabled = false;
                        }
                        break;

                    case "Rezound":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Rezound";
                        }
                        break;

                    case "Sensation":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Sensation";
                        }
                        break;

                    case "Sensation XL":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Sensation_XL";
                        }
                        break;

                    case "Vivid":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Vivid";
                        }
                        break;

                    case "Wildfire":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Wildfire";
                        }
                        break;

                    case "Wildfire S":
                        {
                            WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Device = "Wildfire_S";
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
                Properties.Settings.Default["Device"] = device.Content.ToString();
                Properties.Settings.Default.Save();
                ((Flyout)Flyouts.Items[0]).IsOpen = false;
                PhoneTextBox.Text = "Current Device: " + device.Content.ToString();
                phoneDownload.ShowDialog();
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

        private void TogglePhoneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ((Flyout)Flyouts.Items[0]).IsOpen = !((Flyout)Flyouts.Items[0]).IsOpen;
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
                            if (File.Exists("./run.bat"))
                            {
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/run.bat");
                            }
                            else
                            {
                                var phoneDownload = new DownloadWindow();
                                WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Selector = "One X AT&T";
                                phoneDownload.ShowDialog();
                            }
                        }
                        else
                        {
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
                file.Close();
            }
        }

        private async void getTokenIDButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Getting Token ID...");
                        using (StreamWriter sw = File.CreateText("./Data/token.txt"))
                        {
                            string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_identifier_token")));
                            string rawToken = GetStringBetween(rawReturn, "< Please cut following message >\r\n",
                                "\r\nOKAY");
                            string cleanedToken = rawToken.Replace("(bootloader) ", "");
                            sw.WriteLine(cleanedToken);
                            sw.WriteLine(" ");
                            sw.WriteLine(
                                "PLEASE COPY EVERYTHING ABOVE THIS LINE!");
                            sw.WriteLine(" ");
                            sw.WriteLine("NEXT, SIGN IN TO YOUR HTC DEV ACCOUNT ON THE WEBPAGE THAT JUST OPENED!");
                            sw.WriteLine(
                                "IF YOU DO NOT HAVE ONE, CREATE AND ACTIVATE AN ACCOUNT WITH A VALID EMAIL ADDRESS THEN COME BACK TO THIS LINK:");
                            sw.WriteLine("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            sw.WriteLine(
                                "THEN, PASTE THE TOKEN ID YOU JUST COPIED AT THE BOTTOM OF THE HTCDEV WEBPAGE!");
                            sw.WriteLine("HIT SUBMIT, AND WAIT FOR THE EMAIL WITH THE UNLOCK BINARY FILE!");
                            sw.WriteLine(" ");
                            sw.WriteLine(
                                "ONCE YOU HAVE RECEIVED THE UNLOCK FILE IN YOUR EMAIL, YOU CAN CONTINUE ON TO THE NEXT STEP!");
                            sw.WriteLine("THIS FILE IS SAVED AS token.txt WITHIN THE DATA FOLDER IF NEEDED IN THE FUTURE!");
                        }
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await TaskEx.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                            Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                            await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                        }
                        else
                        {
                            Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                            await TaskEx.Run(() => UpdateDevice());
                            await TaskEx.Run(() => _android.Dispose());
                            await this.ShowMessageAsync("Next Step!", "Once you have recieved the unlock file from HTC, you can move on to the next step, unlocking your bootloader!",
                        MessageDialogStyle.Affirmative);
                        }
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Getting Token ID...");
                        using (StreamWriter sw = File.CreateText("./Data/token.txt"))
                        {
                            string rawReturn = await TaskEx.Run(() => Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, "oem", "get_identifier_token")));
                            string rawToken = GetStringBetween(rawReturn, "< Please cut following message >\r\n",
                                "\r\nOKAY");
                            string cleanedToken = rawToken.Replace("(bootloader) ", "");
                            sw.WriteLine(cleanedToken);
                            sw.WriteLine(" ");
                            sw.WriteLine(
                                "PLEASE COPY EVERYTHING ABOVE THIS LINE!");
                            sw.WriteLine(" ");
                            sw.WriteLine("NEXT, SIGN IN TO YOUR HTC DEV ACCOUNT ON THE WEBPAGE THAT JUST OPENED!");
                            sw.WriteLine(
                                "IF YOU DO NOT HAVE ONE, CREATE AND ACTIVATE AN ACCOUNT WITH A VALID EMAIL ADDRESS THEN COME BACK TO THIS LINK:");
                            sw.WriteLine("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            sw.WriteLine(
                                "THEN, PASTE THE TOKEN ID YOU JUST COPIED AT THE BOTTOM OF THE HTCDEV WEBPAGE!");
                            sw.WriteLine("HIT SUBMIT, AND WAIT FOR THE EMAIL WITH THE UNLOCK BINARY FILE!");
                            sw.WriteLine(" ");
                            sw.WriteLine(
                                "ONCE YOU HAVE RECEIVED THE UNLOCK FILE IN YOUR EMAIL, YOU CAN CONTINUE ON TO THE NEXT STEP!");
                            sw.WriteLine("THIS FILE IS SAVED AS token.txt WITHIN THE DATA FOLDER IF NEEDED IN THE FUTURE!");
                        }
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Token ID Retrieval Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await TaskEx.Run(() => _android.Dispose());
                            await controller2.CloseAsync();
                            Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                        }
                        else
                        {
                            Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                            Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/token.txt");
                            await TaskEx.Run(() => _android.Dispose());
                            await TaskEx.Run(() => UpdateDevice());
                        }
                    }
                    else
                    {
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
                file.Close();
            }
        }

        private void unlockBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string device = Properties.Settings.Default["Device"].ToString();
                if (device == "Droid DNA")
                {
                    DroidDNAUnlock();
                }
                else if (device == "Droid Incredible 4G LTE")
                {
                    DroidIncredible4GLTEUnlock();
                }
                else if (device == "OnePlus One" || device == "Galaxy Nexus" || device == "Nexus S" || device == "Nexus 4" || device == "Nexus 5" || device == "Nexus 6" || device == "Nexus 7 (2012)" || device == "Nexus 7 (2013)" || device == "Nexus 9" || device == "Nexus 10")
                {
                    AOSPDeviceUnlock();
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
                file.Close();
            }
        }

        private async void DroidDNAUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            if (statusLabel.Content.ToString() == "Online")
            {
                MessageDialogResult result = await this.ShowMessageAsync("Verizon Sucks!", "Your Droid DNA cannot be unlocked through this toolkit. However, you can utilize another method to unlock your phone. Would you like to try this alternate method?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    Process.Start("http://theroot.ninja/");
                }
            }
            else
            {
                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
            }
        }

        private async void DroidIncredible4GLTEUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            if (statusLabel.Content.ToString() == "Online")
            {
                MessageDialogResult result = await this.ShowMessageAsync("Verizon Sucks!", "Your phone cannot be unlocked through this toolkit. However, you can utilize another prograsm to unlock your phone. Would you like to download it now?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    if (File.Exists("./RunMe.bat"))
                    {
                        Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/RunMe.bat");
                        MessageBox.Show(
                            @"You only need to complete Steps 1 and 2. After that, you can continue on to the next steps in the toolkit.",
                            @"Just a heads up.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var phoneDownload = new DownloadWindow();
                        WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Selector = "Droid Incredible 4G LTE";
                        phoneDownload.ShowDialog();
                    }
                }
                else
                {
                    Process.Start("http://forum.xda-developers.com/showthread.php?t=2214653");
                }
            }
            else
            {
                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
            }
        }

        private async void AOSPDeviceUnlock()
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock!", "This will unlock your bootloader and completely wipe your phone. Please ensure that you have backed up all necessary files. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Unlocking Bootloader...");
                    controller.SetMessage("Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button confirm. Once you've done this, click 'Ok'.");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    await TaskEx.Run(() => UpdateDevice());
                    await TaskEx.Run(() => _android.Dispose());
                    await controller.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you re-enable USB Debugging.", MessageDialogStyle.Affirmative);
                }
                else if (statusLabel.Content.ToString() == "Fastboot")
                {
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Unlocking Bootloader...");
                    controller.SetMessage("Your device will now display a screen asking you about unlocking. Use your volume buttons to choose Yes, then press the power button confirm. Once you've done this, click 'Ok'.");
                    await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "unlock")));
                    await TaskEx.Run(() => UpdateDevice());
                    await TaskEx.Run(() => _android.Dispose());
                    await controller.CloseAsync();
                    await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                }
                else
                {
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

            MessageDialogResult result = await this.ShowMessageAsync("Ready To Unlock?", "This will unlock your bootloader and completely wipe the data your device. You must have downloaded the unlock_code.bin file from HTC. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
            if (result == MessageDialogResult.Affirmative)
            {
                OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "HTC Unlock Files (*.bin*)|*.bin*", Multiselect = false };
                ofd.ShowDialog();
                if (File.Exists(ofd.FileName))
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing Unlock File...");
                        controller.SetMessage("Your device will now display a screen asking you about unlocking. Please read it carefully. Use your volume buttons to choose Yes, then press the power button confirm the unlock.");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlocktoken " + ofd.FileName)));
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot. You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging when your device finishes rebooting.", MessageDialogStyle.Affirmative);
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing Unlock File...");
                        controller.SetMessage("Your device will now display a screen asking you about unlocking. Press the volume buttons up to choose Yes, then press the power button confirm. Your device will now reboot.");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "unlocktoken " + ofd.FileName)));
                        controller.SetTitle("Rebooting Device");
                        controller.SetMessage("");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Next Step!", "Your bootloader is now unlocked, and your device will reboot! You can now move on to Step 2, flashing a recovery. Please ensure you enable USB Debugging on you device.", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
        }

        private void firstRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.Selector = "Recovery1.img";
                FlashRecovery();
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

        private void secondRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.Selector = "Recovery2.img";
                FlashRecovery();
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

        private void thirdRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.Selector = "Recovery3.img";
                FlashRecovery();
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

        private async void FlashRecovery()
        {
            if (File.Exists("./Data/Recoveries/" + Settings.Selector))
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash the TWRP recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing TWRP...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + "./Data/Recoveries/" + Settings.Selector)));
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await controller2.CloseAsync();
                        }
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "You can now move on to Step 3, gaining root.", MessageDialogStyle.Affirmative);
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Flashing TWRP...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + "./Data/Recoveries/" + Settings.Selector)));
                        await TaskEx.Run(() => UpdateDevice());
                        await controller.CloseAsync();
                        MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                            await TaskEx.Run(() => _android.WaitForDevice());
                            await TaskEx.Run(() => UpdateDevice());
                            await controller2.CloseAsync();
                        }
                        await TaskEx.Run(() => _android.Dispose());
                        await this.ShowMessageAsync("Next Step!", "You can now move on to Step 3, gaining root.", MessageDialogStyle.Affirmative);
                    }
                    else
                    {
                        await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                    }
                }
            }
            else
            {
                MessageBox.Show(@"This recovery appears to be missing from the Data folder!", @"Where dey at doe?", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageDialogResult result = await this.ShowMessageAsync("AT&T Sucks!", "Due to restrictions put in place by AT&T, you must flash a recovery and gain root through a special program. Would you like to download it now?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            if (File.Exists("./Data/Installers/Desire_610.exe"))
                            {
                                Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + "/Data/Installers/Desire_610.exe");
                            }
                            else
                            {
                                var phoneDownload = new DownloadWindow();
                                WinDroid_Universal_Android_Toolkit.DownloadWindow.Settings.Selector = "Desire 610";
                                phoneDownload.ShowDialog();
                            }
                        }
                        else
                        {
                            await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                        MessageDialogStyle.Affirmative);
                        }
                    }
                }
                else
                {
                    MessageDialogResult result = await this.ShowMessageAsync("Ready To Root?", "This command will download SuperSU, reboot you device into recovery, then allow you to flash SuperSU to permanently gain root. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            if (_device.HasRoot.ToString() == "True")
                            {
                                MessageDialogResult result2 = await this.ShowMessageAsync("Already Rooted!", "It appears that your device is already rooted! Would you still like to continue with flashing SuperSU?",
                                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                                if (result2 == MessageDialogResult.Affirmative)
                                {
                                    if (File.Exists("./Data/Installers/SuperSU.zip"))
                                    {
                                        Dispatcher.Invoke(new Action(SuperSU));
                                    }
                                    else
                                    {
                                        var controller = await this.ShowProgressAsync("Downloading SuperSU...", "");
                                        var client = new WebClient();
                                        client.DownloadFileCompleted += (client_DownloadFileCompleted);
                                        await TaskEx.Run(() => client.DownloadFileAsync(new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/SuperSU.zip"), "./Data/Installers/SuperSU.zip"));
                                        await controller.CloseAsync();
                                    }
                                }
                            }
                            else
                            {
                                if (File.Exists("./Data/Installers/SuperSU.zip"))
                                {
                                    Dispatcher.Invoke(new Action(SuperSU));
                                }
                                else
                                {
                                    var controller = await this.ShowProgressAsync("Downloading SuperSU...", "");
                                    var client = new WebClient();
                                    client.DownloadFileCompleted += (client_DownloadFileCompleted);
                                    await TaskEx.Run(() => client.DownloadFileAsync(new Uri("https://s.basketbuild.com/dl/devs?dl=WindyCityRockr/WinDroid_Universal_Android_Toolkit/SuperSU.zip"), "./Data/Installers/SuperSU.zip"));
                                    await controller.CloseAsync();
                                }
                            }
                        }
                        else
                        {
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
                file.Close();
            }
        }

        private async void SuperSU()
        {
            var controller = await this.ShowProgressAsync("Pushing SuperSU...", "");
            if (await TaskEx.Run(() => _device.PushFile("./Data/Installers/SuperSU.zip", "/sdcard/SuperSU.zip").ToString() == "True"))
            {
                controller.SetTitle("Rebooting Device...");
                await TaskEx.Run(() => _device.RebootRecovery());
                await controller.CloseAsync();
                await this.ShowMessageAsync("Rebooting Device...", "Once in TWRP, tap on 'Install' in the top left corner. Then, scroll until you find 'SuperSU.zip'. Finally, tap on it, then swipe to to confirm flash. Once you have finished, click 'Ok'.",
                    MessageDialogStyle.Affirmative);
                await this.ShowMessageAsync("Congratulations!", "You are all done with unlocking and rooting process! Hit the 'Reboot System' buttom in the bottom right corner in TWRP, and your device will boot up with root privileges. :D",
                    MessageDialogStyle.Affirmative);
            }
            else
            {
                await this.ShowMessageAsync("Push Failed!", "An error occured while attempting to push SuperSU.zip. Please restart the toolkit and try again in a few minutes.",
                                MessageDialogStyle.Affirmative);
            }
        }

        private async void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    await this.ShowMessageAsync("Download Failed!", "An error occured while downloading SuperSU.zip. Please restart the toolkit, check your internet connection and try again in a few minutes.",
                                MessageDialogStyle.Affirmative);
                }
                else
                {
                    Dispatcher.Invoke(new Action(SuperSU));
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

        private async void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var controller = await this.ShowProgressAsync("Finding Device...", "");
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
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.Reboot());
                        await TaskEx.Run(() => _android.WaitForDevice());
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                    }
                    else
                    {
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
                file.Close();
            }
        }

        private async void rebootRecoveryButton_Click(object sender, RoutedEventArgs e)
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
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootRecovery());
                    await TaskEx.Run(() => _android.WaitForDevice());
                    await TaskEx.Run(() => UpdateDevice());
                    await TaskEx.Run(() => _android.Dispose());
                    await controller.CloseAsync();
                }
                else
                {
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void rebootBootloaderButton_Click(object sender, RoutedEventArgs e)
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
                var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                await TaskEx.Run(() => UpdateDevice());
                await controller9.CloseAsync();
                if (statusLabel.Content.ToString() == "Online")
                {
                    var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                    await TaskEx.Run(() => _android.WaitForDevice());
                    controller.SetTitle("Rebooting Device...");
                    await TaskEx.Run(() => _device.RebootBootloader());
                    await TaskEx.Run(() => _android.WaitForDevice());
                    await TaskEx.Run(() => UpdateDevice());
                    await TaskEx.Run(() => _android.Dispose());
                    await controller.CloseAsync();
                }
                else
                {
                    await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
                }
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
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Kernel Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Rebooting Device...");
                            await TaskEx.Run(() => _device.RebootBootloader());
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Kernel...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "boot " + ofd.FileName)));
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else if (statusLabel.Content.ToString() == "Fastboot")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Kernel...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "boot " + ofd.FileName)));
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else
                        {
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
                file.Close();
            }
        }

        private async void flashRecoveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Yes",
                    NegativeButtonText = "No",
                };

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Flash?", "This will reboot your device into fastboot mode then flash your chosen recovery. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "Recovery Files (*.img*)|*.IMG*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Rebooting Device...");
                            await TaskEx.Run(() => _device.RebootBootloader());
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Recovery...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + ofd.FileName)));
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else if (statusLabel.Content.ToString() == "Fastboot")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Flashing Recovery...");
                            await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "flash", "recovery " + ofd.FileName)));
                            await TaskEx.Run(() => UpdateDevice());
                            await controller.CloseAsync();
                            MessageDialogResult result2 = await this.ShowMessageAsync("Flash Successful!", "Would you like to reboot now?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                            if (result2 == MessageDialogResult.Affirmative)
                            {
                                var controller2 = await this.ShowProgressAsync("Rebooting Device...", "");
                                await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "reboot")));
                                await TaskEx.Run(() => _android.WaitForDevice());
                                await TaskEx.Run(() => UpdateDevice());
                                await controller2.CloseAsync();
                            }
                            await TaskEx.Run(() => _android.Dispose());
                        }
                        else
                        {
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

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Sideload?", "This will push a ROM or other zip file of your choosing to your device using ADB Sideload. You must have TWRP flashed on your device. Are you ready to continue?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "ZIP Files (*.zip*)|*.ZIP*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        MessageDialogResult result2 = await this.ShowMessageAsync("Ready To Sideload?", "Is your device currently in ADB Sideload mode?",
                            MessageDialogStyle.AffirmativeAndNegative, mySettings);
                        if (result2 == MessageDialogResult.Affirmative)
                        {
                            var controller = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient, and do not disconnect your device.");
                            await TaskEx.Run(() => Adb.ExecuteAdbCommand(Adb.FormAdbCommand("sideload", ofd.FileName)));
                            await TaskEx.Run(() => UpdateDevice());
                            await TaskEx.Run(() => _android.Dispose());
                            await controller.CloseAsync();
                            await this.ShowMessageAsync("Sideload Successful!", "The zip file should be flashing now. After it flashes, you can reboot your phone, or continue using features in the recovery.", MessageDialogStyle.Affirmative);
                        }
                        else
                        {
                            var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                            await TaskEx.Run(() => UpdateDevice());
                            await controller9.CloseAsync();
                            if (statusLabel.Content.ToString() == "Online")
                            {
                                var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                                await TaskEx.Run(() => _android.WaitForDevice());
                                controller.SetTitle("Rebooting Device...");
                                await TaskEx.Run(() => _device.RebootRecovery());
                                await controller.CloseAsync();
                                await this.ShowMessageAsync("Activate Sideloading", "Once in TWRP, tap on 'Advanced' in the bottom left corner. Then, tap on ADB Sideload in the same spot. Finally, swipe to to start sideload. Once you have finished, click 'Ok'.", MessageDialogStyle.Affirmative);
                                var controller2 = await this.ShowProgressAsync("Sideloading ZIP...", "Depending on the size of the zip file being sideloaded, this process can take awhile. Please be patient, and do not disconnect your device.");
                                await TaskEx.Run(() => Adb.ExecuteAdbCommand(Adb.FormAdbCommand("sideload", ofd.FileName)));
                                await TaskEx.Run(() => UpdateDevice());
                                await TaskEx.Run(() => _android.Dispose());
                                await controller2.CloseAsync();
                                await this.ShowMessageAsync("Sideload Successful!", "The zip file should be flashing now. After it flashes, you can reboot your phone, or continue using features in the recovery.", MessageDialogStyle.Affirmative);
                            }
                            else
                            {
                                await this.ShowMessageAsync("No Device Found!", "Please ensure that USB Debugging is enabled, your device is plugged in correctly, and you correctly installed the ADB Drivers. If this is a persistent issue, please reply in the XDA thread.",
                                            MessageDialogStyle.Affirmative);
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
                string fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
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

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Push?", "This will push a file your choosing to your device's storage. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "All Files (*.*)|*.*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "Depending on the size of the file being pushed, this process can take awhile. Please be patient, and do not disconnect your device.");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Pushing File...");
                            if (await TaskEx.Run(() => _device.PushFile(ofd.FileName, "/sdcard/" + ofd.SafeFileName).ToString() == "True"))
                            {
                                await TaskEx.Run(() => UpdateDevice());
                                await TaskEx.Run(() => _android.Dispose());
                                await controller.CloseAsync();
                                await this.ShowMessageAsync("Push Successful!", ofd.SafeFileName + " was successfully pushed!", MessageDialogStyle.Affirmative);
                            }
                            else
                            {
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("File Push Failed!", "An issue occurred while attempting to push " + ofd.SafeFileName + " Please restart the toolkit and try again in a few minutes.", MessageDialogStyle.Affirmative);
                            }
                        }
                    }
                    else
                    {
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

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Install?", "This will install an app of your choosing to your device. Are you ready to continue?",
                       MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Filter = "APK Files (*.apk*)|*.APK*", Multiselect = false };
                    ofd.ShowDialog();
                    if (File.Exists(ofd.FileName))
                    {
                        var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                        await TaskEx.Run(() => UpdateDevice());
                        await controller9.CloseAsync();
                        if (statusLabel.Content.ToString() == "Online")
                        {
                            var controller = await this.ShowProgressAsync("Waiting For Device...", "If the install process takes more than a minute, please check your device as you may have to accept a notice from Google.");
                            await TaskEx.Run(() => _android.WaitForDevice());
                            controller.SetTitle("Installing App...");
                            if (await TaskEx.Run(() => _device.InstallApk(ofd.FileName).ToString()) == "True")
                            {
                                await TaskEx.Run(() => UpdateDevice());
                                await controller.CloseAsync();
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("Install Successful!", ofd.SafeFileName + " was successfully installed!", MessageDialogStyle.Affirmative);
                            }
                            else
                            {
                                await TaskEx.Run(() => _android.Dispose());
                                await this.ShowMessageAsync("App Install Failed!", "An issue occurred while attempting to install " + ofd.SafeFileName, MessageDialogStyle.Affirmative);
                            }
                        }
                    }
                    else
                    {
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

                MessageDialogResult result = await this.ShowMessageAsync("Ready To Relock?", "This will relock your device's bootloader. You will lose root and custom recovery. Are you ready to continue?",
                        MessageDialogStyle.AffirmativeAndNegative, mySettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    var controller9 = await this.ShowProgressAsync("Checking Device...", "");
                    await TaskEx.Run(() => UpdateDevice());
                    await controller9.CloseAsync();
                    if (statusLabel.Content.ToString() == "Online")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Rebooting Device...");
                        await TaskEx.Run(() => _device.RebootBootloader());
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Relocking Bootloader...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.",
                          MessageDialogStyle.Affirmative);
                    }
                    else if (statusLabel.Content.ToString() == "Fastboot")
                    {
                        var controller = await this.ShowProgressAsync("Waiting For Device...", "");
                        await TaskEx.Run(() => _android.WaitForDevice());
                        controller.SetTitle("Relocking Bootloader...");
                        await TaskEx.Run(() => Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, "oem", "lock")));
                        await TaskEx.Run(() => UpdateDevice());
                        await TaskEx.Run(() => _android.Dispose());
                        await controller.CloseAsync();
                        await this.ShowMessageAsync("Relock Successful!", "Your device will now reboot.",
                          MessageDialogStyle.Affirmative);
                    }
                    else
                    {
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
                file.Close();
            }
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/showpost.php?p=52041197&postcount=2");
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

        #region Nested type: Settings

        public static class Settings
        {
            public static string Selector = "";
        }

        #endregion Nested type: Settings
    }
}