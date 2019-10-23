
CREATE DATABASE BlogEngine
GO
PRINT 'BlogEngine DATABASE CREATED'		

USE [BlogEngine]
GO
PRINT 'BlogEngine DATABASE USED'

BEGIN

DECLARE @BlogEngineDatabase NVARCHAR(10) = 'BlogEngine';
DECLARE @RoleTable NVARCHAR(10) = 'Role';
DECLARE @PersonTable NVARCHAR(10) = 'Person';
DECLARE @PostTable NVARCHAR(10) = 'Post';
DECLARE @CommentTable NVARCHAR(10) = 'Comment';

DECLARE @RoleWriter NVARCHAR(10) = 'writer';
DECLARE @RoleEditor NVARCHAR(10) = 'editor';


IF EXISTS (SELECT * FROM master.dbo.sysdatabases WHERE [name] = @BlogEngineDatabase)
BEGIN

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=@RoleTable and xtype='U')
BEGIN
	CREATE TABLE Role(
	Id   BIGINT IDENTITY(1, 1) NOT NULL, 
	Name NVARCHAR(50) NOT NULL, 
	CreationDate DATETIME NOT NULL,
	LastUpdated DATETIME NOT NULL,
	PRIMARY KEY (Id) 
	)
	PRINT @RoleTable + ' TABLE CREATED'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=@PersonTable and xtype='U')
BEGIN
	CREATE TABLE Person(
	Id   BIGINT IDENTITY(1, 1) NOT NULL, 
	RoleId BIGINT NOT NULL, 
	Name NVARCHAR(50) NOT NULL,
	UserName NVARCHAR(50) NULL,
	Email NVARCHAR(50) NOT NULL,
	Pass NVARCHAR(50) NOT NULL,
	CreationDate DATETIME NOT NULL,
	LastUpdated DATETIME NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (RoleId) REFERENCES Role(Id) 
	)
	PRINT @PersonTable + ' TABLE CREATED'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=@PostTable and xtype='U')
BEGIN
	CREATE TABLE Post(
	Id   BIGINT IDENTITY(1, 1) NOT NULL, 
	Title NVARCHAR(50) NOT NULL,
	Body NVARCHAR(MAX) NULL,
	ImageURL NVARCHAR(MAX) NULL,
	AuthorName NVARCHAR(50) NOT NULL,
	PendingToApprove BIT NULL,
	ApproverName NVARCHAR(50) NULL,
	ApprovalDateTime DATETIME NULL,
	IsPublished BIT NULL,
	CreationDate DATETIME NOT NULL,
	LastUpdated DATETIME NOT NULL,
	PRIMARY KEY (Id) 
	)
	PRINT @PostTable + ' TABLE CREATED'
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name=@CommentTable and xtype='U')
BEGIN
	CREATE TABLE Comment(
	Id   BIGINT IDENTITY(1, 1) NOT NULL,
	PostId BIGINT NOT NULL, 
	Body NVARCHAR(MAX) NULL,
	AuthorName NVARCHAR(50) NOT NULL,
	AuthorEmail NVARCHAR(50) NOT NULL,
	CreationDate DATETIME NOT NULL,
	LastUpdated DATETIME NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (PostId) REFERENCES Post(Id) 
	)
	PRINT @CommentTable + ' TABLE CREATED'
END


IF NOT EXISTS (SELECT * FROM Role WHERE name=@RoleWriter)
BEGIN
	INSERT INTO [dbo].[Role] (Name, CreationDate, LastUpdated) VALUES (@RoleWriter, GETDATE(), GETDATE())
	PRINT 'Role '+ @RoleWriter +' DATA INSERTED'
END

IF NOT EXISTS (SELECT * FROM Role WHERE name=@RoleEditor)
BEGIN
	INSERT INTO [dbo].[Role] (Name, CreationDate, LastUpdated) VALUES (@RoleEditor, GETDATE(), GETDATE())
	PRINT 'Role '+ @RoleEditor +' DATA INSERTED'
END

IF NOT EXISTS (SELECT * FROM Person WHERE name='User Writer')
BEGIN
	DECLARE @RoleWriterId INT = (select Id from Role where Name = @RoleWriter);
        
	INSERT INTO [dbo].[Person] (RoleId, Name, UserName, Email, Pass, CreationDate, LastUpdated) 
	VALUES (@RoleWriterId, 'User Writer', 'userwriter', 'userwriter@test.com', 'userwriter', GETDATE(), GETDATE())
	PRINT 'User Writer DATA INSERTED'
END

IF NOT EXISTS (SELECT * FROM Person WHERE name='User Editor')
BEGIN
	DECLARE @RoleEditorId INT = (select Id from Role where Name = @RoleEditor);
        
	INSERT INTO [dbo].[Person] (RoleId, Name, UserName, Email, Pass, CreationDate, LastUpdated) 
	VALUES (@RoleEditorId, 'User Editor', 'usereditor', 'usereditor@test.com', 'usereditor', GETDATE(), GETDATE())
	PRINT 'User Editor DATA INSERTED'
END


END

END