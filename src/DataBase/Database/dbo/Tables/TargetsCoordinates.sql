CREATE TABLE [dbo].[TargetsCoordinates] (
    [id]         INT IDENTITY (1, 1) NOT NULL,
    [targets_id] INT NOT NULL,
    [value]      INT NOT NULL,
    CONSTRAINT [PK_TargetsCoordinates] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_TargetsCoordinates_Targets] FOREIGN KEY ([targets_id]) REFERENCES [dbo].[Targets] ([id])
);



