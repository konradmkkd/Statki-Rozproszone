using System.Diagnostics;
using System;
using System.Windows.Forms;
using System.Net.Sockets;

namespace Statki_Rozproszone
{
    public partial class Statki : Form
    {
        public Statki()
        {
            InitializeComponent();
            RestartGame();

            

        }
        List<Button> playerPositionButtons;
        List<Button> enemyPositionButtons;

        List<Button> player1Positions = new List<Button>();
        List<Button> player2Positions = new List<Button>();


        List<Button> player1Hits = new List<Button>();
        List<Button> player2Hits = new List<Button>();
        List<Button> player1board = new List<Button>();
        List<Button> player2board = new List<Button>();

        List<Button> neighboursList = new List<Button>();
        List<Button> deactivatedButtons = new List<Button>();

     


        int totalShipis = 10;
        int player1Score;
        int player2Score;
        int kolej;
        bool pierwszyWybiera;
        bool czyWojna;


        private string buttonsToString(List<Button> lista)
        {
            string tempName = "";
            string buttonsNames = "";
            foreach (var button in lista)
            {
                tempName = button.Name as string;
                buttonsNames += tempName;
            }
            return buttonsNames;
        }

        private void RestartGame()
        {

         
            playerPositionButtons = new List<Button> { A1, A2, A3, A4, A5, B1, B2, B3, B4, B5, C1, C2, C3, C4, C5, D1, D2, D3, D4, D5, E1, E2, E3, E4, E5 };
            enemyPositionButtons = playerPositionButtons;


            txtHelp.Text = "1 Gracz wybiera miejsca na statki";
            for (int i = 0; i < enemyPositionButtons.Count; i++)
            {
                enemyPositionButtons[i].Enabled = true;
                enemyPositionButtons[i].Tag = " ";
                enemyPositionButtons[i].BackColor = Color.White;

            }
            for (int i = 0; i < playerPositionButtons.Count; i++)
            {
                playerPositionButtons[i].Enabled = true;
                playerPositionButtons[i].Tag = " ";

            }
            player1Score = 0;
            player2Score = 0;
            totalShipis = 10;
            kolej = -1;

            txtPlayer.Text = player1Score.ToString();
            txtEnemy.Text = player2Score.ToString();
     
            czyWojna = false;
            pierwszyWybiera = true;
            deactivatedButtons.Clear();
            neighboursList.Clear();
            player1Positions.Clear();
            player2Positions.Clear();


            btnChangePlayer.Enabled = false;

        }
        private List<Button> Neighborhood(Button btn)
        {
            List<Button> lista = new List<Button>();
            string name = btn.Name as string;

            char letter = name[0];
            char number = name[1];

            string[] neighbours = {(letter).ToString() + ((char)(number + 1)).ToString(),
                (letter).ToString() + ((char)(number - 1)).ToString(),
                ((char)(letter + 1)).ToString() + (number).ToString(),
                ((char)(letter - 1)).ToString() + (number).ToString()
            };
            foreach (Button obj in playerPositionButtons)
            {
                if (neighbours.Contains(obj.Name))
                    lista.Add(obj);     
            }
            return lista;
        }
      
        private void PlayerPositionButtonsEvent(object sender, EventArgs e)
        {
       

            var button = (Button)sender;
            kolej++;
             

            if (totalShipis > 0 && pierwszyWybiera)    //moment wybierania 1 gracza
            {

                //dodaj do kolejki statków gracza 1
                string tempName = "";
                if (totalShipis > 7)
                {
                    button.Enabled = false;
                    button.BackColor = Color.Red;
                    player1Positions.Add(button);
                    totalShipis -= 1;
                    tempName = button.Name as string;
                    List<Button> tempNeighbours = Neighborhood(button);
                    neighboursList.AddRange(tempNeighbours);

                    if(totalShipis == 7)
                    {
                        foreach (var btn in playerPositionButtons) // dzia³anie na ka¿dym nie zaznaczonym buttonie.
                        {

                            if (!player1Positions.Contains(btn))
                            {
                                btn.Enabled = true;
                                btn.BackColor = Color.White;
                            }
                        }

                        foreach (var neighbour in neighboursList) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
                        {
                            if (!player1Positions.Contains(neighbour))
                            {
                                deactivatedButtons.Add(neighbour);
                                neighbour.Enabled = false;
                                neighbour.BackColor = Color.LightGray;
                            }

                        }
                        neighboursList.Clear();
                    }
                    else
                    {
                        foreach (var btn in playerPositionButtons)
                        {

                            if (!player1Positions.Contains(btn))
                            {
                                btn.Enabled = false;
                                btn.BackColor = Color.LightGray;
                            }
                        }

                        foreach (var neighbour in neighboursList)
                        {
                            if (!player1Positions.Contains(neighbour))
                            {
                                neighbour.Enabled = true;
                                neighbour.BackColor = Color.White;
                            }

                        }
                    }
                    
                    //stwórz s¹siedztwo
                    //wyœwietl tylko s¹siedztwo
                }

                if (totalShipis <= 7 && totalShipis >0 && kolej >=3)
                {
                    button.Enabled = false;
                    button.BackColor = Color.Red;
                    player1Positions.Add(button);
                    tempName = button.Name as string;
                    List<Button> tempNeighbours = Neighborhood(button);
                    neighboursList.AddRange(tempNeighbours);
                    
                    if (totalShipis == 6 || totalShipis<=4)
                    {
                        foreach (var btn in playerPositionButtons) // dzia³anie na ka¿dym nie zaznaczonym buttonie.
                        {

                            if (!player1Positions.Contains(btn))
                            {
                                btn.Enabled = true;
                                btn.BackColor = Color.White;
                            }
                        }

                        foreach (var neighbour in neighboursList) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
                        {
                            if (!player1Positions.Contains(neighbour))
                            {
                                deactivatedButtons.Add(neighbour);
                                neighbour.Enabled = false;
                                neighbour.BackColor = Color.LightGray;
                            }

                        }
                        neighboursList.Clear();
                        foreach (var deactivated in deactivatedButtons)
                        {
                            deactivated.Enabled = false;
                            deactivated.BackColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        foreach (var btn in playerPositionButtons)
                        {

                            if (!player1Positions.Contains(btn))    // dzia³anie na ka¿dym nie zaznaczonym buttonie.
                            {
                                btn.Enabled = false;
                                btn.BackColor = Color.LightGray;
                            }
                        }

                        foreach (var neighbour in neighboursList)
                        {
                            if (!player1Positions.Contains(neighbour) && !deactivatedButtons.Contains(neighbour)) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
                            {
                                neighbour.Enabled = true;
                                neighbour.BackColor = Color.White;
                            }

                        }
                    }
                    totalShipis -= 1;

                    if (totalShipis == 0)
                    {
                        txtHelp.Text = "2 Gracz wybiera miejsca na statki";
                        string statkiPlayer1 = "";
                        foreach (var item in player1Positions)
                        {
                            statkiPlayer1 = statkiPlayer1 + item.Text;
                            item.BackColor = Color.White;
                            item.Enabled = true;
                        }
                        foreach (var item in deactivatedButtons) //lub neighbourButtons
                        {
                            statkiPlayer1 = statkiPlayer1 + item.Text;
                            item.BackColor = Color.White;
                            item.Enabled = true;
                        }
                        foreach (var item in neighboursList) //lub neighbourButtons
                        {
                            statkiPlayer1 = statkiPlayer1 + item.Text;
                            item.BackColor = Color.White;
                            item.Enabled = true;
                        }
                        txtPlayer.Text = statkiPlayer1;

                       
                        pierwszyWybiera = false;
                        totalShipis = 10;
                        deactivatedButtons.Clear();
                        neighboursList.Clear();

                        txtPlayer.Text = statkiPlayer1;
                        txtHelp.Text = "1 Gracz strzela!";
                        czyWojna = true;
                    }

                }  
            }
            #region 2 gracz wybiera, ale to drill
            //if (totalShipis > 0 && !pierwszyWybiera && kolej>=10)    //moment wybierania 2 gracza
            //{

            //    string tempName = "";
            //    if (totalShipis > 7)
            //    {
            //        button.Enabled = false;
            //        button.BackColor = Color.Red;
            //        totalShipis -= 1;
            //        player2Positions.Add(button);
            //        tempName = button.Name as string;
            //        List<Button> tempNeighbours = Neighborhood(button);
            //        neighboursList.AddRange(tempNeighbours);

            //        if (totalShipis == 7)
            //        {
            //            foreach (var btn in playerPositionButtons) // dzia³anie na ka¿dym nie zaznaczonym buttonie.
            //            {

            //                if (!player2Positions.Contains(btn))
            //                {
            //                    btn.Enabled = true;
            //                    btn.BackColor = Color.White;
            //                }
            //            }

            //            foreach (var neighbour in neighboursList) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
            //            {
            //                if (!player2Positions.Contains(neighbour))
            //                {
            //                    deactivatedButtons.Add(neighbour);
            //                    neighbour.Enabled = false;
            //                    neighbour.BackColor = Color.LightGray;
            //                }

            //            }
            //            neighboursList.Clear();
            //        }
            //        else
            //        {
            //            foreach (var btn in playerPositionButtons)
            //            {

            //                if (!player2Positions.Contains(btn))
            //                {
            //                    btn.Enabled = false;
            //                    btn.BackColor = Color.LightGray;
            //                }
            //            }

            //            foreach (var neighbour in neighboursList)
            //            {
            //                if (!player2Positions.Contains(neighbour))
            //                {
            //                    neighbour.Enabled = true;
            //                    neighbour.BackColor = Color.White;
            //                }

            //            }
            //        }

            //        //stwórz s¹siedztwo
            //        //wyœwietl tylko s¹siedztwo
            //    }

            //    if (totalShipis <= 7 && totalShipis > 0 && kolej >= 13)
            //    {
            //        button.Enabled = false;
            //        button.BackColor = Color.Red;

            //        player2Positions.Add(button);
            //        tempName = button.Name as string;
            //        List<Button> tempNeighbours = Neighborhood(button);
            //        neighboursList.AddRange(tempNeighbours);

            //        if (totalShipis == 6 || totalShipis <= 4)
            //        {
            //            foreach (var btn in playerPositionButtons) // dzia³anie na ka¿dym nie zaznaczonym buttonie.
            //            {

            //                if (!player2Positions.Contains(btn))
            //                {
            //                    btn.Enabled = true;
            //                    btn.BackColor = Color.White;
            //                }
            //            }

            //            foreach (var neighbour in neighboursList) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
            //            {
            //                if (!player2Positions.Contains(neighbour))
            //                {
            //                    deactivatedButtons.Add(neighbour);
            //                    neighbour.Enabled = false;
            //                    neighbour.BackColor = Color.LightGray;
            //                }

            //            }
            //            neighboursList.Clear();
            //            foreach (var deactivated in deactivatedButtons)
            //            {
            //                deactivated.Enabled = false;
            //                deactivated.BackColor = Color.LightGray;
            //            }
            //        }
            //        else
            //        {
            //            foreach (var btn in playerPositionButtons)
            //            {

            //                if (!player2Positions.Contains(btn))    // dzia³anie na ka¿dym nie zaznaczonym buttonie.
            //                {
            //                    btn.Enabled = false;
            //                    btn.BackColor = Color.LightGray;
            //                }
            //            }

            //            foreach (var neighbour in neighboursList)
            //            {
            //                if (!player2Positions.Contains(neighbour) && !deactivatedButtons.Contains(neighbour)) //dzia³anie na ka¿dym s¹siedzie zaznaczonych buttonów.
            //                {
            //                    neighbour.Enabled = true;
            //                    neighbour.BackColor = Color.White;
            //                }

            //            }
            //        }
            //        totalShipis -= 1;

            //        if (totalShipis == 0)
            //        {
            //            string statkiPlayer1 = "";
            //            foreach (var item in player2Positions)
            //            {
            //                statkiPlayer1 = statkiPlayer1 + item.Text;
            //                item.BackColor = Color.White;
            //                item.Enabled = true;
            //            }
            //            foreach (var item in deactivatedButtons)
            //            {
            //                item.Enabled = true;
            //                item.BackColor = Color.White;
            //            }
            //            txtPlayer.Text = statkiPlayer1;
            //            txtHelp.Text = "1 Gracz strzela!";
            //            deactivatedButtons.Clear();
            //            neighboursList.Clear();
            //            czyWojna = true;

            //        }

            //    }
            //}
            #endregion


            // gdy gracz wybiera to powinien zatwierdziæ to buttonem,
            // a nastêpnie dostaæ odpowiedŸ od serwera -
            // czy 2 gracz wybra³ ju¿ statki,
            // jeœli nie - wszystkie buttony s¹ deactivated i napis jest "oczekiwanie na 2 klienta"
            // jeœli tak - buttony siê aktywuj¹ u gracza który 1 strzela, wybrane buttony zapisuj¹ siê u przeciwnika, aby na cliencie by³y sprawdzane, a by³a tylko przekazywana informacja w co zosta³o strzelone
            // 


            if (/*kolej % 2==0 &&*/ czyWojna /*&& kolej>=20*/)   //ruch 1 gracza
            {
                btnChangePlayer.Enabled = true;
                foreach (var item in player2Positions)
                {
                    if (item.Text == button.Text)
                    {
                        string tempTag = button.Tag as string;
                        tempTag = tempTag + 'x';
                        button.Tag = tempTag;
                        button.BackColor= Color.BlueViolet;
                        player1Score++;
                        player1board.Add(button);
             
                        //ShowBoard(player2board, player1board);
                        break;

                    }
                    else
                    {
                        string tempTag = button.Tag as string;
                        tempTag = tempTag + 'o';
                        button.Tag = tempTag;
                        button.BackColor = Color.Green;
                        player1board.Add(button);
                 
                        //ShowBoard(player2board, player1board);
                    }
                }


                //ustawianie kolejnej tury
                txtHelp.Text = "2 Gracz strzela!";



                foreach (var buttons in playerPositionButtons)
                {
                    buttons.Enabled = false;
                }
                if (player1Score == 10)
                {
                    txtHelp.Text = "1 GRACZ ZWYCIÊ¯A";
                    const string message = "Gracz 1 zwyciê¿a";
                    const string caption = "Zrestartuj grê";
                    var result = MessageBox.Show(message, caption, MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        RestartGame();
                    }
                }
                

            }

            if (kolej % 2 != 0 && czyWojna && kolej>20)
            {
               
                player2Hits.Add(button);
                btnChangePlayer.Enabled = true;

                foreach (var item in player1Positions)
                {
                    if (item.Text == button.Text)
                    {
                        string tempTag = button.Tag as string;
                        tempTag = tempTag + 'X';
                        button.Tag = tempTag;

                        button.BackColor = Color.BlueViolet;

                        player2Score++;
                        player2board.Add(button);
                 
                        //ShowBoard(player1board, player2board);
                        break;
                    }
                    else
                    {
                        string tempTag = button.Tag as string;
                        tempTag = tempTag + 'O';
                        button.Tag = tempTag;

                        button.BackColor = Color.Green;
                        player2board.Add(button);

                        //ShowBoard(player1board, player2board);

                    }
                }
                //System.Threading.Thread.Sleep(1000);
                foreach (var buttons in playerPositionButtons)
                {
                    buttons.Enabled = false;
                }
                txtHelp.Text = "1 Gracz strzela!";
                if (player2Score == 10)
                {
                    txtHelp.Text = "2 GRACZ ZWYCIÊ¯A";
                    const string message = "Gracz 2 zwyciê¿a";
                    const string caption = "Zrestartuj grê";
                    var result = MessageBox.Show(message, caption, MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        RestartGame();
                    }
                }
                
            }
            textBox3.Text = "K: " + kolej.ToString() + " T: " + totalShipis.ToString();

        }
        private void ShowBoard(List<Button> toShow)
        {
            ClearTheBoard();

            if (kolej % 2 == 1)
            {
                foreach (var item in toShow)
                {
                    if (item.Tag.ToString().Contains('o'))
                    {
                        item.BackColor = Color.Green;
                        item.Enabled = false;
                    }
                    if (item.Tag.ToString().Contains('x'))
                    {
                        item.BackColor = Color.BlueViolet;
                        item.Enabled = false;
                    }
                }
            }
            else
            {
                foreach (var item in toShow)
                {
                    if (item.Tag.ToString().Contains('O'))
                    {
                        item.BackColor = Color.Green;
                        item.Enabled = false;
                    }
                    if (item.Tag.ToString().Contains('X'))
                    {
                        item.BackColor = Color.BlueViolet;
                        item.Enabled = false;
                    }
                }
            }
            
        }
        private void ClearTheBoard()
        {
            foreach (var button in playerPositionButtons)
            {
                button.BackColor = Color.White;
                button.Enabled = true;
            }
        }
        
       

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender,e);

        }

        private void Statki_Load(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button17_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button18_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button19_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button20_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button11_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button12_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button14_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }


        private void B1_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void A2_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void A5_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void B2_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void B5_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void C1_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void C2_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void C5_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void D1_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void D2_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void D5_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void E1_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void E2_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void E5_Click(object sender, EventArgs e)
        {
            PlayerPositionButtonsEvent(sender, e);

        }

        private void txtHelp_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    
        private void button1_Click_1(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (kolej % 2 != 0)
            {
                ShowBoard(player1board);
            }
            if (kolej % 2 == 0)
            {
                ShowBoard(player2board);
            }
            btnChangePlayer.Enabled = false;
        }

        private void txtPlayer_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 8080);
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                String s = String.Empty;
                while (!s.Equals("Exit"))
                {
                    Console.Write("Enter a string to send to the server: ");
                    //s = Console.ReadLine();
                    //Console.WriteLine();

                    if (player1Positions.Count > 0)
                    {
                        s = buttonsToString(player1Positions);
                        
                    }
                    else if (player2Positions.Count>0)
                    {
                        s = buttonsToString(player2Positions);
                    }
                    else
                    {
                        s = "wiadomosc do serwera";
                    }
                    writer.WriteLine(s);
                    writer.Flush();

                    //wiadomoœæ od servera
                    String server_string = reader.ReadLine();
                    txtHelp.Text = server_string;
                    Console.WriteLine(server_string);
                    Console.ReadKey();
                }
                reader.Close();
                writer.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}