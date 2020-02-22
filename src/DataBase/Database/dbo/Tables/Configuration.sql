CREATE TABLE [dbo].[Configuration] (
    [id]           BIGINT           IDENTITY (1, 1) NOT NULL,
    [process_guid] UNIQUEIDENTIFIER NOT NULL,
    [mcStep]       INT              NOT NULL,
    [order]        INT              NOT NULL,
    [x]            INT              NOT NULL,
    [y]            INT              NOT NULL,
    [z]            INT              NOT NULL,
    CONSTRAINT [PK_Configuration_1] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Configuration_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid])
);

