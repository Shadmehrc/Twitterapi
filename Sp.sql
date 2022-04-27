USE TwitterDatabase;
GO
CREATE PROCEDURE MostLikedTweet
AS
BEGIN
    SELECT Id,
           Text,
           Likes,
           UserId,
           TweetViewCount
    FROM dbo.Tweets
    WHERE Likes =
    (
        SELECT MAX(Likes)FROM dbo.Tweets
    );
END;
GO


USE TwitterDatabase;
GO
CREATE PROCEDURE MostViewedTweet
AS
BEGIN
    SELECT Tweet.Id,
           Tweet.Text,
           Tweet.UserId,
           Tweet.TagCount,
           Tweet.TweetViewCount
    FROM dbo.Tweets AS Tweet
    WHERE TweetViewCount =
    (
        SELECT MAX(TweetViewCount)FROM dbo.Tweets
    );
END;
GO



USE [TwitterDatabase];
GO
CREATE PROCEDURE MostTaggedTweet
AS
BEGIN
    SELECT Tweet.Id,
           Tweet.Text,
           Tweet.TagCount,
           Tag.Word
    FROM dbo.Tweets AS Tweet WITH (NOLOCK)
        INNER JOIN dbo.Tags Tag WITH (NOLOCK)
            ON Tweet.Id = Tag.TweetId
    WHERE Tweet.TagCount =
    (
        SELECT MAX(TagCount)FROM dbo.Tweets
    );
END;
GO


USE TwitterDatabase;
GO
CREATE PROCEDURE CreateTag @Word NVARCHAR(MAX)
AS
BEGIN
    IF EXISTS (SELECT * FROM dbo.Tags WHERE Word = @Word)
	BEGIN
	    SELECT Id,Word FROM dbo.Tags WHERE Word=@Word
		RETURN
    END;

    INSERT INTO dbo.Tags
    (
        Word
    )
    VALUES
    (@Word -- Word - nvarchar(max)
        );

    SELECT Id,
           Word
    FROM dbo.Tags
    WHERE Id = @@IDENTITY;
END;