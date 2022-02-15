USE catalogdb;
GO
DROP PROCEDURE IF EXISTS dbo.Get_Score;
GO
CREATE PROCEDURE Get_Score
    @filmid int,
	@score float out 
AS
Select @score = CAST(Sum(ValueRating) AS float)/Count(*) from Ratings
Where Ratings.FilmId = @filmid
GO