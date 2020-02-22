
CREATE PROCEDURE [dbo].[BunkFiles] @baseDir VARCHAR(MAX),
	@process_guid VARCHAR(MAX)
AS
BEGIN TRAN

	--DECLARE @baseDir AS VARCHAR(MAX) = 'C:\GridProteinFolding\Server\a501cb9d-9201-435a-81c1-134af1a7f0a6\Seed'
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
				
				DECLARE @results table (id bigint,x int, y int, z int)
				
				DECLARE @fullFileName AS VARCHAR(500)
				SET @fullFileName=RTRIM(@basedir)+'\'+RTRIM(@fileName)
				
				Set @sqlSchemaCoordinates = '
				DECLARE @isem AS INT
				
				SELECT @isem=isem
						FROM  OPENROWSET( 
						BULK  ''' + @fullFileName+ ''',
								  FORMATFILE =''C:\GridProteinFolding\schemaSeed.fmt'',LASTROW=1
										)
							as schemaSeed	
				
				INSERT INTO dbo.Seed (isem,process_guid) VALUES (@isem,''' +@process_guid+ ''')
							
				SELECT @isem,*
					FROM  OPENROWSET( 
					BULK  ''' + @fullFileName+ ''',
							  FORMATFILE =''C:\GridProteinFolding\schemaCoordinates.fmt'',FIRSTROW=2
									)
						as schemaCoordinates'
				
				INSERT INTO dbo.Coordinates EXEC (@sqlSchemaCoordinates)
				
			 FETCH NEXT FROM myCursor INTO @fileName
		END CLOSE myCursor
	DEALLOCATE myCursor


	IF @@ERROR>0
		ROLLBACK TRAN
	ELSE
		COMMIT TRAN