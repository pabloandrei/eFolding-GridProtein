CREATE TABLE [dbo].[DataToProcess] (
    [loadDatFile]       BIT              CONSTRAINT [DF_DataToProcess_loadDatFile] DEFAULT ((1000)) NULL,
    [totalSitio]        INT              NOT NULL,
    [isem]              INT              NOT NULL,
    [maxInterations]    INT              NOT NULL,
    [process_guid]      UNIQUEIDENTIFIER NOT NULL,
    [file]              TEXT             NULL,
    [maxMotionPeerIsem] BIGINT           CONSTRAINT [DF_DataToProcess_maxMotionPeerIsem] DEFAULT ((1000)) NOT NULL,
    [modelType]         TINYINT          CONSTRAINT [DF_DataToProcess_model] DEFAULT ((0)) NOT NULL,
    [valueOfDelta]      FLOAT (53)       CONSTRAINT [DF_DataToProcess_valueOfDelta] DEFAULT ((0)) NOT NULL,
    [beta]              FLOAT (53)       CONSTRAINT [DF_DataToProcess_beta] DEFAULT ((1.00)) NOT NULL,
    [targets_id]        INT              NULL,
    [temperature]       FLOAT (53)       CONSTRAINT [DF_DataToProcess_temperature] DEFAULT ((0.00)) NOT NULL,
    [recPathEvery]      INT              CONSTRAINT [DF_DataToProcess_recPathEvery] DEFAULT ((1)) NOT NULL,
    [splitFileEvery]    INT              CONSTRAINT [DF_DataToProcess_splitFileEvery] DEFAULT ((100000)) NOT NULL,
    CONSTRAINT [PK_DataToProcess] PRIMARY KEY CLUSTERED ([process_guid] ASC),
    CONSTRAINT [FK_DataToProcess_ModelType] FOREIGN KEY ([modelType]) REFERENCES [dbo].[ModelType] ([id]),
    CONSTRAINT [FK_DataToProcess_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_DataToProcess_Targets] FOREIGN KEY ([targets_id]) REFERENCES [dbo].[Targets] ([id])
);










GO


