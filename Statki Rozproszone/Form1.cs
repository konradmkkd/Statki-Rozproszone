using System.Diagnostics;
using System;


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


 

        Random rand = new Random();
        int totalShipis = 3;
        int round = 10;
        int player1Score;
        int player2Score;
        int kolej;
        bool czyWojna;


        private void RestartGame()
        {


            playerPositionButtons = new List<Button> { A1, A2, A3, A4, A5, B1, B2, B3, B4, B5, C1, C2, C3, C4, C5, D1, D2, D3, D4, D5, E1, E2, E3, E4, E5 };
            enemyPositionButtons = playerPositionButtons;

            EnemyLocationListBox.Items.Clear();
            EnemyLocationListBox.Text = null;
          

            txtHelp.Text = "1 Gracz wybiera miejsca na statki";
            for (int i = 0; i < enemyPositionButtons.Count; i++)
            {
                enemyPositionButtons[i].Enabled = true;
                enemyPositionButtons[i].Tag = null;
                enemyPositionButtons[i].BackColor = Color.White;
                EnemyLocationListBox.Items.Add(enemyPositionButtons[i].Text);



            }
            for (int i = 0; i < playerPositionButtons.Count; i++)
            {
                playerPositionButtons[i].Enabled = true;
                playerPositionButtons[i].Tag = null;

            }
            player1Score = 0;
            player2Score = 0;
            round = 10;
            totalShipis = 3;
            kolej = -1;

            txtPlayer.Text = player1Score.ToString();
            txtEnemy.Text = player2Score.ToString();
            enemyMove.Text = "A1";
            czyWojna = false;

            btnAttack.Enabled = false;
            enemyLocationPicker();

        }

        private void enemyLocationPicker()
        {
            for (int i = 0; i < 3; i++)
            {
                int index = rand.Next(enemyPositionButtons.Count);
                if (enemyPositionButtons[index].Enabled == true && (string)enemyPositionButtons[index].Tag == null)
                {
                    enemyPositionButtons[index].Tag = "enemyShip";
                    Debug.WriteLine("Enemy position: " + enemyPositionButtons[index].Text);
                }
                else
                {
                    index = rand.Next(enemyPositionButtons.Count);
                }
                
            }
        }
        private void PlayerPositionButtonsEvent(object sender, EventArgs e)
        {
            var button = (Button)sender;
            kolej++;
            if (totalShipis > 0)    //moment wybierania 1 gracza
            {
                button.Enabled = false;
                //button.Tag = "playerShip";
                button.BackColor = Color.Red;

                player1Positions.Add(button);

                totalShipis -= 1;
                if (totalShipis == 0)
                {
                    btnAttack.Enabled = true;
                    btnAttack.BackColor = Color.Green;
                    btnAttack.ForeColor = Color.Blue;

                    txtHelp.Text = "2 Gracz wybiera miejsca na statki";
                    string statkiPlayer1 = "";
                    foreach (var item in player1Positions)
                    {
                        statkiPlayer1 = statkiPlayer1 + item.Text;
                        item.BackColor = Color.White;
                        item.Enabled = true;
                    }
                    txtPlayer.Text = statkiPlayer1;
                }
            }
            else
            {
                if (totalShipis > -3)   //moment wybierania drugiego gracza
                {
                    button.Enabled = false;
                    //button.Tag = "playerShip";
                    button.BackColor = Color.Red;

                    player2Positions.Add(button);

                    totalShipis -= 1;
                    if (totalShipis == -3)
                    {
                        btnAttack.Enabled = true;
                        btnAttack.BackColor = Color.Green;
                        btnAttack.ForeColor = Color.Blue;

                        string statkiPlayer1 = "";
                        foreach (var item in player2Positions)
                        {
                            statkiPlayer1 = statkiPlayer1 + item.Text;
                            item.BackColor = Color.White;
                            item.Enabled = true;
                        }
                        txtEnemy.Text = statkiPlayer1;
                        txtHelp.Text = "1 Gracz strzela!";
                        czyWojna = true;
                    }




                }
            }

            //tutaj po wybraniu statku, mo¿na robiæ kolejne klikniêcia, które s¹ rollowane za pomoc¹ zmiennej `kolej`
            if (czyWojna) txtRounds.Text = kolej.ToString();

            if (kolej>=6 && kolej % 2==0 && czyWojna)   //ruch 1 gracza
            {
                foreach (var item in player2Positions)
                {
                    if (item.Text == button.Text)
                    {
                        button.Tag = "hit";
                        button.BackColor= Color.BlueViolet;
                        player1Score++;
                        break;

                    }
                    else
                    {
                        button.Tag = "miss";
                        button.BackColor = Color.Green;


                    }
                }
                player1Hits.Add(button);

                //System.Threading.Thread.Sleep(1000);


                //ustawianie kolejnej tury
                txtHelp.Text = "2 Gracz strzela!";
                //button.BackColor = Color.White;

                 


                if (player1Score == 3) txtHelp.Text = "1 GRACZ ZWYCIÊ¯A";

            }

            if (totalShipis == -3 && kolej % 2 != 0 && czyWojna && kolej>6)
            {

                player2Hits.Add(button);
                foreach (var item in player1Positions)
                {
                    if (item.Text == button.Text)
                    {
                        button.Tag = "hit";
                        button.BackColor = Color.BlueViolet;
                        player2Score++;
                        break;
                    }
                    else
                    {
                        button.Tag = "miss";
                        button.BackColor = Color.Green;
                        

                    }
                }
                //System.Threading.Thread.Sleep(1000);

                txtHelp.Text = "1 Gracz strzela!";
                if (player2Score == 3) txtHelp.Text = "2 GRACZ ZWYCIÊ¯A";
            }

        }
           
           
        
        private void EnemyPlayTimerEvent(object sender, EventArgs e)
        {

            if (playerPositionButtons.Count > 0 && round > 0)
            {
                round -= 1;
                txtRounds.Text = "Round: " + round;
                int index = rand.Next(playerPositionButtons.Count);
                if ((string)playerPositionButtons[index].Tag == "playerShip")
                {
                    playerPositionButtons[index].BackColor = Color.Yellow;
                    enemyMove.Text = playerPositionButtons[index].Text;
                    playerPositionButtons[index].Enabled = false;
                    playerPositionButtons.RemoveAt(index);
                    player2Score += 1;
                    txtEnemy.Text = player2Score.ToString();
                    EnemyPlayTimer.Stop();
                }
                else
                {
                    playerPositionButtons[index].BackColor = Color.Pink;
                    enemyMove.Text = playerPositionButtons[index].Text;
                    playerPositionButtons[index].Enabled = false;
                    playerPositionButtons.RemoveAt(index);
                    EnemyPlayTimer.Stop();
                }
            }
            if (round < 1 || player2Score > 2 || player1Score > 2)
            {
                if (player1Score > player2Score)
                {
                    MessageBox.Show("You win", "Winning");
                    RestartGame();
                }
                else if (player2Score > player1Score)
                {
                    MessageBox.Show("Zatopiony", "Lost");
                    RestartGame();
                }
                else if (player2Score == player1Score)
                {
                    MessageBox.Show("No one wins this game", "Draw");
                    RestartGame();
                }
            }

        }
        private void player1Name_Click(object sender, EventArgs e)
        {

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

        private void btnAttack_Click(object sender, EventArgs e)
        {
            //foreach (var item in playerPositionButtons)
            //{
            //    foreach (var zaznaczone in player1Positions)
            //    {
            //        if (item.Tag == zaznaczone.Tag)
            //        {
            //            Console.WriteLine("zaznaczony: "+ zaznaczone);
            //            item.BackColor = Color.Black;
            //        }

            //    }
            //}

            AttackButtonEvent(sender, e);

        }
        private void AttackButtonEvent(object sender, EventArgs e)
        {
            if (EnemyLocationListBox.Text != "")
            {
                var attackPosition = EnemyLocationListBox.Text.ToLower();
                int index = enemyPositionButtons.FindIndex(a => a.Name == attackPosition);
                if (enemyPositionButtons[index].Enabled && round > 0)
                {
                    round -= 1;
                    txtRounds.Text = "Round: " + round;
                    if ((string)enemyPositionButtons[index].Tag == "enemyShip")
                    {
                        enemyPositionButtons[index].Enabled = false;
                        enemyPositionButtons[index].BackColor = Color.Yellow;
                        player1Score += 1;
                        txtPlayer.Text = player1Score.ToString();
                        EnemyPlayTimer.Start();
                    }
                    else
                    {
                        enemyPositionButtons[index].Enabled = false;
                        enemyPositionButtons[index].BackColor = Color.DarkBlue;
                        EnemyPlayTimer.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Choose a location from the drop down first", "Information");
            }
        }
    }
}