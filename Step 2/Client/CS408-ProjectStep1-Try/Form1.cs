using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS408_ProjectStep1_Try
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            result_box.ReadOnly = true;
            disconnect_button.Enabled = false;
        }

        delegate void StringArgReturningVoidDelegate( string text );
        static Socket clientSoc = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
        static List<String> clientList = new List<string>();
        static bool connected = true;
        bool inserver = false;
        string myname;
        int sendQA = 0;

        private void SetText( string text ) // Creates thread safe "AppendText" using "Invoke" and "Delegate" for richtextbox.
        {
            if (this.result_box.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate( SetText );
                this.Invoke( d, new object[] { text } );
            }

            else
            {
                this.result_box.AppendText( text );
                this.result_box.AppendText("\n");
                this.result_box.Update( );
                Application.DoEvents( );
            }
        }

        

        private void Receive() // Receive function for client. Received messages from server processed here.
        {
            int ackQ = 6;                               // ASCII value of number 6 is used for acquiring questions.
            char num = (char)ackQ;                      // Client will know that it is their turn to ask a question.
            string quest = num.ToString();              // Server will be listenning until the client sends a question.

            int ackA = 5;                               // ASCII value of number 5 is used for acquiring answer acknowledgement.
            char value = (char)ackA;                    // Client will receive a question and will know that it is their turn to
            string answ = value.ToString();             // send an answer to the server.

            int ack = 4;                                // ASCII value of number 4 is used for exit situations.
            char getout = (char)ack;                    // Client will know that server had been exited.
            string terminate = getout.ToString();

            int know = 3;                               // ASCII value of number 3 is used for server disconnection.
            char serverlost = (char)know;
            string serverdead = serverlost.ToString();

            while (connected) // Main receive loop.
            {
                try
                {
                    inserver = true;

                    Byte[] buffer = new Byte[256];
                    clientSoc.Receive(buffer); // Receive the message from the server and find its appropriate condition below.

                    string line = Encoding.Default.GetString(buffer);
                    string[] type = line.Split('$');

                    if (type[0] == quest) // Question condition.
                    {
                        this.SetText("Server: " + type[1]); // Your turn to ask.
                        sendQA = 6;
                    }

                    else if (type[0] == answ) // Answer condition.
                    {
                        this.SetText("Server: " + type[1]); // Your turn to answer.
                        sendQA = 5;
                    }

                    else if (type[0] == terminate) // Exit condition.
                    {
                        this.SetText(type[1]);
                        inserver = false;
                        clientSoc.Shutdown(SocketShutdown.Both);
                        clientSoc.Close();
                        Application.Exit();
                        Environment.Exit(Environment.ExitCode);
                    }

                    else { this.SetText("Server: " + type[1]); } // Condition for printing the other messages (e.g. Round, Ask question",..).
                }

                catch   // In case of any failure.
                {
                    this.SetText("ERROR");
                    connected = false;
                    clientSoc.Close();
                }
            }
        }

        private bool userExist()    // Checks if the username already exists, "initiated from connect_button_click".
        {
            try
            {
                byte[] code = new byte[5];
                int temp = 0;

                clientSoc.Receive(code); // Receive the error code from server (whether if username exists or not).
                temp = BitConverter.ToInt32(code, 0);

                if (temp == 200) { return false; } // If exists, return false.
                else { return true; }
            }

            catch
            {
                this.SetText("Connection lost.");
                return false;
            }
        }

        private void connect_button_Click (object sender, EventArgs e)
        {
            string serverIP = ip_box.Text;
            int portNum = int.Parse(port_box.Text);
            myname = name_box.Text.ToLower();

            try
            {
                if (!clientSoc.Connected) // Initiate a connection if not already connected.
                    clientSoc.Connect(serverIP, portNum);

                clientSoc.Send(Encoding.ASCII.GetBytes(myname)); // Send the current clients name.

                if (userExist()) // If true, then it means that the entered username is unique.
                {
                    this.SetText("Connection established!");
                    Thread receiveThread = new Thread(new ThreadStart(Receive));    // Create a thread (receiveThread) for "Receive" function.
                    receiveThread.Start();                                          // Start the receiveThread thread.
                    connect_button.BackColor = Color.Green;
                    connect_button.Enabled = false;
                    disconnect_button.Enabled = true;
                }

                else                                                                // If username exists, connection is not allowed.
                {
                    this.SetText( myname + " already exists!");
                    connect_button.BackColor = Color.Red;
                }
            }

            catch                                                                   // If connection fails, catch.
            {
                connect_button.BackColor = Color.Red;
                this.SetText("Problem occured while connecting...\n");
                clientSoc.Close();
            }
        }

        private void result_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void send_line_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void send_button_Click(object sender, EventArgs e)  // Send the question, works fine as long as you use it AFTER
        {                                                           // "Ask Question" command (and send the question and answer
                                                                    // together, seperating with a "?").
            string message = send_line_box.Text;

            if (message == "terminate") { this.Close(); }

            if (sendQA == 6) // Question
            {
                int ackQ = 6;
                char num = (char)ackQ;
                string quest = num.ToString();

                clientSoc.Send(Encoding.Default.GetBytes(quest + "$" + message));
                this.SetText("\nYour question has been sent. ( " + message + " )");
            }

            else if (sendQA == 5) //Answer
            {
                int ackA = 5;
                char var = (char)ackA;
                string answ = var.ToString();

                clientSoc.Send(Encoding.Default.GetBytes(answ + "$" + message));
                this.SetText("\nYour answer has been sent. ( " + message + " )\n");
            }

            else { this.SetText("\nNot your turn."); } // If a user sends something while it is not their turn.
            send_line_box.Clear();
        }

        private void name_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void port_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void ip_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void clear_button_Click(object sender, EventArgs e) // To clear the results box (can get pretty messy after a few rounds).
        {
            result_box.Clear();
        }

        private void disconnect_button_Click(object sender, EventArgs e) // To close the server, not very healthy.
        {
            disconnect_button.Enabled = false;
            connect_button.BackColor = Color.Gray;
            this.SetText("Disconnected\n");
            connected = false;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) // Override function for "X" button.
        {  
            int shut = 4;
            char num = (char)shut;
            string exit = num.ToString();
            string terminate = exit + "$Shutting Down...";
            Byte[] close = Encoding.Default.GetBytes(terminate);

            if (inserver)
            {
                clientSoc.Send(close);
                inserver = false;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
                Environment.Exit(Environment.ExitCode);
                return;
            }

            base.OnFormClosing(e);
        }
    }
}
