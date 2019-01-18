using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace CS408_ProjectStep1_Try_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            result_box.ReadOnly = true;     // Output box is not editable.
            server_close.Enabled = false;   // Closing the server is initially not allowed (since there aren't any).
        }

        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> clientSockets = new List<Socket>();
        static List<String> nameList = new List<string>();

        static bool terminating = false;
        static bool accept = true;
        Thread acceptThread, receiveThread;
        delegate void StringArgReturningVoidDelegate(string text);
       
        private void SetText(string text)                   // Creates thread safe "AppendText" using "Invoke" and "Delegate".
        {
            if (this.result_box.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }

            else
            {
                this.result_box.AppendText(text); 
                this.result_box.AppendText("\n");
                this.result_box.Update();
                Application.DoEvents();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    byte[] exitcode = BitConverter.GetBytes(505);  // Send the error code "200" which blocks the client

                    if (0 != clientSockets.Count())
                    {
                        foreach (Socket client in clientSockets)
                        {
                            client.Send(exitcode);
                        }
                    }
                    accept = false;
                    terminating = true;
                    receiveThread.Abort();
                    acceptThread.Abort();
                    this.SetText("Server is closed...");
                    this.Close();
                    break;
            }

            
        }

        private void Accept()   // Accepts the client that is requesting connection.
        {
            while (accept)                                  
            {
                try
                {
                    clientSockets.Add(serverSocket.Accept());   // Add the new clients information to "clientSockets" list.
                    receiveThread = new Thread(new ThreadStart(Receive));    // Create a thread (receiveThread) for "Receive" function.
                    receiveThread.Start();  // Start the receiveThread thread.
                }

                catch       // If connection fails.
                {
                    if (terminating)
                    {
                        this.SetText("Server stopped working...");
                        accept = false;
                    }

                    else
                    {
                        this.SetText("Problem occured in accept function...");
                    }
                }
            }
        }

        private void Receive()  // Basically, server starts to listen.
        {
            bool connected = true;
            int lenClientSoc = clientSockets.Count();
            Socket thisClient = clientSockets[lenClientSoc - 1];
            bool isExist = false;
            string temp = "";
            int round = 1;

            while (connected && !terminating)   // Loops until a disconnection or otherwise occurs.
            {
                try
                {
                    if (!isExist)                       // If the player name does not exist in our "nameList".
                    {                                   // Clients will always enter to this condition in their first loop.
                        Byte[] name = new byte[20];     
                        int rec = thisClient.Receive(name); // Receive the name of the current client.

                        string nick = Encoding.ASCII.GetString(name);   // Encodes the name of the client and eliminates the
                        nick = nick.Substring(0, nick.IndexOf("\0"));   // unnecessary parts of the buffer.

                        if (nameList.Contains(nick))    // If the current clients name already exists.
                        {
                            byte[] exist = BitConverter.GetBytes(200);  // Send the error code "200" which blocks the client
                            thisClient.Send(exist);                     // from connecting (see client code for details).
                        }

                        else
                        {
                            byte[] notexist = BitConverter.GetBytes(100);   // If name does not exists, send a random value
                            thisClient.Send(notexist);                      // so the boolean function in client code will
                            this.SetText(nick + " connected");            // return true and client will connect succesfully.
                            nameList.Add(nick);
                            isExist = true;
                            temp = nick;
                        }
                    }

                    else
                    {
                        int first, second, delete, trans, exitcode;

                        if (lenClientSoc > 1)   // Starting point of the game, a client will loop without entering to this condition
                        {                       // until another client connects in which case "IenClientSoc" will become "2".
                            
                            int compare = nameList[0].CompareTo(nameList[1]);   // Compares the two connected players names alphabetically,
                                                                                // returns 0 if the first joined player is smaller than second
                                                                                // joined player (and vice versa).
                            
                            if (compare < 0)    // this condition satisfies the alphabetical constraint
                            {                   // where alphabetically smaller player must start first.

                                first = 0;      // If our compare value is 0 then it means that our first
                                second = 1;     // joined player is alphabetically smaller than the second
                                delete = 0;     // joined player.
                            }

                            else                // And otherwise.
                            {
                                first = 1;
                                second = 0;
                                delete = 1;
                            }

                            if (round % 2 == 0) // This condition is to make the program switch next turn in order to
                            {                   // sustain the event of exchanging roles each turn.
                                trans = second;
                                second = first;
                                first = trans;
                            }

                            this.SetText("Round" + round);

                            foreach (Socket client in clientSockets) // Let all clients know the round number.
                            {
                                try
                                {
                                    Byte[] roundnum = Encoding.Default.GetBytes("Round" + round);
                                    client.Send(roundnum);
                                }

                                catch
                                {
                                    this.SetText("There is a problem! Check the connection...");
                                    terminating = true;
                                    serverSocket.Close();
                                }
                            }

                            round = round + 1;  // Increase by 1, so next turn modulo 2 will be 0 and roles of the clients will be switched.

                            string ask;
                            ask = "\nAsk question!";                            // "Ask question!" command is prepared and sent to the first
                            Byte[] initiate = Encoding.Default.GetBytes(ask);   // client (alphabetical in first round, switched in next rounds).
                            
                            clientSockets[first].Send(initiate); // (send to first client)

                            Byte[] buffer1 = new Byte[64];
                            clientSockets[first].Receive(buffer1);  // Listen for the first question (and answer, seperated with "?").
                            string firstQuestion = Encoding.Default.GetString(buffer1);
                            exitcode = BitConverter.ToInt32(buffer1, 0);
                            if (exitcode == 404 || firstQuestion == "terminate")// If the client sent "terminate", end the game.
                            {
                                byte[] exiting = BitConverter.GetBytes(404);  // Send the error code "404" which blocks the client
                                clientSockets[second].Send(exiting);
                                this.SetText("A player has disconneted\nQuitting the game...\n\n Closing server...");
                                clientSockets.Remove(clientSockets[second]);
                                clientSockets.Remove(clientSockets[first]);
                                connected = false; 
                                break;
                            } 
                            
    

                            this.SetText("Question and answer is ready." + firstQuestion);  // Report that first question is ready.
                            this.SetText("Question sent to other user, waiting for an answer.");  // Report that first question is ready.

                            string firstQ, firstA, secondA;            // We seperated the question and answer by the use of "?" since in the project
                            string[] firstArray = firstQuestion.Split('?');     // paper it states that client sends question AND answer, so we thought that they
                            firstQ = firstArray[0] + "?";                       // different strings (2 question/answer = total of 4 strings).
                            //this.SetText("First question is: " + firstQ);

                            firstA = firstArray[1].Substring(0, firstArray[1].IndexOf("\0")).ToLower(); // Using "ToLower()" to eliminate the errors which may occur
                            //this.SetText("First answer is: " + firstA);                               // due to lowercase/uppercase matching.


                            Byte[] sendSecond = Encoding.Default.GetBytes(firstQ);
                            clientSockets[second].Send(sendSecond);                 // Send the answer of the second (answer to the first question).

                            Byte[] buffer3 = new Byte[64];
                            clientSockets[second].Receive(buffer3);
                            string final = Encoding.Default.GetString(buffer3).ToLower();
                            exitcode = BitConverter.ToInt32(buffer3, 0);
                            if (exitcode == 404 || final == "terminate")
                            {
                                byte[] exiting = BitConverter.GetBytes(404);  // Send the error code "200" which blocks the client
                                clientSockets[first].Send(exiting);
                                this.SetText("A player has disconneted\nQuitting the game...\n\n Closing server...");
                                clientSockets.Remove(clientSockets[second]);
                                clientSockets.Remove(clientSockets[first]);
                                connected = false;
                                break;
                            } 
                            final = final.Substring(0, final.IndexOf("\0"));
                            //this.SetText("Response to second is: " + finalFirst);
                            this.SetText(nameList[second] + " answered: " + final);

                            if (firstA == final)                              // If the answer of first client is true, send the appropriate acknowledgements.
                            {
                                Byte[] bf5 = Encoding.Default.GetBytes("Correct Answer!");
                                clientSockets[second].Send(bf5);

                                Byte[] bf6 = Encoding.Default.GetBytes(nameList[second] + " answered correctly!");
                                clientSockets[first].Send(bf6);

                                this.SetText(nameList[second] + " answered correctly.");
                            }

                            else if (firstA != final)                          // If not, send the appropriate acknowledgements.
                            {
                                Byte[] bf7 = Encoding.Default.GetBytes("Incorrect!");
                                clientSockets[second].Send(bf7);

                                Byte[] bf8 = Encoding.Default.GetBytes(nameList[second] + " answered incorrectly!");
                                clientSockets[first].Send(bf8);

                                this.SetText(nameList[second] + " answered incorrectly.");
                            }
                        }
                    }
                }

                catch       // If the game fails, should not enter if everything goes as planned (no meaningless inputs etc.).
                {
                    this.SetText("Cannot receive");
                    
                    if (!terminating)
                        thisClient.Close();

                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connect_button_Click(object sender, EventArgs e)   // Connect button initiator.
        {
            int portNum =  int.Parse(port_num.Text);

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portNum);   // Start server with the given port number.
                this.SetText("Server is ready.");
                this.SetText("There is not enough player to start the game.\nWaiting for players..\n");

                server_close.Enabled = true;
                connect_button.Enabled = false;

                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                acceptThread = new Thread(new ThreadStart(Accept));      // Initiate a thread for "Accept" function in order to accept
                acceptThread.Start();                                           // any client that wants to connect, and start it.
            }

            catch   // If the server initiation fails.
            {
                this.SetText("There is a problem! Check the port number and try again!");
            }
        }


        private void port_num_TextChanged(object sender, EventArgs e)
        {

        }

        private void result_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void server_close_Click(object sender, EventArgs e) // To close the server, not very healthy.
        {
            this.SetText("Server has been terminated");
            terminating = true;
            connect_button.Enabled = true;
            server_close.Enabled = false;
            this.Close();
        }

        private void clear_button_Click(object sender, EventArgs e) // To clear the results box (can get pretty messy after a few rounds).
        {
            result_box.Clear();
        }
    }
}
