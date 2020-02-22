CREATE FUNCTION dbo.ReturnValues(@guid uniqueidentifier)
RETURNS VARCHAR(MAX) 
AS 

BEGIN
    DECLARE @values AS VARCHAR(8000)
	SET @values=NULL

	SELECT @values=COALESCE(@values + ',','') + LTRIM(STR(M.value))
		FROM dbo.Model M	
		WHERE M.process_guid=@guid
		ORDER BY M.process_guid, M.monomero, M.value

	RETURN @values
END;