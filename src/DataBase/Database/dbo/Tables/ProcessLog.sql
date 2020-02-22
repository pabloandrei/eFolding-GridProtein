CREATE TABLE [dbo].[ProcessLog] (
    [id]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [process_guid] UNIQUEIDENTIFIER NOT NULL,
    [date]         DATETIME         CONSTRAINT [DF_ProcessLog_date] DEFAULT (getdate()) NOT NULL,
    [status_id]    TINYINT          NOT NULL,
    [who]          VARCHAR (50)     NULL,
    CONSTRAINT [PK_ProcessLog] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_ProcessLog_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid]) ON DELETE CASCADE ON UPDATE CASCADE
);





