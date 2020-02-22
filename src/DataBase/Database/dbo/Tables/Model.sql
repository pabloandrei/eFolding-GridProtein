CREATE TABLE [dbo].[Model] (
    [process_guid] UNIQUEIDENTIFIER NOT NULL,
    [monomero]     TINYINT          NOT NULL,
    [value]        FLOAT (53)       NOT NULL,
    CONSTRAINT [PK_Model] PRIMARY KEY CLUSTERED ([process_guid] ASC, [monomero] ASC),
    CONSTRAINT [FK_Model_Process] FOREIGN KEY ([process_guid]) REFERENCES [dbo].[Process] ([guid]) ON DELETE CASCADE ON UPDATE CASCADE
);





