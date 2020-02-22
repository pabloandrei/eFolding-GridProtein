CREATE PROCEDURE dbo.ImportTargets
AS
	BEGIN TRAN

	PRINT '-------- CREATE TEMP --------';  

		CREATE TABLE #temp (
			pos1 varchar(2), pos2 varchar(2), pos3 varchar(2), pos4 varchar(2), pos5 varchar(2),
			pos6 varchar(2), pos7 varchar(2), pos8 varchar(2), pos9 varchar(2), pos10 varchar(2),
			pos11 varchar(2), pos12 varchar(2), pos13 varchar(2), pos14 varchar(2), pos15 varchar(2),
			pos16 varchar(2), pos17 varchar(2), pos18 varchar(2), pos19 varchar(2), pos20 varchar(2),
			pos21 varchar(2), pos22 varchar(2), pos23 varchar(2), pos24 varchar(2), pos25 varchar(2),
			pos26 varchar(2), pos27 varchar(2)
		)

		INSERT INTO #temp SELECT pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, pos10,
			pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19, pos20,
			pos21, pos22, pos23, pos24, pos25, pos26, pos27
		  FROM  OPENROWSET(BULK  'C:\temp\cfg51704.dat',
		  FORMATFILE='C:\temp\formato.fmt'  
		   ) as t1;

	PRINT '-------- DELETE TABLES --------';  

		DELETE FROM [dbo].[TargetsCoordinates]
		DBCC CHECKIDENT ('[dbo].[TargetsCoordinates]', RESEED, 0);
		DELETE FROM [dbo].[Targets]
	

		SET NOCOUNT ON;  
  
		DECLARE @pos1 varchar(2), @pos2 varchar(2), @pos3 varchar(2), @pos4 varchar(2), @pos5 varchar(2),
			@pos6 varchar(2), @pos7 varchar(2), @pos8 varchar(2), @pos9 varchar(2), @pos10 varchar(2),
			@pos11 varchar(2), @pos12 varchar(2), @pos13 varchar(2), @pos14 varchar(2), @pos15 varchar(2),
			@pos16 varchar(2), @pos17 varchar(2), @pos18 varchar(2), @pos19 varchar(2), @pos20 varchar(2),
			@pos21 varchar(2), @pos22 varchar(2), @pos23 varchar(2), @pos24 varchar(2), @pos25 varchar(2),
			@pos26 varchar(2), @pos27 varchar(2);
  
	PRINT '-------- IMPORT --------';  
  
		DECLARE @row int = 1;

		DECLARE tempCursor CURSOR FOR   
		SELECT pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, pos10,
			pos11, pos12, pos13, pos14, pos15, pos16, pos17, pos18, pos19, pos20,
			pos21, pos22, pos23, pos24, pos25, pos26, pos27
		FROM #temp;
  
		OPEN tempCursor  
  
		FETCH NEXT FROM tempCursor   
		INTO @pos1,@pos2,@pos3,@pos4,@pos5,@pos6,@pos7,@pos8,@pos9,@pos10,
			@pos11,@pos12,@pos13,@pos14,@pos15,@pos16,@pos17,@pos18,@pos19,@pos20,
			@pos21,@pos22,@pos23,@pos24,@pos25,@pos26,@pos27;
  
		WHILE @@FETCH_STATUS = 0  
		BEGIN  

			INSERT INTO [dbo].[Targets] VALUES (@row, null)

			;WITH CTE
			AS
				(
				SELECT* FROM (
					SELECT @pos1 as pos1,@pos2 as pos2,@pos3 as pos3,@pos4 as pos4,@pos5 as pos5,
					@pos6 as pos6,@pos7 as pos7,@pos8 as pos8,@pos9 as pos9,@pos10 as pos10,
					@pos11 as pos11,@pos12 as pos12,@pos13 as pos13,@pos14 as pos14,@pos15 as pos15,
					@pos16 as pos16,@pos17 as pos17,@pos18 as pos18,@pos19 as pos19,@pos20 as pos20,
					@pos21 as pos21,@pos22 as pos22,@pos23 as pos23,@pos24 as pos24,@pos25 as pos25,
					@pos26 as pos26,@pos27 as pos27) as T
				UNPIVOT ( value FOR N IN (pos1,pos2,pos3,pos4,pos5,pos6,pos7,pos8,pos9,pos10,
				pos11,pos12,pos13,pos14,pos15,pos16,pos17,pos18,pos19,pos20,
				pos21,pos22,pos23,pos24,pos25,pos26,pos27))P
				)
				INSERT INTO [dbo].[TargetsCoordinates]
					SELECT @row as targets_id,value AS value
					FROM CTE

					

			SET @row=@row+1

			FETCH NEXT FROM tempCursor   
			INTO @pos1,@pos2,@pos3,@pos4,@pos5,@pos6,@pos7,@pos8,@pos9,@pos10,
			@pos11,@pos12,@pos13,@pos14,@pos15,@pos16,@pos17,@pos18,@pos19,@pos20,
			@pos21,@pos22,@pos23,@pos24,@pos25,@pos26,@pos27;
		END   
		CLOSE tempCursor;  
		DEALLOCATE tempCursor;  

	PRINT '-------- DROP TEMP --------';  
		DROP TABLE #temp

	--PRINT '-------- DROP TEMP --------';  
	--	SELECT TOP 2 * FROM [dbo].[Targets]
	--	SELECT TOP 54 * FROM [dbo].[TargetsCoordinates]

	IF @@ERROR=0
		COMMIT TRAN
	ELSE
		ROLLBACK TRAN