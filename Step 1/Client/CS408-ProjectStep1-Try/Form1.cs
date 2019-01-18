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
        string myname;
        Thread receiveThread;

        private void SetText( string text ) // Creates thread safe "AppendText" using "Invoke" and "Delegate".
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
                    byte[] exitcode = BitConverter.GetBytes(404);  // Send the error code "200" which blocks the client
                    if(connected == true)
                        clientSoc.Send(exitcode);
                    receiveThread.Abort();
                    clientSoc.Close();
                    break;
            }
        }

        private void Receive()  // Basically, client starts to listen.
        {
            while (connected)
            {
                try
                {   
                    int exiting = 0;
                    Byte[] buffer = new Byte[256];
                    clientSoc.Receive(buffer);  // Receive messages from server,
                    string message = Encoding.Default.GetString(buffer);
                    exiting = BitConverter.ToInt32(buffer, 0);
                    if (exiting == 404)
                    {
                        this.SetText("A player has disconnected\nQuitting game...\n");
                        connected = false;
                        break;
                    }
                    else if (exiting == 505)
                    {
                        this.SetText("Server has disconnected\nQuitting game...\n");
                        connected = false;
                        break;

                    }
                    this.SetText("Server: " + message);  // and print them.
                   
                }

                catch   // In case of any failure.
                {
                    
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

                clientSoc.Receive(code);    // Receive the error code from server (whether if username exists or not).
                temp = BitConverter.ToInt32(code, 0);   

                if (temp == 200)        // If exists, return false.
                {
                    return false;
                }

                else
                    return true;
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
                if (!clientSoc.Connected)   // Initiate a connection if not already connected.
                    clientSoc.Connect(serverIP, portNum);

                clientSoc.Send(Encoding.ASCII.GetBytes(myname));  // Send the current clients name.

                if (userExist())    // If true, then it means username is unique.
                {
                    this.SetText("Connection established!");
                    receiveThread = new Thread(new ThreadStart(Receive)); /// Create a thread (receiveThread) for "Receive" function.
                    receiveThread.Start();  // Start the receiveThread thread.
                    connect_button.BackColor = Color.Green;
                    connect_button.Enabled = false;
                    disconnect_button.Enabled = true;
                }

                else     // If username exists, connection is not allowed.
                {
                    this.SetText( myname + " already exists!\n");
                    connect_button.BackColor = Color.Red;
                }
            }

            catch   // If connection fails, catch.
            {
                connect_button.BackColor = Color.Red;
                this.SetText("Problem occured while connecting...\n");
                clientSoc.Close();
            }

            ip_box.Clear();
            port_box.Clear();
            name_box.Clear();
        }

        private void result_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void send_line_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void send_button_Click(object sender, EventArgs e)  // Send the question, works fine as long as you use it AFTER
        {                                                           // "Ask Question!" command (and send the question and answer
            string message = send_line_box.Text;                    // together, seperating with a "?").
            

            if (message == "terminate")
            {
                byte[] exitcode = BitConverter.GetBytes(404);  // Send the error code "200" which blocks the client
                if (connected == true)
                    clientSoc.Send(exitcode);
                receiveThread.Abort();
                clientSoc.Close();
                this.Close();
            }
            else
               clientSoc.Send(Encoding.Default.GetBytes(message)); 

           // this.SetText("\nYour message has been sent. ( " + message + " )\n");
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

        private void disconnect_button_Click(object sender, EventArgs e)  // To close the server, not very healthy.
        {
            disconnect_button.Enabled = false;
            connect_button.BackColor = Color.Gray;
            this.SetText("Disconnected\n");
            connected = false;
            this.Close();
        }
    }
}
