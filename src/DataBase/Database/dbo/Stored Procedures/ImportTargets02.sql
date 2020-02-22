CREATE PROCEDURE dbo.ImportTargets02
AS
	BEGIN TRAN

	PRINT '-------- DELETE TABLES --------';  

		DELETE FROM [dbo].[Seeds]

	PRINT '-------- IMPORT --------';  

		INSERT INTO [dbo].[Seeds] 
			SELECT pos1, pos2
			  FROM  OPENROWSET(BULK  'C:\temp\ale10k.dat',
			  FORMATFILE='C:\temp\formato02.fmt'  
			   ) as t1;

	
	IF @@ERROR=0
		COMMIT TRAN
	ELSE
		ROLLBACK TRAN