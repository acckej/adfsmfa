﻿USE [%DATABASENAME%];

SET ANSI_NULLS ON;

SET QUOTED_IDENTIFIER ON;

CREATE TABLE [dbo].[KEYS]
(
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UPN] [varchar](256) NOT NULL,
	[SECRETKEY] [varchar](8000) NULL,
	[CERTIFICATE] [varchar](8000) NULL,
 CONSTRAINT [PK_REGISTRATIONS] PRIMARY KEY CLUSTERED ([ID] ASC) ,
 CONSTRAINT [IX_KEYS_UPN] UNIQUE NONCLUSTERED ([UPN] ASC)
 );

USE [master];

ALTER DATABASE [%DATABASENAME%] SET READ_WRITE;

