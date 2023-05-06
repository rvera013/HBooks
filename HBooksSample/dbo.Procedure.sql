CREATE PROCEDURE [dbo].sp_getInventoryList
@BookCode nvarchar(4)

AS
	SELECT * from INVENTORY where BOOK_CODE like @BookCode + '%' 
RETURN 0