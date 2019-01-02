SET XACT_ABORT ON;

ALTER DATABASE CURRENT
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE CURRENT
SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE



SET XACT_ABORT ON
BEGIN TRAN
    DROP TABLE IF EXISTS [dbo].[DatabaseVersion];
	DROP TABLE IF EXISTS [dbo].[IntervalText];
	DROP TABLE IF EXISTS [dbo].[BaseText];
	DROP TABLE IF EXISTS [dbo].[CultureHierarchy];
	DROP TABLE IF EXISTS [dbo].[Culture];
	DROP TABLE IF EXISTS [dbo].[DictionaryScope];


	CREATE TABLE [dbo].[DatabaseVersion]
	(
	   [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_DatabaseVersion(Id)] PRIMARY KEY CLUSTERED,
	   [DatabaseVersion] varchar(50) NOT NULL,
	   [SolutionVersion] varchar(50) NULL,
	   [UpgradeDate] datetime NOT NULL DEFAULT GETDATE(),
	   [UpgradeUser] varchar(150) NOT NULL default SYSTEM_USER,
	);

	CREATE TABLE [dbo].[DictionaryScope]
	(
		[Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_DictionaryScope(Id)] PRIMARY KEY CLUSTERED,
		[Name] varchar(255) NOT NULL UNIQUE
	);

	CREATE TABLE [dbo].[Culture]
	(
		[Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Culture(Id)] PRIMARY KEY CLUSTERED,
		[Name] varchar(5) NOT NULL UNIQUE
	);

	CREATE TABLE [dbo].[CultureHierarchy]
	(
		[Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_CultureHierarchy(Id)] PRIMARY KEY CLUSTERED,
		[Culture] int NOT NULL CONSTRAINT [FK_CultureHierarchy(Culture)_Culture(Id)] FOREIGN KEY REFERENCES [dbo].[Culture] ON DELETE NO ACTION,
		[ParentCulture] int NOT NULL CONSTRAINT [FK_CultureHierarchy(ParentCulture)_Culture(Id)] FOREIGN KEY REFERENCES [dbo].[Culture] ON DELETE NO ACTION,
		[LevelProperty] tinyint NOT NULL,
		CONSTRAINT [UQ_CultureHierarchy(Culture,ParentCulture)] UNIQUE(Culture, ParentCulture)
	);

	CREATE TABLE [dbo].[BaseText]
	(
		[Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_BaseText(Id)] PRIMARY KEY CLUSTERED,
		[Culture] int NOT NULL CONSTRAINT [FK_BaseText(Culture)_Culture(Id)] FOREIGN KEY REFERENCES [dbo].[Culture],
		[DictionaryScope] int NOT NULL CONSTRAINT [FK_BaseText(DictionaryScope)_DictionaryScope(Id)] FOREIGN KEY REFERENCES [dbo].[DictionaryScope] ON DELETE NO ACTION,
	    [Name] varchar(255) NOT NULL,
	    [Format] smallint NOT NULL,
		[Discriminator] nvarchar(max) NOT NULL,
		[Text] nvarchar(max) NOT NULL,
	    [ModificationTime] datetime NOT NULL DEFAULT GETDATE(),
	    [ModificationUser] nvarchar(255) NULL,
		CONSTRAINT [UQ_BaseText(Culture,DictionaryScope,Name)] UNIQUE([Culture], [DictionaryScope], [Name])
	);

    CREATE TABLE [IntervalText]
	(
        [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_IntervalText(Id)] PRIMARY KEY CLUSTERED,
        [IntervalEnd] int NOT NULL,
        [IntervalStart] int NOT NULL,
        [Text] nvarchar(max) NOT NULL,
		[PluralizedStaticText] int NOT NULL CONSTRAINT [FK_IntervalText(PluralizedStaticText)_PluralizedStaticText(Id)] FOREIGN KEY REFERENCES [dbo].[BaseText] ON DELETE NO ACTION,
    );


    INSERT INTO [dbo].[DatabaseVersion]
		(DatabaseVersion)
	VALUES
		('000' );
		-- DatabaseVersion - varchar

--ROLLBACK
COMMIT
