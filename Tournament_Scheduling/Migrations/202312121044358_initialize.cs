namespace Tournament_Scheduling.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Eventid = c.Int(nullable: false),
                        name = c.String(),
                        startdate = c.DateTime(nullable: false),
                        enddate = c.DateTime(nullable: false),
                        keyparams = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Matchid = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        TeamA_id = c.Int(nullable: false),
                        TeamB_id = c.Int(nullable: false),
                        TeamA_score = c.Int(nullable: false),
                        TeamB_score = c.Int(nullable: false),
                        MatchDate = c.DateTime(nullable: false),
                        Venue = c.String(),
                        keyparams = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PointsTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TeamID = c.Int(nullable: false),
                        TeamName = c.String(),
                        EventID = c.Int(nullable: false),
                        Matches = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Loss = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Keyparams = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Teamid = c.Int(nullable: false),
                        EventID = c.Int(nullable: false),
                        Name = c.String(),
                        keyparams = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Team");
            DropTable("dbo.PointsTable");
            DropTable("dbo.Match");
            DropTable("dbo.Event");
        }
    }
}
