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
            result_box.ReadOnly = true; // Output box is not editable.
            roundbox.ReadOnly = true; // Roundbox is not editable.
            server_close.Enabled = false; // Closing the server is initially not allowed (since there aren't any).
        }

        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static List<Socket> clientSockets = new List<Socket>();
        static List<String> nameList = new List<string>();
        static List<Player> playerList = new List<Player>();
        static List<Socket> sortedSockets = new List<Socket>();
        Dictionary<Player, Socket> hash = new Dictionary<Player, Socket>();
        bool unexpectedleave = false;
        bool gameover = false;
        bool tie = false;
        string received = "";
        int receivecount = 0;
        bool gamestart = false;
        int connectcount = 0;
        int questcount = 0;
        static bool terminating = false;
        static bool accept = true;
        Thread acceptThread, receiveThread, gameThread;
        delegate void StringArgReturningVoidDelegate(string text);

        private void SetText(string text) // Creates thread safe "AppendText" using "Invoke" and "Delegate" for main richtextbox.
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

        private void SetRound(string text) // Creates thread safe "AppendText" using "Invoke" and "Delegate" four roundtextbox.
        {
            if (this.roundbox.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetRound);
                this.Invoke(d, new object[] { text });
            }

            else
            {
                this.roundbox.AppendText(text);
                this.roundbox.AppendText("\n");
                this.roundbox.Update();
                Application.DoEvents();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)   // Override function for closing the server via "X" button.
        {
            int shut = 4;
            char num = (char)shut;
            string exit = num.ToString();
            string terminate = exit + "$Shutting Down...";

            Byte[] close = Encoding.Default.GetBytes(terminate);

            if (hash.Count != 0)
            {
                foreach (Player user in playerList)
                {
                    hash[user].Send(close); 
                    hash[user].Shutdown(SocketShutdown.Both);
                    hash[user].Close();
                }
            }

            Application.Exit();
            Environment.Exit(Environment.ExitCode);
            return;
        }

        private void Accept() // Accepts the client that is requesting connection.
        {
            while (accept)
            {
                try
                {
                    if (!gamestart)
                    {
                        clientSockets.Add(serverSocket.Accept());   // Add the new clients information to "clientSockets" list.

                        if (clientSockets.Count > 1) { this.Invoke((MethodInvoker)delegate { start_button.Enabled = true; }); } // Error case.

                        receiveThread = new Thread(new ThreadStart(Receive)); // Create a thread (receiveThread) for "Receive" function.
                        receiveThread.IsBackground = true;                    // Everything that is sent from clients are processed here.
                        receiveThread.Start();                                // Start the receiveThread thread.
                    }
                }

                catch // If connection fails.
                {
                    if (terminating)
                    {
                        this.SetText("Server stopped working...");
                        accept = false;
                    }

                    else { this.SetText("Problem occured in accept function..."); }
                }
            }
        }

        private void Receive() // Server receives all the messages in this function.
        {
            bool connected = true;
            int lenClientSoc = clientSockets.Count();
            Socket thisClient = clientSockets[lenClientSoc - 1];
            bool isExist = false;
            Player playerone = new Player();

            int numero = 2;                         // ASCII value of number 2 is used for preventing further
            char started = (char)numero;            // connections to the server once the game starts.
            string noaccept = started.ToString();

            int ackA = 5;                           // ASCII value of number 5 is used for acquiring answers.
            char num = (char)ackA;                  // Server will know that the messages that it is receiving
            string amark = num.ToString();          // with ASCII value of 5 are answers.

            int ackQ = 6;                           // ASCII value of number 6 is used for acquiring questions.
            char var = (char)ackQ;                  // Server will know that the messages that it is receiving
            string qmark = var.ToString();          // with ASCII value of 6 are questions.

            int ack = 4;                            // ASCII value of number 4 is used for exit situations.
            char exit = (char)ack;                  // Server will know that a user had been exited and will
            string quit = exit.ToString();          // remove that user appropriately in its corresponding statement below.

            while (connected && !terminating)       // Loops until a disconnection or otherwise occurs.
            {
                try
                {
                    if (!isExist)                                       // If the player name does not exist in our "nameList".
                    {                                                   // Clients will always enter to this condition in their first loop.
                        Byte[] name = new byte[20];
                        int rec = thisClient.Receive(name);             // Receive the name of the current client.
                        Player test = new Player();
                        string nick = Encoding.ASCII.GetString(name);   // Encodes the name of the client and eliminates the
                        nick = nick.Substring(0, nick.IndexOf("\0"));   // unnecessary parts of the buffer (prevents capital letter misguidance).
                        test.name = nick;

                        if (playerList.Contains(test))                    // If the current clients name already exists.
                        {
                            byte[] exist = BitConverter.GetBytes(200);  // Send the error code "200" which blocks the client
                            thisClient.Send(exist);                     // from connecting (see client code for details).
                        }

                        else
                        {
                            byte[] notexist = BitConverter.GetBytes(100);   // If name does not exists, send a random value
                            thisClient.Send(notexist);

                            if (!gamestart)                                 // If the name does not already exist and game has not started yet.
                            {
                                this.SetText(nick + " connected");

                                playerone.name = nick;                      // Add the newly connected client to the playerList.
                                playerone.connectedas = connectcount;
                                playerone.point = 0;
                                playerList.Add(playerone);
                                connectcount = connectcount + 1;            // Increment connected client amount.
                                hash.Add(playerone, thisClient);
                                isExist = true;
                            }

                            else                                            // If the game has started, then prevent connection.         
                            {
                                byte[] alreadystarted = Encoding.Default.GetBytes(noaccept + "$The game has already started. You are not allowed to join.");
                                thisClient.Send(alreadystarted);
                            }
                        }
                    }

                    else                                            // Clients that are connected will enter this else condition
                    {                                               // for the rest of the game. Messages will be processed here.
                        Byte[] sthg = new Byte[2048];
                        thisClient.Receive(sthg);
                        received = Encoding.Default.GetString(sthg);
                        string[] type = received.Split('$');        // We made use of "$" sign to split the ASCII code from
                                                                    // the actual message sent by the user.
                        if (type[0] == amark) // Answer condition.
                        {
                            playerone.answer = type[1];
                            receivecount++;
                        }

                        else if (type[0] == qmark) // Question condition.  
                        {
                            playerone.question = type[1];
                            questcount++;
                        }

                        else if (type[0] == quit) // Exit condition.
                        {
                            hash[playerone].Shutdown(SocketShutdown.Both);  // Shut down that users socket.
                            hash[playerone].Close();
                            hash.Remove(playerone);

                            playerList.Remove(playerone); // and remove him/her.

                            if (hash.Count == 1) // If only one player is left in the game.
                            {
                                gameover = true;
                            }

                            else
                            {
                                unexpectedleave = true;
                            }  

                            foreach (Player client in playerList)
                            {
                                //this.SetText(client.name);
                            }

                            break;
                        }

                        else
                        {
                            this.SetText("ERROR");
                        }
                    }
                }

                catch // If receive malfunctions. 
                {
                    this.SetText("Cannot receive");
                    if (!terminating) { thisClient.Close(); }
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
                     
        }

        private void game() // The function where the game itself runs.
        {
            int round = 1; // Initial round value.
            playerList.Sort((a, b) => a.name.CompareTo(b.name));    // Sort the players alphabetically and ask the first question
                                                                    // accordingly. After that, applies a round-robin fashion.

            while (gamestart && round != 3)                         // Game will continue until one player is left or round number is fulfilled (main game loop).
            {
                roundDisplay(round);                                // Display which round on server window.

                foreach (Player client in playerList)               // Let all clients know the round number.
                {
                    try
                    {
                        Byte[] roundnum = Encoding.Default.GetBytes("0$Round: " + round);   
                        hash[client].Send(roundnum); // Display the round number on each clients window.
                    }

                    catch
                    {
                        this.SetText("ERROR");
                        terminating = true;
                        serverSocket.Close();
                    }
                }

                round = round + 1;  // Increment the round value (for next round display). 

                int ack = 6;        // Question condition (ASCII of 6).
                char character = (char)ack;
                string qmark = character.ToString();

                int ans = 5;        // Answer condition (ASCII of 5).
                char mahalle = (char)ans;
                string amark = mahalle.ToString();

                string ask;
                ask = qmark + "$Ask a question."; 
                Byte[] initiate = Encoding.Default.GetBytes(ask);
                
                Deleted:
                foreach (Player client in playerList)
                {
                    this.SetText("Currently asking: " + client.name);
                   
                    hash[client].Send(initiate);

                    while (questcount == 0 && !gameover && !unexpectedleave) {} // Wait here.

                    if (unexpectedleave) // If a player leaves while it is their turn to ask.
                    {
                        receivecount = 0;   // Set the initial values to 0.
                        questcount = 0;
                        unexpectedleave = false;

                        this.SetText("The player who supposed to ask question left the game \nunexpectedly.\nGame will continue without that player.");
                        goto Deleted;
                    }

                    if (gameover) { break; }  // If the game is over.

                    string firstQ, firstA;                                  // We seperated the question and answer by the use of "?" since in the project
                    string[] firstArray = client.question.Split('?');       // paper it states that client sends question AND answer, so we thought that they
                    firstQ = firstArray[0] + "?";                           // different strings (2 question/answer = total of 4 strings).

                    this.SetText("Question is ready. [ " + client.question + "]");                           // Report that the question is ready.
                    this.SetText("Question has been sent to other players.\nWaiting for their answers.");    // Report that first question is ready.

                    firstA = firstArray[1].Substring(0, firstArray[1].IndexOf("\0")).ToLower(); // Using "ToLower()" to eliminate the errors which may occur
                                                                                                // due to lowercase/uppercase matching.
                    Byte[] sendSecond = Encoding.Default.GetBytes(amark + "$" + firstQ);

                    foreach (Player answerer in playerList) // Send the question to each client.
                    {
                        if (answerer.name != client.name)   // Do not send the question to the originator (obviously).
                        {
                            try { hash[answerer].Send(sendSecond); }

                            catch
                            {
                                this.SetText("ERROR");
                                terminating = true;
                                serverSocket.Close();
                            }
                        }
                    }

                    while (receivecount != (playerList.Count - 1) && !gameover && !unexpectedleave) {} // Wait here until everybody has answered.

                    if (unexpectedleave) // If a player leaves after a question has been asked.
                    {
                        unexpectedleave = false;
                        this.SetText("A player left the game unexpectedly.\nGame will continue without that player.");
                    }

                    if (gameover) { break; } // If only one player is left.

                    foreach (Player contestant in playerList)   // Check the answers of each client.
                    {
                        Byte[] buffer3 = new Byte[2048];

                        if (contestant.name != client.name)
                        {
                            string final = contestant.answer;
                            final = final.Substring(0, final.IndexOf("\0"));

                            if (firstA == final) // If the answer is true.
                            {
                                Byte[] bf5 = Encoding.Default.GetBytes("0$Correct Answer!");
                                contestant.point = contestant.point + 1;
                                hash[contestant].Send(bf5); // Acknowledge the contestant (of their own answer).

                                Byte[] bf6 = Encoding.Default.GetBytes("0$" + contestant.name + " answered correctly!");
                                hash[client].Send(bf6); // Acknowledge others.

                                this.SetText(contestant.name + " answered correctly. (" + final + ")"); // Acknowledge the server.
                            }

                            else if (firstA != final) // If the answer is incorrect.
                            {
                                Byte[] bf7 = Encoding.Default.GetBytes("0$Incorrect Answer!");
                                hash[contestant].Send(bf7); // Acknowledge the contestant (of their own answer).

                                Byte[] bf8 = Encoding.Default.GetBytes("0$" + contestant.name + " answered incorrectly!");
                                hash[client].Send(bf8); // Acknowledge others.

                                this.SetText(contestant.name + " answered incorrectly.  (" + final + ")"); // Acknowledge the server.
                            }

                            else { this.SetText("ERROR"); }
                        }
                    }

                    string resultingresult = "";

                    foreach (Player results in playerList) // Forming the scoreboard (or updating).
                    {
                        resultingresult += results.name + " (" + results.point + ") | ";
                    }

                    foreach (Player playing in playerList) // Send the current scores to the clients.
                    {
                        try
                        {
                            Byte[] conclusion = Encoding.Default.GetBytes("0$Scoreboard => | " + resultingresult);
                            hash[playing].Send(conclusion);
                        }

                        catch
                        {
                            this.SetText("ERROR");
                            terminating = true;
                            serverSocket.Close();
                        }
                    }

                    receivecount = 0; // Initialize the count values back to zero.
                    questcount = 0;
                }

                if (gameover) { break; } // Check if the game is over.
            }

            foreach (Player answerer in playerList) // Deciding the winner based on results.
            {
                try
                {
                    if (gameover) // If only one player left before end of the game.
                    {
                        this.SetText("All other players left the game and " + answerer.name + " won the game!");
                        Byte[] gover = Encoding.Default.GetBytes("0$All other players left the game and " + answerer.name + " won the game!");
                        hash[answerer].Send(gover);
                    }

                    else
                    {
                        List<string> winnerlist = new List<string>();
                        winnerlist =  gamewinner(playerList);
                        string winners = "";

                        if (tie) // If there is a tie,
                        {
                            foreach (string word in winnerlist) // list all players with the same score (who tied).
                            {
                                winners += word + "\n";
                            }

                            this.SetText("There was a tie and winners are: \n" + winners);
                            Byte[] tied = Encoding.Default.GetBytes("0$There was a tie and winners are: \n" + winners);
                            hash[answerer].Send(tied);
                        }

                        else // If not.
                        {
                            this.SetText(winnerlist[0] + " is the winner!");
                            Byte[] street = Encoding.Default.GetBytes("0$"+ winnerlist[0] + " is the winner!");
                            hash[answerer].Send(street);
                        }
                    }
                }

                catch
                {
                    this.SetText("ERROR");
                    terminating = true;
                    serverSocket.Close();
                }
            }

            this.SetText("Game over.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connect_button_Click(object sender, EventArgs e)   // Connect button initiator.
        {
            int portNum = int.Parse(port_num.Text);

            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, portNum);   // Start server with the given port number.
                this.SetText("Server is ready.");
                this.SetText("There are not enough player to start the game.\nWaiting for players..\n---------------------");

                server_close.Enabled = true;
                connect_button.Enabled = false;

                serverSocket.Bind(endPoint);
                serverSocket.Listen(999);

                acceptThread = new Thread(new ThreadStart(Accept)); // Initiate a thread for "Accept" function in order to accept
                acceptThread.IsBackground = true;                   // any client that wants to connect, and start the thread.
                acceptThread.Start();
            }

            catch // If the server initiation fails.
            {
                this.SetText("ERROR");
            }
        }

        public List<string> gamewinner(List<Player> list)   // Acquires the player list and sorts it with respect to scores.
        {
            List<string> winners = new List<string>();      // In the sorted list, player who is at the end
            int element = (list.Count) - 2;                 // of it is the one with the highest score.

            playerList.Sort((a, b) => a.point.CompareTo(b.point));

            winners.Add(list[(list.Count)-1].name);

            int winnerspoint = list[list.Count - 1].point;

            for(;element >= 0;element--) // Check for each player.
            {
                if (winnerspoint == list[element].point) // If there are players who tied.
                {
                    winners.Add(list[element].name);
                    tie = true;
                }

                else { break; } // Breaks if there are no players who tied.     
            }

        return winners;
        }
    
        private void port_num_TextChanged(object sender, EventArgs e)
        {

        }

        private void result_box_TextChanged(object sender, EventArgs e)
        {

        }

        private void server_close_Click(object sender, EventArgs e) // To close the server, not very healthy.
        {
            this.SetText("Server has been terminated..");
            terminating = true;
            connect_button.Enabled = true;
            server_close.Enabled = false;
            this.Close();
        }

        private void clear_button_Click(object sender, EventArgs e) // To clear the results box (can get pretty messy after a few rounds).
        {
            result_box.Clear();
        }

        private void start_button_Click(object sender, EventArgs e) // Start game button, starts the game function as a thread.
        {
            gamestart = true;
            gameThread = new Thread(new ThreadStart(game));
            gameThread.IsBackground = true;
            gameThread.Start();
        }

        private void roundDisplay(int round)
        {
            this.Invoke((MethodInvoker)delegate { roundbox.Clear(); });
            string roundnum = round.ToString();
            this.SetRound(roundnum);
        }
    }
    public class Player : IComparable<Player> // Player class for storing all the informations about players (clients).
    {
        public String name;
        public String answer;
        public String question;
        public int point;
        public int connectedas;

        public int CompareTo(Player other)
        {
            return name.CompareTo(other.name);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Player;
            if (other == null) { return false; }
            return other.name == this.name;
        }
    }
}