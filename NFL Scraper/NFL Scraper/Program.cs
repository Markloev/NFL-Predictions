using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace NFL_Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient w = new WebClient();
            Dictionary<string, List<Schedule>> schedule = new Dictionary<string, List<Schedule>>();
            Dictionary<string, string> sites = new Dictionary<string, string>();
            sites.Add("Arizona Cardinals", "http://www.fbschedules.com/nfl-18/2018-arizona-cardinals-football-schedule.php");
            sites.Add("Atlanta Falcons", "http://www.fbschedules.com/nfl-18/2018-atlanta-falcons-football-schedule.php");
            sites.Add("Baltimore Ravens", "http://www.fbschedules.com/nfl-18/2018-baltimore-ravens-football-schedule.php");
            sites.Add("Buffalo Bills", "http://www.fbschedules.com/nfl-18/2018-buffalo-bills-football-schedule.php");
            sites.Add("Carolina Panthers", "http://www.fbschedules.com/nfl-18/2018-carolina-panthers-football-schedule.php");
            sites.Add("Chicago Bears", "http://www.fbschedules.com/nfl-18/2018-chicago-bears-football-schedule.php");
            sites.Add("Cincinnati Bengals", "http://www.fbschedules.com/nfl-18/2018-cincinnati-bengals-football-schedule.php");
            sites.Add("Cleveland Browns", "http://www.fbschedules.com/nfl-18/2018-cleveland-browns-football-schedule.php");
            sites.Add("Dallas Cowboys", "http://www.fbschedules.com/nfl-18/2018-dallas-cowboys-football-schedule.php");
            sites.Add("Denver Broncos", "http://www.fbschedules.com/nfl-18/2018-denver-broncos-football-schedule.php");
            sites.Add("Detroit Lions", "http://www.fbschedules.com/nfl-18/2018-detroit-lions-football-schedule.php");
            sites.Add("Green Bay Packers", "http://www.fbschedules.com/nfl-18/2018-green-bay-packers-football-schedule.php");
            sites.Add("Houston Texans", "http://www.fbschedules.com/nfl-18/2018-houston-texans-football-schedule.php");
            sites.Add("Indianapolis Colts", "http://www.fbschedules.com/nfl-18/2018-indianapolis-colts-football-schedule.php");
            sites.Add("Jacksonville Jaguars", "http://www.fbschedules.com/nfl-18/2018-jacksonville-jaguars-football-schedule.php");
            sites.Add("Kansas City Chiefs", "http://www.fbschedules.com/nfl-18/2018-kansas-city-chiefs-football-schedule.php");
            sites.Add("Los Angeles Rams", "http://www.fbschedules.com/nfl-18/2018-los-angeles-rams-football-schedule.php");
            sites.Add("Los Angeles Chargers", "http://www.fbschedules.com/nfl-18/2018-los-angeles-chargers-football-schedule.php");
            sites.Add("Miami Dolphins", "http://www.fbschedules.com/nfl-18/2018-miami-dolphins-football-schedule.php");
            sites.Add("Minnesota Vikings", "http://www.fbschedules.com/nfl-18/2018-minnesota-vikings-football-schedule.php");
            sites.Add("New England Patriots", "http://www.fbschedules.com/nfl-18/2018-new-england-patriots-football-schedule.php");
            sites.Add("New Orleans Saints", "http://www.fbschedules.com/nfl-18/2018-new-orleans-saints-football-schedule.php");
            sites.Add("New York Giants", "http://www.fbschedules.com/nfl-18/2018-new-york-giants-football-schedule.php");
            sites.Add("New York Jets", "http://www.fbschedules.com/nfl-18/2018-new-york-jets-football-schedule.php");
            sites.Add("Oakland Raiders", "http://www.fbschedules.com/nfl-18/2018-oakland-raiders-football-schedule.php");
            sites.Add("Philadelphia Eagles", "http://www.fbschedules.com/nfl-18/2018-philadelphia-eagles-football-schedule.php");
            sites.Add("Pittsburgh Steelers", "http://www.fbschedules.com/nfl-18/2018-pittsburgh-steelers-football-schedule.php");
            sites.Add("San Francisco 49ers", "http://www.fbschedules.com/nfl-18/2018-san-francisco-49ers-football-schedule.php");
            sites.Add("Seattle Seahawks", "http://www.fbschedules.com/nfl-18/2018-seattle-seahawks-football-schedule.php");
            sites.Add("Tampa Bay Buccaneers", "http://www.fbschedules.com/nfl-18/2018-tampa-bay-buccaneers-football-schedule.php");
            sites.Add("Tennessee Titans", "http://www.fbschedules.com/nfl-18/2018-tennessee-titans-football-schedule.php");
            sites.Add("Washington Redskins", "http://www.fbschedules.com/nfl-18/2018-washington-redskins-football-schedule.php");
            foreach (KeyValuePair<string, string> site in sites)
            {
                List<Schedule> scheduleItems = new List<Schedule>();
                string s = w.DownloadString(site.Value);
                s = s.Substring(s.IndexOf("<table"));
                s = s.Substring(0, s.LastIndexOf("</table>"));
                s += "</table>";
                s = s.Replace("<strong><strong>", "<strong>").Replace("</strong></strong>", "</strong>");
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(s);
                int teamCount = Regex.Matches(doc.Text, "Preseason").Count;
                int avoidNodesTotal = 5;
                if(teamCount > 8)
                {
                    avoidNodesTotal = 6;
                    teamCount = 10;
                }
                var nodes = doc.DocumentNode.SelectNodes("//tr");
                int avoidNodesCount = 0;
                int weekCount = 1;
                foreach (HtmlNode node in nodes)
                {
                    if (avoidNodesCount > avoidNodesTotal)
                    {
                        var details = node.SelectNodes("//strong")[teamCount];
                        string homeAway = "";
                        if (!details.InnerText.Contains("BYE"))
                        {
                            if (details.InnerText.Contains("at "))
                            {
                                homeAway = "Away";
                            }
                            else
                            {
                                homeAway = "Home";
                            }
                            Schedule item = new Schedule();
                            item.Week = weekCount;
                            item.HomeAway = homeAway;
                            item.Opponent = details.InnerText.Split(' ').Last();
                            scheduleItems.Add(item);
                            teamCount += 2;
                        }
                        else
                        {
                            teamCount += 1;
                        }
                        weekCount++;
                    }
                    avoidNodesCount++;
                }
                schedule[site.Key] =  scheduleItems;
            }
            using (MySqlConnection conn = new MySqlConnection("server=localhost; database=nfl; username=root; password=QPalzmqp12; port=3306; SslMode=none"))
            {
                conn.Open();
                foreach (KeyValuePair<string, List<Schedule>> team in schedule)
                {
                    string teamName = team.Key;
                    foreach (Schedule scheduleItem in schedule[team.Key])
                    {
                        string homeAway = scheduleItem.HomeAway;
                        string opponent = scheduleItem.Opponent;
                        int week = scheduleItem.Week;
                        string query = "SELECT TeamID FROM Teams WHERE Team LIKE '%" + teamName + "%' UNION SELECT TeamID FROM Teams WHERE Team LIKE '%" + opponent + "%'";
                        MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(query, conn);
                        DataTable teams = new DataTable();
                        sqlAdapter.Fill(teams);
                        int teamID1 = (int)teams.Rows[0].ItemArray[0];
                        int teamID2 = (int)teams.Rows[1].ItemArray[0];
                        query = "SELECT COUNT(*) FROM Predictions WHERE Season = 2018 AND Week = " + week + " AND (HomeTeamID = " + teamID1 + " OR AwayTeamID = " + teamID1 + ")";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        if (count == 0)
                        {
                            int homeTeamID = 0;
                            int awayTeamID = 0;
                            if (homeAway == "Home")
                            {
                                homeTeamID = teamID1;
                                awayTeamID = teamID2;
                            }
                            else if (homeAway == "Away")
                            {
                                homeTeamID = teamID2;
                                awayTeamID = teamID1;
                            }
                            query = "INSERT INTO Predictions (Season, Week, HomeTeamID, AwayTeamID) VALUES (2018, " + week + ", " + homeTeamID + ", " + awayTeamID + ")";
                            command = new MySqlCommand(query, conn);
                            command.Parameters.AddWithValue("@Week", week);
                            command.Parameters.AddWithValue("@HomeTeamID", homeTeamID);
                            command.Parameters.AddWithValue("@AwayTeamID", awayTeamID);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
