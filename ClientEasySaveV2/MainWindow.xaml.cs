using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace ClientEasySaveV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client c;

        private ClientViewModel _viewModel = new ClientViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
            //DataContext = new ClassViewModel();

            //// Named Mutexes are available computer-wide. Use a unique name.
            //using (var mutex = new Mutex(false, "ApplicationMonoInstance"))
            //{
            //    // TimeSpan.Zero to test the mutex's signal state and
            //    // return immediately without blocking
            //    bool isAnotherInstanceOpen = !mutex.WaitOne(TimeSpan.Zero);
            //    if (isAnotherInstanceOpen)
            //    {
            //        MessageBox.Show("Only one instance of this app is allowed.");
            //        return;
            //    }


            Advancement.Text = "Connexion en cours...";


            Socket _connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            c = new Client(_connectingSocket);

            //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12234);


            c.OnMesssageReceived += (c, msg) =>
            {

                JsonResponse rep = JsonConvert.DeserializeObject<JsonResponse>(msg);
                Request.listRequest.ForEach((Request r) =>
                {

                    if (rep.id == r.jsonRpc.id)
                    {
                        ThreadPool.QueueUserWorkItem((o) =>
                        {
                            Thread t = new Thread(new ThreadStart(() =>
                            {

                                r.traiterDonnees(rep.response);

                            }));
                            t.Start();

                        });

                            //Request.listRequest.Remove(r);
                            //MessageBox.Show($"Requete trouvée ! : id : {r.jsonRpc.id} with response : {rep.response}");
                    }

                });

            };


            //c.WaitAndConnect(remoteEP);





            //mutex.ReleaseMutex();

        }

        /// <summary>
        /// Disconnect user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Disconnect(object sender, RoutedEventArgs e)
        {
            c.Disconnect();
        }


        /// <summary>
        /// Test Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Send(object sender, RoutedEventArgs e)
        {
            AskListJob();
        }


        /// <summary>
        /// Create a new Request for asking the job list
        /// </summary>
        private void AskListJob()
        {
            JsonRPC jsonRpc = new JsonRPC("ListJobClient", "GetListJob", null);
            Request newRequest = new Request(jsonRpc, (string s) =>
            {
                List<Job> listReceive = JsonConvert.DeserializeObject<List<Job>>(s);
                _viewModel.RefreshList(listReceive);
            });

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);
        }

        /// <summary>
        /// Connect the user to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_ConnectButton(object sender, RoutedEventArgs e)
        {
            int port;
            string ipAddress;

            port = Int32.Parse(PORTTextBox.Text);
            ipAddress = IPTextBox.Text;

            bool isValidIP = ValidateIPAddress(ipAddress);
            bool isValidPORT = ValidatePORT(port);

            

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            Thread t = new Thread(new ThreadStart(() =>
            {

                c.WaitAndConnect(remoteEP);
                this.Dispatcher.Invoke(() =>
                {
                    Advancement.Text = "Connecté !";
                });

                bool cancel = false;
                this.Closed += (EventArgs, o) => cancel = true;

                c.OnCrashed += () =>
                {
                    cancel = true;
                    MessageBox.Show("disconnect");
                };

                Thread t2 = new Thread(new ThreadStart(() =>
                {
                    while(!cancel)
                    {
                        AskListJob();
                        Thread.Sleep(500);
                    }
                }));
                t2.Start();


            }));
            t.Start();

        }

        bool ValidateIPAddress(string ipAddress)
        {
            // Set the default return value to false
            bool isIPAddress = false;

            // Set the regular expression to match IP addresses
            string ipPattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";

            // Create an instance of System.Text.RegularExpressions.Regex
            Regex regex = new Regex(ipPattern);

            // Validate the IP address
            isIPAddress = regex.IsMatch(ipAddress);

            return isIPAddress;
        }

        bool ValidatePORT(int port)
        {

            if (port > 15000)
            {
                //MessageBox.Show("Entré un numéro de port >1500");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PauseJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToPause = (sender as Button).DataContext as Job;
            JsonRPC jsonRpc = new JsonRPC("LaunchJobController", "PauseJob", new object[] { JsonConvert.SerializeObject(jobToPause) });
            Request newRequest = new Request(jsonRpc, (string s) => { });

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);
        }

        private void PlayJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToLaunch = (sender as Button).DataContext as Job;
            JsonRPC jsonRpc = new JsonRPC("LaunchJobController", "PlayJob", new object[] { JsonConvert.SerializeObject(jobToLaunch) });
            Request newRequest = new Request(jsonRpc, (string s) => { });

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);
        }

        private void StopJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToStop = (sender as Button).DataContext as Job;
            JsonRPC jsonRpc = new JsonRPC("LaunchJobController", "StopJob", new object[] { JsonConvert.SerializeObject(jobToStop) });
            Request newRequest = new Request(jsonRpc, (string s) => { });

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);

        }


        private void LoadJob_Click(object sender, RoutedEventArgs e)
        {
            Job jobToLoad = (sender as Button).DataContext as Job;
            JsonRPC jsonRpc = new JsonRPC("LaunchJobController", "LoadJob", new object[] { JsonConvert.SerializeObject(jobToLoad) });
            Request newRequest = new Request(jsonRpc, (string s) => { });

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            JsonRPC jsonRpc = new JsonRPC("ListJobClient", "AddJob", null);
            Request newRequest = new Request(jsonRpc, (string s) => {});

            string val = JsonConvert.SerializeObject(newRequest.jsonRpc);
            c.Send(val);
        }
    }

}

/// <summary>
/// Class to add in the request, to call a specific method in a specific function
/// </summary>
class JsonRPC
{
    public int id;
    public string methodName;
    public string className;
    public object[] args;


    private static int totalId = 0;
    public JsonRPC(string c, string m, object[] j)
    {
        totalId++;
        args = j;
        methodName = m;
        className = c;
        id = totalId;
    }



}

/// <summary>
/// Contain a JsonRPC with a delegate to use with the response
/// </summary>
class Request
{

    public delegate void DELG(string data);
    DELG delg;

    public JsonRPC jsonRpc;
    public static List<Request> listRequest = new List<Request>();

    public Request(JsonRPC rpc, DELG d)
    {
        this.jsonRpc = rpc;
        delg = d;
        listRequest.Add(this);
    }
    public void traiterDonnees(string data)
    {
        delg.Invoke(data);
    }

}

/// <summary>
/// If we have to send a Response
/// </summary>
class JsonResponse
{
    public int id;
    public string response;

    public JsonResponse(string r, int i)
    {
        id = i;
        response = r;
    }
}

