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
        public List<Match> RoundRobinFIxtures(List<Venue> venues , List<Team> teams, DateTime startDate, DateTime endDate)
        {
            List<Match> fixtures = new List<Match>();
            int totalTeams = teams.Count;
            int totalVenues = venues.Count;

            DateTime currentDate = startDate;
            int venueIndex = 0;

            
                for (int i = 0; i < totalTeams - 1; i++)
                {
                    for (int j = i + 1; j < totalTeams; j++)
                    {
                        // Check if the current date is within the specified date range
                        if (currentDate <= endDate)
                        {
                            Match match = new Match
                            {
                                TeamA_id = teams[i].id,
                                TeamB_id = teams[j].id,
                                MatchDate = currentDate,
                                Venue = venues[venueIndex % totalVenues].Name, // Assign a venue based on the available venues list

                            };

                            fixtures.Add(match);

                            // Increment the date and time for the next match
                            currentDate = currentDate.AddDays(1); // You can adjust this as needed
                            venueIndex++;

                        }
                    }
                
            }

            return fixtures;
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
            var myevent = db.Events.Where(x => x.id == id).FirstOrDefault();
            foreach(var match in matches)
            {
                MatchesViewModel matchdetails = new MatchesViewModel();
                matchdetails.MatchDate = match.MatchDate;
                matchdetails.Venue = match.Venue;
                matchdetails.TeamA_Name = db.teams.Where(x => x.id == match.TeamA_id).Select(x => x.Name).FirstOrDefault();
                matchdetails.TeamB_Name = db.teams.Where(x => x.id == match.TeamB_id).Select(x => x.Name).FirstOrDefault();
                matchdetails.TeamA_score = match.TeamA_score;
                matchdetails.TeamB_score = match.TeamB_score;
                matchdetails.matchid = match.id;
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
            myevent.id = eventid;
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
                team.id = rand.Next(9999, 1000000);
                team.keyparams = team.id.ToString();
                allteams.Add(team);
                db.teams.Add(team);
                db.SaveChanges();
                PointsTable point = new PointsTable();
                point.TeamID = team.id;
                point.EventID = eventid;
                point.TeamName = team.Name;
                point.Matches = 0;
                point.Wins = 0;
                point.Loss = 0;
                point.Points = 0;
                point.Keyparams = team.id.ToString();
                db.PointsTables.Add(point);
                db.SaveChanges();
            }

            var matches = RoundRobinFIxtures(Venues, allteams,  StartDate, EndDate );
            foreach(var match in matches)
            {
                var matchid = rand.Next(999, 999999);
                match.EventId = eventid;
                match.id = matchid;
                match.keyparams = matchid.ToString();
                db.matches.Add(match);
                db.SaveChanges();
                

            }
            
            return Content("event added");
        }
        [HttpPost]
        public ActionResult UpdateMatch(MatchesViewModel Match)
        {
            var target = db.matches.Where(x => x.id == Match.matchid).FirstOrDefault();
            target.TeamA_score = Match.TeamA_score;
            target.TeamB_score = Match.TeamB_score;
            target.Venue = Match.Venue;
            target.MatchDate = Match.MatchDate;
            db.Entry(target).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("Match Details Updated");
        }
        public ActionResult PointsTable(int EventID = 0)
        {
            var pointstable = db.PointsTables.Where(x => x.EventID == EventID).ToList();
            return View(pointstable);
        }
    }
}