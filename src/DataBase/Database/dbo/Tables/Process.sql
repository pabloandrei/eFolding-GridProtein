CREATE TABLE [dbo].[Process] (
    [guid]              UNIQUEIDENTIFIER NOT NULL,
    [guidFather]        UNIQUEIDENTIFIER NULL,
    [date]              DATETIME         CONSTRAINT [DF_Process_date] DEFAULT (getdate()) NOT NULL,
    [userId]            UNIQUEIDENTIFIER NOT NULL,
    [status_id]         TINYINT          CONSTRAINT [DF_Process_status] DEFAULT ((0)) NOT NULL,
    [emailNotification] TINYINT          CONSTRAINT [DF_Process_emailNOtification] DEFAULT ((0)) NOT NULL,
    [note]              NVARCHAR (50)    NULL,
    [label]             VARCHAR (255)    NOT NULL,
    [crypt]             BIT              CONSTRAINT [DF_Process_crypt] DEFAULT ((0)) NOT NULL,
    [macAddr]           VARCHAR (50)     NULL,
    [configApp_id]      INT              NOT NULL,
    [machineName]       NVARCHAR (50)    NULL,
    CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED ([guid] ASC),
    CONSTRAINT [FK_Process_ConfigApp] FOREIGN KEY ([configApp_id]) REFERENCES [dbo].[ConfigApp] ([id]),
    CONSTRAINT [FK_Process_Process] FOREIGN KEY ([guidFather]) REFERENCES [dbo].[Process] ([guid]),
    CONSTRAINT [FK_Process_Status] FOREIGN KEY ([status_id]) REFERENCES [dbo].[Status] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);








GO
CREATE TRIGGER [dbo].[Logging]
    ON [dbo].[Process]
    AFTER INSERT, UPDATE
    AS
    BEGIN
    SET NOCOUNT ON
	INSERT INTO [dbo].[ProcessLog] SELECT [guid],GETDATE(),[status_id],[machineName] FROM INSERTED

    END