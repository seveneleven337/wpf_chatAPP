using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Diagnostics;

namespace chat_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string messages = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            
    
        }

        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        public static String StartClient(String message)
        {
            byte[] bytes = new byte[1024];
            String res = "not working";
            try
            {

                //localhost
                /*IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6000);*/

                //working with server
                IPAddress ipAddress = System.Net.IPAddress.Parse("172.104.250.232");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 6000);

                // Create a TCP/IP  socket.
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
         
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
                    res = Encoding.ASCII.GetString(messageReceived,0, byteRecv);
                    
                    // Release the socket.
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return res.Replace("<EOF>", string.Empty);
        }

        private void connectbtn_Click(object sender, RoutedEventArgs e)
        {
            
            
            label.Content = StartClient("connected<EOF>") + Environment.NewLine;
            sendBtn.IsEnabled = true;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //  label.Content = input.Text;

            if (messages == string.Empty) { chatMessage.Text = string.Empty; }
            messages = StartClient(msgSend.Text + "<EOF>") + Environment.NewLine;
            chatMessage.Text += messages;
            msgSend.Text = string.Empty;
        }
    }
}
