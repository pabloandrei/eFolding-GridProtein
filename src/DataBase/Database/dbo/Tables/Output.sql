CREATE TABLE [dbo].[Output] (
    [guid]          UNIQUEIDENTIFIER NOT NULL,
    [evolution]     BIT              CONSTRAINT [DF_Output_evolution] DEFAULT ((1)) NOT NULL,
    [distribution]  BIT              CONSTRAINT [DF_Output_distribution] DEFAULT ((1)) NOT NULL,
    [configuration] BIT              CONSTRAINT [DF_Output_configuration] DEFAULT ((1)) NOT NULL,
    [debug]         BIT              CONSTRAINT [DF_Output_debug] DEFAULT ((1)) NOT NULL,
    [histogram]     BIT              CONSTRAINT [DF_Output_histogram] DEFAULT ((0)) NOT NULL,
    [trajectory]    BIT              CONSTRAINT [DF_Output_trajectory] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Output] PRIMARY KEY CLUSTERED ([guid] ASC),
    CONSTRAINT [FK_Output_Process] FOREIGN KEY ([guid]) REFERENCES [dbo].[Process] ([guid]) ON DELETE CASCADE
);

