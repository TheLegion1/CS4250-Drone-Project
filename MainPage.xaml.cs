using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Sockets;
using Windows.Gaming.Input;
using System.Numerics;
using Windows.UI.Core;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Text;
using Windows.ApplicationModel;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DriverStationMk3
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        float leftY = 0;
        float rightY = 0;
        bool enabled = false;
        //start of net code
        // The port number for the remote device.  
        private const int port = 11000;
        // The response from the remote device.  
        private static String serverResponse = String.Empty;

        //end of net code
        private Gamepad _Gamepad = null;
        public MainPage()
        {
            this.InitializeComponent();
            Uri url = new Uri("http://192.168.0.122:8080/javascript_simple.html");
            webPage.Navigate(url);
            Package pkg = Package.Current;
            PackageId pkgId = pkg.Id;
            PackageVersion pkgVer = pkgId.Version;
            string versionNumber = string.Format("{0}.{1}.{2}.{3}", pkgVer.Major, pkgVer.Minor, pkgVer.Build, pkgVer.Revision);
            ver_txt.Text = "DriverStation Ver: " + versionNumber;
            aBtn.Opacity = 0;
            bBtn.Opacity = 0;
            xBtn.Opacity = 0;
            yBtn.Opacity = 0;
            dUp.Opacity = 0;
            dDown.Opacity = 0;
            dLeft.Opacity = 0;
            dRight.Opacity = 0;
            
        }

        private void WebView_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        private async void Cnt_btn_Click(object sender, RoutedEventArgs e)
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
        
                    while (true)
                    {
                        await Dispatcher.RunAsync(
                                   CoreDispatcherPriority.Normal,() =>
                                   {
                                       if (_Gamepad == null)
                                       {
                                           return;
                                       }

                                       // Get the current state
                                       var reading = _Gamepad.GetCurrentReading();

                                      // tbLeftTrigger.Text = reading.LeftTrigger.ToString();
                                      // tbRightTrigger.Text = reading.RightTrigger.ToString();
                                      // tbLeftThumbstickX.Text = reading.LeftThumbstickX.ToString();
                                     //  tbLeftThumbstickY.Text = reading.LeftThumbstickY.ToString();
                                     //  tbRightThumbstickX.Text = reading.RightThumbstickX.ToString();
                                     //  tbRightThumbstickY.Text = reading.RightThumbstickY.ToString();
                                     //  tbButtons.Text = string.Empty;
                                       string buttons = string.Empty;
                                       aBtn.Opacity = 0;
                                       bBtn.Opacity = 0;
                                       xBtn.Opacity = 0;
                                       yBtn.Opacity = 0;
                                       dUp.Opacity = 0;
                                       dDown.Opacity = 0;
                                       dLeft.Opacity = 0;
                                       dRight.Opacity = 0;

                                       //triggers
                                       lTrigger.Value = reading.LeftTrigger * 100;
                                       rTrigger.Value = reading.RightTrigger * 100;
                                       if ((reading.Buttons & GamepadButtons.A) == GamepadButtons.A)
                                       {
                                           aBtn.Opacity = 100;
                                           buttons += "*A";

                                       }
                                       if ((reading.Buttons & GamepadButtons.B) == GamepadButtons.B)
                                       {
                                           bBtn.Opacity = 100;
                                           buttons += "*B";

                                       }
                                       if ((reading.Buttons & GamepadButtons.X) == GamepadButtons.X)
                                       {
                                           xBtn.Opacity = 100;
                                           buttons += "*X";

                                       }
                                       if ((reading.Buttons & GamepadButtons.Y) == GamepadButtons.Y)
                                       {
                                           yBtn.Opacity = 100;
                                           buttons += "*Y";
                                       }

                                      // tbButtons.Text += (reading.Buttons & GamepadButtons.LeftShoulder) == GamepadButtons.LeftShoulder ? "LeftShoulder " : "";
                                     ////  tbButtons.Text += (reading.Buttons & GamepadButtons.RightShoulder) == GamepadButtons.RightShoulder ? "RightShoulder " : "";
                                     //  tbButtons.Text += (reading.Buttons & GamepadButtons.LeftThumbstick) == GamepadButtons.LeftThumbstick ? "LeftThumbstick " : "";
                                     //tbButtons.Text += (reading.Buttons & GamepadButtons.RightThumbstick) == GamepadButtons.RightThumbstick ? "RightThumbstick " : "";
                                       //tbButtons.Text += (reading.Buttons & GamepadButtons.DPadLeft) == GamepadButtons.DPadLeft ? "DPadLeft " : "";
                                       //tbButtons.Text += (reading.Buttons & GamepadButtons.DPadRight) == GamepadButtons.DPadRight ? "DPadRight " : "";
                                       //tbButtons.Text += (reading.Buttons & GamepadButtons.DPadUp) == GamepadButtons.DPadUp ? "DPadUp " : "";
                                       //tbButtons.Text += (reading.Buttons & GamepadButtons.DPadDown) == GamepadButtons.DPadDown ? "DPadDown " : "";
                                       if ((reading.Buttons & GamepadButtons.DPadLeft) == GamepadButtons.DPadLeft)
                                       {
                                           dLeft.Opacity = 100;
                                           buttons += "*D_LEFT";
                                       }
                                       if ((reading.Buttons & GamepadButtons.DPadUp) == GamepadButtons.DPadUp)
                                       {
                                           dUp.Opacity = 100;
                                           buttons += "*D_UP";
                                       }
                                       if ((reading.Buttons & GamepadButtons.DPadRight) == GamepadButtons.DPadRight)
                                       {
                                           dRight.Opacity = 100;
                                           buttons += "*D_RIGHT";
                                       }
                                       if ((reading.Buttons & GamepadButtons.DPadDown) == GamepadButtons.DPadDown)
                                       {
                                           dDown.Opacity = 100;
                                           buttons += "*D_DOWN";
                                       }
                                       if (enabled)
                                       {
                                           if (reading.LeftThumbstickY != leftY || reading.RightThumbstickY != rightY)
                                           {
                                               leftY = (float)reading.LeftThumbstickY;
                                               rightY = (float)reading.RightThumbstickY;
                                               string data = PackData(reading.LeftThumbstickX, reading.LeftThumbstickY, reading.RightThumbstickX, reading.RightThumbstickY, reading.RightTrigger, reading.LeftTrigger, buttons);
                                               StartClient(data);
                                           }
                                        

                                       }
                                   });


                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                    }//end while loop
        }

        private async void Gamepad_GamepadRemoved(object sender,
                                            Gamepad e)
        {
            _Gamepad = null;

            await Dispatcher.RunAsync(
                               CoreDispatcherPriority.Normal, () =>
                               {
                                   tbConnected.Text = "Controller removed";
                                   tbConnected.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
                               });
        }

        private async void Gamepad_GamepadAdded(object sender,
                                        Gamepad e)
        {
            _Gamepad = e;

            await Dispatcher.RunAsync(
                         CoreDispatcherPriority.Normal, () =>
                         {
                         tbConnected.Text = "Controller added";
                         tbConnected.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 0, 255, 0));
                         });
        }

        //netcode functions
        private async void StartClient(string data)
        {
            var hostName = new Windows.Networking.HostName("localhost"); // local machine
            //  IPAddress ipAddress = IPAddress.Parse("192.168.0.122"); //rpi

            // Create a TCP/IP socket.  



           
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // The name of the    


                // Connect to the remote endpoint. 
                using (var streamSocket = new Windows.Networking.Sockets.StreamSocket())
                {
                    if (lb1.Items.Count > 10)
                    {
                        lb1.Items.Clear();
                    }

                    lb1.Items.Add("connecting");
                    //new Windows.Networking.HostName("Legion-Surface")
                    //new Windows.Networking.HostName("raspberrypi")

                    await streamSocket.ConnectAsync(new Windows.Networking.HostName("192.168.0.122"), port.ToString());
                    // Send test data to the remote device.  
                    if (lb1.Items.Count > 10)
                    {
                        lb1.Items.Clear();
                    }
                    lb1.Items.Add("connected. Sending Data");
                    string request = data + "<EOF>";
                    using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                    {
                        using (var streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(request);
                            await streamWriter.FlushAsync();
                        }
                    }


                    // Receive the response from the remote device.  
                    // Receive(streamSocket);
                    //lb1.Items.Add("received the response: " + serverResponse);
                    streamSocket.Dispose();
                }
                
            
            }
            catch (Exception e)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(e.GetBaseException().HResult);
                this.lb1.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : e.Message);
            }
        }
        private async void Receive(Windows.Networking.Sockets.StreamSocket streamSocket)
        {
            string response;
            using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
            {
                using (StreamReader streamReader = new StreamReader(inputStream))
                {
                    response = await streamReader.ReadLineAsync();
                }
                serverResponse = response;
            }
        }

        public string PackData(double lx, double ly, double rx, double ry, double rt, double lt, string buttons)
        {
            //purpose of this function is to take all
            //the data we need to transmit and put it in 
            //a single string that will be decoded on the rpi
            string output = "";
            /*
             * <SOF>Left_stick|Right_stick|left_triger|right_trigger|enable/disable<EOF>
             * {X=<VALUE>, Y=<VALUE>}
             * */

            //shorten the lx,ly, rx, ry
            lx = (float)lx;
            if (lx < 0.2 && lx > 0) { lx = 0; }
            if (lx > -0.2 && lx < 0) { lx = 0; }
            ly = (float)ly;
            if (ly < 0.2 && ly > 0) ly = 0;
            if (ly > -0.2 && ly < 0) { ly = 0; }
            rx = (float)rx;
            if (rx < 0.2 && rx > 0) rx = 0;
            if (rx > -0.2 && rx < 0) { rx = 0; }
            ry = (float)ry;
            if (ry < 0.2 && ry > 0) ry = 0;
            if (ry > -0.2 && ry < 0) { ry = 0; }
            string delimiter = "|";
            output += "<SOF>";
            output += "{X=" + 1 + ",Y=" + ly.ToString() + "}";
            output += delimiter;
            output += "{X=" + 1 + ",Y=" + ry.ToString() + "}";
            output += delimiter;
            output += lt.ToString();
            output += delimiter;
            output += rt.ToString();
            output += delimiter;
            output += buttons;
            return output;
        }

        private void Lb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Enbl_btn_Click(object sender, RoutedEventArgs e)
        {
            enabled = !enabled;
            tbEnabled.Text = enabled.ToString();
            if (enabled)
            {
                tbEnabled.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 0, 255, 0));
                enbl_btn.Content = "Disable";
            }
            else {
                tbEnabled.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 255, 0, 0));
                enbl_btn.Content = "Enable";
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            webPage.Refresh();
        }
    }

  

}
