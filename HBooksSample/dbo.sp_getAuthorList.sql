CREATE PROCEDURE [dbo].sp_getAuthorList
@lastname nvarchar(50)

AS
	SELECT * 
	from AUTHOR 
	where AUTHOR_last like @lastname + '%' 
RETURN 0