CREATE PROCEDURE [dbo].sp_getPubisherNameSpecifics
@publisher nvarchar(25)
AS
	 SELECT PUBLISHER_NAME
	 from PUBLISHER 
	 WHERE PUBLISHER_NAME like '%' + @publisher + '%'
	order by PUBLISHER_NAME
RETURN 0
