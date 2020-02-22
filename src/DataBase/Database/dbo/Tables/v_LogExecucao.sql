CREATE TABLE [dbo].[v_LogExecucao] (
    [guid]              UNIQUEIDENTIFIER NOT NULL,
    [date]              SMALLDATETIME    NOT NULL,
    [userId]            UNIQUEIDENTIFIER NOT NULL,
    [status_id]         TINYINT          NOT NULL,
    [emailNotification] TINYINT          NOT NULL,
    [note]              NVARCHAR (50)    NULL,
    [label]             VARCHAR (255)    NOT NULL,
    [logDate]           SMALLDATETIME    NOT NULL,
    [logStatus_id]      TINYINT          NOT NULL,
    [description]       VARCHAR (50)     NOT NULL
);

