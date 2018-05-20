using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace NFLPredictions
{
    public partial class TeamDetails : Form
    {
        public TeamDetails(string teamName)
        {
            InitializeComponent();
            this.TeamGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TeamGrid_MouseDown);
            SetLabel(teamName);
            FillGridView(teamName);
            SetDimensions();
        }

        private void SetLabel(string teamName)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=nfl; username=root; password=QPalzmqp12; port=3306; SslMode=none"))
            {
                conn.Open();
                string query = "SELECT Team FROM Teams WHERE Team LIKE '%" + teamName + "%'";
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(query, conn);
                DataTable teams = new DataTable();
                sqlAdapter.Fill(teams);
                TeamName.Text = teams.Rows[0].ItemArray[0].ToString();
            }
        }

        private void FillGridView(string teamName)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=nfl; username=root; password=QPalzmqp12; port=3306; SslMode=none"))
            {
                conn.Open();
                string query = "SELECT TeamID FROM Teams WHERE Team LIKE '%" + teamName + "%'";
                MySqlCommand command = new MySqlCommand(query, conn);
                int teamID = Convert.ToInt32(command.ExecuteScalar());

                query = "SELECT Week, CASE WHEN HomeTeamID = " + teamID + " THEN 'Home' WHEN Neutral = true THEN 'Neutral' ELSE 'Away' END AS Location, CASE WHEN HomeTeamID = " + teamID + " THEN (SELECT Team FROM Teams WHERE TeamID = AwayTeamID ) ELSE (SELECT Team FROM Teams WHERE TeamID = HomeTeamID) END AS Opponent, WinningTeamID " +
                    "FROM Predictions WHERE HomeTeamID = " + teamID + " OR AwayTeamID = " + teamID + " ORDER BY Week";
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(query, conn);
                DataTable teams = new DataTable();
                sqlAdapter.Fill(teams);
                int rowCount = 0;
                int winningTotal = 0;
                int losingTotal = 0;

                foreach(DataRow row in teams.Rows)
                {
                    TeamGrid.Rows.Add(row.ItemArray[0], row.ItemArray[2], row.ItemArray[1]);
                    if (row.ItemArray[3] != DBNull.Value)
                    {
                        if ((int)row.ItemArray[3] == teamID)
                        {
                            TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor = Color.LightGreen;
                            winningTotal++;
                        }
                        else
                        {
                            TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor = Color.PaleVioletRed;
                            losingTotal++;
                        }
                    }
                    rowCount++;
                }

                Record.Text = winningTotal + " - " + losingTotal;
            }
        }

        private void SetDimensions()
        {
            int height = 0;
            foreach (DataGridViewRow row in TeamGrid.Rows)
            {
                height += row.Height;
            }
            height += TeamGrid.ColumnHeadersHeight;

            int width = 0;
            foreach (DataGridViewColumn col in TeamGrid.Columns)
            {
                width += col.Width;
            }
            width += TeamGrid.RowHeadersWidth;

            TeamGrid.ClientSize = new Size(width + 2, height + 2);
        }

        private void SaveChanges_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=nfl; username=root; password=QPalzmqp12; port=3306; SslMode=none"))
            {
                conn.Open();
                string query = "SELECT TeamID FROM Teams WHERE Team LIKE '%" + TeamName.Text + "%'";
                MySqlCommand command = new MySqlCommand(query, conn);
                int teamID = Convert.ToInt32(command.ExecuteScalar());

                int rowCount = 0;
                foreach (DataGridViewRow row in TeamGrid.Rows)
                {
                    query = "SELECT TeamID FROM Teams WHERE Team LIKE '%" + row.Cells[1].Value + "%'";
                    command = new MySqlCommand(query, conn);
                    int opponentTeamID = Convert.ToInt32(command.ExecuteScalar());
                    int week = (int)row.Cells[0].Value;
                    int winningTeamID = 0;
                    if (TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor == Color.LightGreen)
                    {
                        winningTeamID = teamID;
                    }
                    else if (TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor == Color.PaleVioletRed)
                    {
                        winningTeamID = opponentTeamID;
                    }
                    rowCount++;

                    query = "UPDATE Predictions SET WinningTeamID=@WinningTeamID WHERE Season=2018 AND Week=@Week AND (HomeTeamID=@TeamID OR AwayTeamID=@TeamID)";
                    command = new MySqlCommand(query, conn);
                    if (winningTeamID == 0)
                    {
                        command.Parameters.AddWithValue("@WinningTeamID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@WinningTeamID", winningTeamID);
                    }
                    command.Parameters.AddWithValue("@Week", week);
                    command.Parameters.AddWithValue("@TeamID", teamID);
                    command.ExecuteNonQuery();
                }
            }
            this.Close();
        }

        private void TeamGrid_MouseDown(object sender, MouseEventArgs e)
        {
            var hti = TeamGrid.HitTest(e.X, e.Y);
            if (hti.RowIndex != -1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    TeamGrid.Rows[hti.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    TeamGrid.Rows[hti.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    TeamGrid.Rows[hti.RowIndex].DefaultCellStyle.BackColor = Color.PaleVioletRed;
                }
            }
            UpdateWinningTotal();
        }

        private void UpdateWinningTotal()
        {
            int winningTotal = 0;
            int losingTotal = 0;
            int rowCount = 0;
            foreach (DataGridViewRow row in TeamGrid.Rows)
            {                
                if (TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor == Color.LightGreen)
                {
                    winningTotal++;
                }
                else if (TeamGrid.Rows[rowCount].DefaultCellStyle.BackColor == Color.PaleVioletRed)
                {
                    losingTotal++;
                }
                rowCount++;
            }
            Record.Text = winningTotal + " - " + losingTotal;
        }

        private void TeamGrid_SelectionChanged(object sender, EventArgs e)
        {
            TeamGrid.ClearSelection();
        }

        private void TeamDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
