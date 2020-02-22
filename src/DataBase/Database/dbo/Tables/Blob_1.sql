CREATE TABLE [dbo].[Blob] (
    [process_guid] UNIQUEIDENTIFIER NOT NULL,
    [blob]         VARBINARY (MAX)  NULL,
    CONSTRAINT [FK_Blob_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid])
);

