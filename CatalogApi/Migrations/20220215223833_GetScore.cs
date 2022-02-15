using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogApi.Migrations
{
    public partial class GetScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedur = @"CREATE PROCEDURE [dbo].[Get_Score]
                                @filmid int,
                                @score float out 
                            AS
                                Select @score = CAST(Sum(ValueRating) AS float)/Count(*) from Ratings
                            Where Ratings.FilmId = @filmid";
            migrationBuilder.Sql(procedur);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var procedur = @"DROP PROCEDURE [dbo].[Get_Score]";
            migrationBuilder.Sql(procedur);
        }
    }
}