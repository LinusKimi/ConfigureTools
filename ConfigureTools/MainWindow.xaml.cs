using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Modbus;
using System.IO;
using System.IO.Ports;
using Modbus.Device;
using System.ComponentModel;

namespace ConfigureTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPort serialPort;
        public IModbusSerialMaster imodbusserialmaster;
        public BindingList<string> msglist;
        public MainWindow()
        {
            InitializeComponent();
            serialPort = new SerialPort();
            msglist = new BindingList<string> { ">> 启动成功！"};

            msgbox.ItemsSource = msglist;
            buteRaate.ItemsSource = new List<string>() { "115200"};
            buteRaate.SelectedIndex = 0;

            foreach (string com in System.IO.Ports.SerialPort.GetPortNames())//枚举所有可用的串口
            {
                uartname.Items.Add(com);
            }
            uartname.SelectedIndex = 0;

        }

        public void addmsg(string msg)
        {
            Dispatcher.Invoke(()=>
            {
                msglist.Add(msg);
                if (msglist.Count > 5)
                    msglist.RemoveAt(0);
            });
        }

        private void uartportcontrl_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)uartportcontrl.IsChecked)
            {
                try
                {
                    serialPort.PortName = uartname.SelectedItem.ToString();
                    serialPort.BaudRate = 115200;
                    serialPort.DataBits = 8;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Open();

                    imodbusserialmaster = ModbusSerialMaster.CreateAscii(serialPort);

                    uartname.IsEnabled = false;
                    buteRaate.IsEnabled = false;
                    addmsg($">> 打开串口成功！");
                    
                }
                catch (Exception ex)
                {
                    uartportcontrl.IsChecked = false;
                    uartname.IsEnabled = true;
                    buteRaate.IsEnabled = true;
                    addmsg($"ERROR：打开串口失败！{ex.ToString()}");
                }
            }
            else
            {
                try 
                {
                    serialPort.Close();
                    imodbusserialmaster.Dispose();
                    uartname.IsEnabled = true;
                    buteRaate.IsEnabled = true;
                    addmsg($">> 关闭串口成功！");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"关闭串口失败，请插拔串口，并重启软件！ {ex.ToString()}");
                }
            }
        }

        private void senddata_Click(object sender, RoutedEventArgs e)
        {
            byte slaveID = 1;
            ushort registerAddress = 0;
            ushort value = 100;//你要写的值
            imodbusserialmaster.WriteSingleRegister(slaveID, registerAddress, value);   // 写单个寄存器
            //imodbusserialmaster.WriteMultipleRegisters(); // 写多个寄存器

            addmsg($">> 发送数据！");
        }
    }
}
