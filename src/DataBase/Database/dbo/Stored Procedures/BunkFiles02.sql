

CREATE PROCEDURE [dbo].[BunkFiles02] @baseDir VARCHAR(MAX),
	@process_guid VARCHAR(MAX)
AS
BEGIN TRAN

	--DECLARE @baseDir AS VARCHAR(MAX) = 'C:\GridProteinFolding\Server\a501cb9d-9201-435a-81c1-134af1a7f0a6\Histogram'
	--DECLARE @process_guid AS VARCHAR(MAX)='a501cb9d-9201-435a-81c1-134af1a7f0a6'

	DELETE FROM dbo.Coordinates WHERE seed_isem IN 
		(SELECT isem FROM dbo.Seed WHERE process_guid=@process_guid)
	DELETE FROM dbo.Seed WHERE process_guid=@process_guid
	
	DECLARE @files TABLE (ID INT IDENTITY, FILENAME VARCHAR(100))

	DECLARE @cmd AS CHAR(500) = 'dir '+@baseDir+' /b'
	INSERT INTO @files EXECUTE xp_cmdshell @cmd

	Declare @sqlSchemaSeed as varchar(max)
	Declare @sqlSchemaCoordinates as varchar(max)

	DECLARE @fileName AS VARCHAR(max)
	DECLARE myCursor CURSOR FAST_FORWARD FOR
		SELECT FILENAME FROM @files
		OPEN myCursor FETCH NEXT FROM myCursor INTO @fileName
			WHILE @@FETCH_STATUS = 0 BEGIN 
				PRINT @fileName
				
			
				DECLARE @fullFileName AS VARCHAR(500)
				SET @fullFileName=RTRIM(@basedir)+'\'+RTRIM(@fileName)
				
				
				
				--Histogram
				SET @fullFileName=RTRIM(@basedir)+'\'+'fileIdealHistogramCalDistanceBetweenLastPointFirst.dat'
				
				DECLARE @sqlHistogram VARCHAR(MAX)
				SET @sqlHistogram= 'SELECT (SELECT isem FROM dbo.Seed WHERE process_guid=@process_guid),lenght,[min],[max],n,sum_RG,medium_RG
					FROM  OPENROWSET( 
					BULK  '+@fullFileName+',
							  FORMATFILE =''C:\GridProteinFolding\schemaHistogram.fmt'',FIRSTROW=2,LASTROW=2
									)
						as schemaHistogram'
				
				INSERT INTO dbo.Histogram (seed_isem,lenght,min,max,n,sum,medium) EXEC (@sqlHistogram)
				
				
				
			 FETCH NEXT FROM myCursor INTO @fileName
		END CLOSE myCursor
	DEALLOCATE myCursor


	IF @@ERROR>0
		ROLLBACK TRAN
	ELSE
		COMMIT TRAN