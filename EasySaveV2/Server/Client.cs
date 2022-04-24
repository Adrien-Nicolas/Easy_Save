using Newtonsoft.Json;
using EasySaveV2;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Server
{

    /// <summary>
    /// Allow the server to send, receive, and manage client
    /// </summary>
    class Client
    {
        public Socket _socket { get; set; }
        public int Id { get; set; }


        private byte[] _buffer;

        public Client(Socket socket, int id)
        {
            _socket = socket;
            Id = id;
            StartReceiving();
        }




        /// <summary>
        /// Start receive on a socket
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
        /// Use when receive msg from client
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


                    TraiterMessage(data);


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
        /// Manage the uncoming message from the client to use the right function
        /// </summary>
        /// <param name="data"></param>
        private void TraiterMessage(string data)
        {

            JSONRpc valReceive = JsonConvert.DeserializeObject<JSONRpc>(data);


            try
            {
                string result = "";
                
                var type = Type.GetType("EasySaveV2.Server.FonctionServ." + valReceive.className);
                object clazz = Activator.CreateInstance(type);


                var method = type.GetMethod(valReceive.methodName);
                result = (string)method.Invoke(clazz, valReceive.args);

                

                Send(JsonConvert.SerializeObject(new JsonResponse(result, valReceive.Id)));

            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }


            //MessageBox.Show("Serveur : " + data);

            
        }

        /// <summary>
        /// Stock link between server and client
        /// </summary>
        private void Disconnect()
        {
            // Close connection
            _socket.Disconnect(true);
            
            
            //MessageBox.Show($"Serveur : Client {this.Id} Diconnected !");
            
            
            // Next line only apply for the server side receive
            ClientController.RemoveClient(this.Id);
            // Next line only apply on the Client Side receive
            //Here you want to run the method TryToConnect()
        }

        /// <summary>
        /// Send a msg to a client
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
                throw new Exception();
            }
        }

    }

    
    /// <summary>
    /// Manage all clients
    /// </summary>
    static class ClientController
    {
        public static List<Client> Clients = new List<Client>();

        public static void AddClient(Socket socket)
        {
            Clients.Add(new Client(socket, Clients.Count));
        }

        public static void RemoveClient(int id)
        {
            Clients.RemoveAt(Clients.FindIndex(x => x.Id == id));
        }
    }
}
