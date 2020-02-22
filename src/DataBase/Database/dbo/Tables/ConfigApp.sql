CREATE TABLE [dbo].[ConfigApp] (
    [id]                INT          IDENTITY (1, 1) NOT NULL,
    [randomEqualOne]    FLOAT (53)   NOT NULL,
    [randomMagicNumber] VARCHAR (50) NOT NULL,
    [dateTime]          DATETIME     CONSTRAINT [DF_ConfigApp_dateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ConfigApp] PRIMARY KEY CLUSTERED ([id] ASC)
);

