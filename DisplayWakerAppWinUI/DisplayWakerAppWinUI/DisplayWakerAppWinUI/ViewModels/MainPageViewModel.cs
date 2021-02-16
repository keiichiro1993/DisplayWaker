using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DisplayWakerAppWinUI.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Win32
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MONITORPOWER = 0xF170;
        private const int MONITOR_TURNOFF = 2;

        [DllImport("user32.dll")]
        static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001;
        #endregion


        private List<string> _PortNames;
        public List<string> PortNames
        {
            get { return _PortNames; }
            set
            {
                _PortNames = value;
                RaisePropertyChanged();
            }
        }
        public string SelectedPortName { get; set; }

        private string _Signal;
        public string Signal
        {
            get { return _Signal; }
            set
            {
                _Signal = value;
                RaisePropertyChanged();
            }
        }

        private string _WatchButtonContent = "Start watching";
        public string WatchButtonContent
        {
            get { return _WatchButtonContent; }
            set
            {
                _WatchButtonContent = value;
                RaisePropertyChanged();
            }
        }


        public void Init()
        {
            RefreshPortNames();
        }

        public void TurnOffButton_Click(object sender, RoutedEventArgs e)
        {
            var handle = Process.GetCurrentProcess().MainWindowHandle;
            SendMessage(handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_TURNOFF);
        }


        private Task currentTask;
        private bool running;
        public async void WatchButton_Click(object sender, RoutedEventArgs e)
        {
            switch (WatchButtonContent)
            {
                case "Start watching":
                    if (string.IsNullOrEmpty(SelectedPortName))
                    {
                        //await new MessageDialog("Plz select the port with Motion Sensor.", "Port not selected").ShowAsync();
                    }
                    else
                    {
                        running = true;
                        currentTask = WatchHumanExistence(SelectedPortName);
                        WatchButtonContent = "Stop watching";
                    }
                    break;
                case "Stop watching":
                    if (currentTask != null && !currentTask.IsCompleted)
                    {
                        running = false;
                        await currentTask;
                    }
                    WatchButtonContent = "Start watching";
                    break;
            }
        }

        public void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshPortNames();
        }


        private void RefreshPortNames()
        {
            PortNames = new List<string>(SerialPort.GetPortNames());
        }

        private async Task WatchHumanExistence(string portName)
        {
            var serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.BaudRate = 500000;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Encoding = Encoding.UTF8;
            serialPort.WriteTimeout = 100000;
            try
            {
                serialPort.Open();
                while (running)
                {
                    await Task.Delay(500);
                    var message = "";
                    await Task.Run(() => message = serialPort.ReadLine());
                    Signal = message;

                    //TODO: check display status
                    if (message == "High\r")
                    {
                        mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
                    }
                }
            }
            catch (Exception ex)
            {
                //await new MessageDialog(ex.Message, "Error occurred").ShowAsync();
            }
            finally
            {
                serialPort.Close();
                Signal = "";
            }
        }
    }
}
