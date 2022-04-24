using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace ClientEasySaveV2
{
    class Client
    {
        public Socket _socket { get; set; }
        public int port { get; }
        public string ipAddress { get; }

        private byte[] _buffer;

        //public delegate void ConnectionEvent(Client c);
        //public event ConnectionEvent OnConnect;

        public delegate void MessageReceivedEvent(Client c, string msg);
        public event MessageReceivedEvent OnMesssageReceived;

        public delegate void OnCrash();
        public event OnCrash OnCrashed;

        public Client(Socket socket)
        {
            _socket = socket;
        }

        /// <summary>
        /// Wait until connect success
        /// </summary>
        /// <param name="ie"></param>
        public void WaitAndConnect(IPEndPoint ie)
        {
            while (true)
                try
                {
                    _socket.Connect(ie);
                    break;
                }
                catch { Thread.Sleep(200); }

            this.StartReceiving();
        }



        /// <summary>
        /// Async, start receiving response
        /// </summary>
        public void StartReceiving()
        {
            try
            {
                _buffer = new byte[4];
                _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch { }
        }

        /// <summary>
        /// Callback use when message receive
        /// </summary>
        /// <param name="AR"></param>
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                // if bytes are less than 1 takes place when a client disconnect from the server.
                // So we run the Disconnect function on the current client
                if (_socket.EndReceive(AR) > 1)
                {
                    // Convert the first 4 bytes (int 32) that we received and convert it to an Int32 (this is the size for the coming data).
                    _buffer = new byte[BitConverter.ToInt32(_buffer, 0)];
                    // Next receive this data into the buffer with size that we did receive before
                    _socket.Receive(_buffer, _buffer.Length, SocketFlags.None);
                    // When we received everything its onto you to convert it into the data that you've send.
                    // For example string, int etc... in this example I only use the implementation for sending and receiving a string.

                    // Convert the bytes to string and output it in a message box
                    string data = Encoding.Default.GetString(_buffer);





                    OnMesssageReceived(this, data);

                    // Now we have to start all over again with waiting for a data to come from the socket.
                    StartReceiving();
                }
                else
                {
                    Disconnect();
                }
            }
            catch
            {
                // if exeption is throw check if socket is connected because than you can startreive again else Dissconect
                if (_socket.Connected)
                {
                    Disconnect();
                }
                else
                {
                    StartReceiving();
                }
            }
        }

        /// <summary>
        /// Disconnect the Client
        /// </summary>
        public void Disconnect()
        {
            // Close connection
            _socket.Disconnect(true);
        }

        /// <summary>
        /// Send string to the linked socket
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            try
            {
                /* what hapends here:
                     1. Create a list of bytes
                     2. Add the length of the string to the list.
                        So if this message arrives at the server we can easily read the length of the coming message.
                     3. Add the message(string) bytes
                */

                var fullPacket = new List<byte>();
                fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                fullPacket.AddRange(Encoding.Default.GetBytes(data));

                /* Send the message to the server we are currently connected to.
                Or package stucture is {length of data 4 bytes (int32), actual data}*/
                _socket.Send(fullPacket.ToArray());
            }
            catch (Exception ex)
            {
                OnCrashed();
                //MessageBox.Show(ex.ToString());
            }
        }

    }
}
