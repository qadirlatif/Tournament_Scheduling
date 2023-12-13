using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Tournament_Scheduling.Models;
using Tournament_Scheduling.View_Model;

namespace Tournament_Scheduling.Controllers
{
    public class CricketController : Controller
    {
        TSContext db = new TSContext();
        // GET: Cricket
        public List<Match> RoundRobinFixtures(List<Venue> venues, List<Team> teams, DateTime startDate, DateTime endDate)
        {
            List<Match> fixtures = new List<Match>();
            int totalTeams = teams.Count;
            int totalVenues = venues.Count;

            DateTime currentDate = startDate;
            int venueIndex = 0;

            // Shuffle the order of teams randomly
            Random random = new Random();
            teams = teams.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < totalTeams - 1; i++)
            {
                for (int j = i + 1; j < totalTeams; j++)
                {
                    // Create a match for the current team pair
                    Match match = new Match
                    {
                        TeamA_id = teams[i].Teamid,
                        TeamB_id = teams[j].Teamid,
                        MatchDate = currentDate,
                        Venue = venues[venueIndex % totalVenues].Name,
                    };

                    // Increment the venue index for the next match
                    venueIndex++;

                    // Add the match to the list
                    fixtures.Add(match);

                    // Increment the date for the next match
                    currentDate = currentDate.AddDays(1);

                    // Check if the date exceeds the specified end date
                    if (currentDate > endDate)
                    {
                        // Reset the date to the start date
                        currentDate = startDate;
                    }
                }
            }

            return fixtures;
        }
        public List<Match> RoundRobinDateSetter (List<Venue> venues, List<Team> teams, DateTime startDate, DateTime endDate)
        {
            List<Match> fixtures = new List<Match>();
            fixtures = RandomMatches<Match>(RoundRobinFixtures(venues, teams, startDate, endDate));
            List<Match> finalScedule = new List<Match>();
            DateTime currentDate = startDate;

            foreach (var match in fixtures)
            {
                match.MatchDate = currentDate;


                finalScedule.Add(match);

                // Increment the date for the next match
                currentDate = currentDate.AddDays(1);

                // Check if the date exceeds the specified end date
                if (currentDate > endDate)
                {
                    // Reset the date to the start date
                    currentDate = startDate;
                }
            }

            return finalScedule;
        }
        static List<Match> GenerateKnockoutSchedule(List<Team> teams)
        {
            List<Match> matches = new List<Match>();
            int totalTeams = teams.Count;

            // Ensure the number of teams is a power of 2
            if ((totalTeams & (totalTeams - 1)) != 0)
            {
                //throw new ArgumentException("The number of teams must be a power of 2.");
            }

            // Generate matches for each round
            //for (int round = 1; round < totalTeams; round += 2)
            //{
            int round = 1;
                for (int i = 0; i < totalTeams; i += round * 2)
                {
                    Match match = new Match
                    {
                        TeamA_id = teams[i].Teamid,
                        TeamB_id = teams[i + round].Teamid
                    };

                    matches.Add(match);
                }
            //}

            return matches;
        }
        static List<Match> RandomMatches<T>(List<Match> Matches)
        {
            List<Match> randomList = new List<Match>(Matches);
            Random random = new Random();

            int n = randomList.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Match value = randomList[k];
                randomList[k] = randomList[n];
                randomList[n] = value;
            }

            return randomList;
        }

        public ActionResult Index()
        {
            var events = db.Events.ToList();
            if(events != null)
            {
                return View(events);
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult Schedules(int id)
        {
            var matches = db.matches.Where(x => x.EventId == id).OrderBy(x=>x.MatchDate).ToList();
            List<MatchesViewModel> model = new List<MatchesViewModel>();
            var myevent = db.Events.Where(x => x.Eventid == id).FirstOrDefault();
            foreach(var match in matches)
            {
                MatchesViewModel matchdetails = new MatchesViewModel();
                matchdetails.MatchDate = match.MatchDate;
                matchdetails.Venue = match.Venue;
                matchdetails.TeamA_Name = db.teams.Where(x => x.Teamid == match.TeamA_id).Select(x => x.Name).FirstOrDefault();
                matchdetails.TeamB_Name = db.teams.Where(x => x.Teamid == match.TeamB_id).Select(x => x.Name).FirstOrDefault();
                matchdetails.TeamA_score = match.TeamA_score;
                matchdetails.TeamB_score = match.TeamB_score;
                matchdetails.matchid = match.Matchid;
                model.Add(matchdetails);
            }
            ViewBag.events = myevent;
            return View(model);
        }
        public ActionResult AddEvent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addEvent(List<Venue> Venues, List<Team> teams, string eventname, string type, DateTime StartDate, DateTime EndDate)
        {
            Random rand = new Random();
            int eventid = rand.Next(999, 1000000);
            Event myevent = new Event();
            myevent.Eventid = eventid;
            myevent.name = eventname;
            myevent.startdate = StartDate;
            myevent.enddate = EndDate;
            myevent.keyparams = eventid.ToString();
            db.Events.Add(myevent);
            db.SaveChanges();
            List<Team> allteams = new List<Team>();
            foreach (Team team in teams)
            {
                team.EventID = eventid;
                team.Teamid = rand.Next(9999, 1000000);
                team.keyparams = team.Teamid.ToString();
                allteams.Add(team);
                db.teams.Add(team);
                db.SaveChanges();
                PointsTable point = new PointsTable();
                point.TeamID = team.Teamid;
                point.EventID = eventid;
                point.TeamName = team.Name;
                point.Matches = 0;
                point.Wins = 0;
                point.Loss = 0;
                point.Points = 0;
                point.Keyparams = team.Teamid.ToString();
                db.PointsTables.Add(point);
                db.SaveChanges();
            }
            List<Match> matches = new List<Match>();
            if (type == "1")
            {
                matches = RoundRobinDateSetter(Venues, allteams, StartDate, EndDate);
            }
            else if (type == "2")
            {
                matches = GenerateKnockoutSchedule( allteams);
            }
            
            foreach(var match in matches)
            {
                var matchid = rand.Next(999, 999999);
                match.EventId = eventid;
                match.Matchid = matchid;
                match.MatchDate = DateTime.Now;
                match.keyparams = matchid.ToString();
                db.matches.Add(match);
                db.SaveChanges();
                

            }
            
            return Content("event added");
        }
        [HttpPost]
        public ActionResult UpdateMatch(MatchesViewModel Match)
        {
            var target = db.matches.Where(x => x.Matchid == Match.matchid).FirstOrDefault();
            target.TeamA_score = Match.TeamA_score;
            target.TeamB_score = Match.TeamB_score;
            target.Venue = Match.Venue;
            target.MatchDate = Match.MatchDate;
            db.Entry(target).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var targetteamAPoint = db.PointsTables.Where(x => x.TeamID == target.TeamA_id).FirstOrDefault();
            var targetteamBPoint = db.PointsTables.Where(x => x.TeamID == target.TeamB_id).FirstOrDefault();
            if (target.TeamA_score > target.TeamB_score)
            {
                targetteamAPoint.Matches += 1;
                targetteamBPoint.Matches += 1;
                targetteamAPoint.Wins += 1;
                targetteamBPoint.Loss += 1;
                targetteamAPoint.Points += 2;
                db.Entry(targetteamAPoint).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Entry(targetteamBPoint).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else if(target.TeamB_score > target.TeamA_score)
            {
                targetteamAPoint.Matches += 1;
                targetteamBPoint.Matches += 1;
                targetteamAPoint.Loss += 1;
                targetteamBPoint.Wins += 1;
                targetteamBPoint.Points += 2;
                db.Entry(targetteamAPoint).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Entry(targetteamBPoint).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Content("Match Details Updated");
        }
        public ActionResult PointsTable(int EventID = 0)
        {
            var pointstable = db.PointsTables.Where(x => x.EventID == EventID).OrderByDescending(x=>x.Points).ToList();
            return View(pointstable);
        }
    }
}