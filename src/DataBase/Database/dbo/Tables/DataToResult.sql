CREATE TABLE [dbo].[DataToResult] (
    [process_guid]   UNIQUEIDENTIFIER NOT NULL,
    [valueDivResult] INT              CONSTRAINT [DF_DataToResult_valueDivResult] DEFAULT ((100)) NOT NULL,
    [valueDiscard]   INT              CONSTRAINT [DF_DataToResult_valueDiscard] DEFAULT ((20)) NOT NULL,
    CONSTRAINT [PK_DataToResult] PRIMARY KEY CLUSTERED ([process_guid] ASC),
    CONSTRAINT [FK_DataToResult_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid]) ON DELETE CASCADE ON UPDATE CASCADE
);





