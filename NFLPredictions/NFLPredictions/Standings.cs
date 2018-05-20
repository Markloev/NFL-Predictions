using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFLPredictions
{
    public partial class Standings : Form
    {
        public Boolean switchSort = false;

        public Standings()
        {
            InitializeComponent();
        }

        private void Image_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            string teamName = control.Name.ToString();
            TeamDetails teamDetailsForm = new TeamDetails(teamName);
            teamDetailsForm.ShowDialog();
        }

        private void RefreshStandings_Click(object sender, EventArgs e)
        {
            RefreshTeamStandings();
        }

        private void RefreshTeamStandings()
        {
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=nfl; username=root; password=QPalzmqp12; port=3306; SslMode=none"))
            {
                conn.Open();
                foreach (GroupBox g in this.Controls.OfType<GroupBox>())
                {
                    foreach (GroupBox gInner in g.Controls.OfType<GroupBox>())
                    {
                        Point firstImageLocation = new Point();
                        Point secondImageLocation = new Point();
                        Point thirdImageLocation = new Point();
                        Point fourthImageLocation = new Point();
                        Point firstLocation = new Point();
                        Point secondLocation = new Point();
                        Point thirdLocation = new Point();
                        Point fourthLocation = new Point();
                        int count = 0;
                        foreach (PictureBox teamImage in gInner.Controls.OfType<PictureBox>().OrderBy(x => x.Location.X))
                        {
                            if (count == 0) firstImageLocation = teamImage.Location;
                            else if (count == 1) secondImageLocation = teamImage.Location;
                            else if (count == 2) thirdImageLocation = teamImage.Location;
                            else if (count == 3) fourthImageLocation = teamImage.Location;
                            count++;
                        }
                        count = 0;
                        foreach (Label recordLabel in gInner.Controls.OfType<Label>().OrderBy(x => x.Location.X))
                        {
                            if (count == 0) firstLocation = recordLabel.Location;
                            else if (count == 1) secondLocation = recordLabel.Location;
                            else if (count == 2) thirdLocation = recordLabel.Location;
                            else if (count == 3) fourthLocation = recordLabel.Location;
                            count++;
                        }
                        Dictionary<string, Dictionary<int, int>> teamOrder = new Dictionary<string, Dictionary<int, int>>();
                        foreach (Label recordLabel in gInner.Controls.OfType<Label>().OrderBy(x => x.Location.X))
                        {
                            string teamName = recordLabel.Name.Replace("Record", "");

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

                            foreach (DataRow row in teams.Rows)
                            {
                                if (row.ItemArray[3] != DBNull.Value)
                                {
                                    if ((int)row.ItemArray[3] == teamID)
                                    {
                                        winningTotal++;
                                    }
                                    else
                                    {
                                        losingTotal++;
                                    }
                                }
                                rowCount++;
                            }
                            Dictionary<int, int> totals = new Dictionary<int, int>();
                            totals.Add(winningTotal, losingTotal);
                            teamOrder.Add(teamName, totals);
                            recordLabel.Text = winningTotal + " - " + losingTotal;
                        }
                        var teamList = teamOrder.ToList();
                        teamList = teamList.OrderBy(x => x.Value.Keys.First()).ThenByDescending(x => x.Value.Values.First()).ThenByDescending(x => x.Key).ToList();

                        int orderCount = 0;
                        foreach (KeyValuePair<string, Dictionary<int, int>> teamReorder in teamList)
                        {
                            foreach (Label record in gInner.Controls.OfType<Label>().OrderBy(x => x.Location.X))
                            {
                                string teamName = record.Name.Replace("Record", "");
                                if (teamReorder.Key == teamName)
                                {
                                    Control imageItem = gInner.Controls[teamName];
                                    if (orderCount == 0)
                                    {
                                        record.Location = fourthLocation;
                                        imageItem.Location = fourthImageLocation;
                                    }
                                    else if (orderCount == 1)
                                    {
                                        record.Location = thirdLocation;
                                        imageItem.Location = thirdImageLocation;
                                    }
                                    else if (orderCount == 2)
                                    {
                                        record.Location = secondLocation;
                                        imageItem.Location = secondImageLocation;
                                    }
                                    else if (orderCount == 3)
                                    {
                                        record.Location = firstLocation;
                                        imageItem.Location = firstImageLocation;
                                    }
                                }
                            }
                            orderCount++;
                        }
                    }
                }
            }
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            RefreshTeamStandings();
        }
    }
}
