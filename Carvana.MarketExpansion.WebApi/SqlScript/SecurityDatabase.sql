DROP TABLE [MarketAuth].[tblSecurityRoleClaim]
DROP TABLE [MarketAuth].[tblSecurityRoleUser]
DROP TABLE [MarketAuth].[tblSecurityClaim]
DROP TABLE [MarketAuth].[tblSecurityRole]
DROP TABLE [MarketAuth].[tblSecurityUser]


CREATE TABLE [MarketAuth].[tblSecurityClaim](
	[SecurityClaimID] INT IDENTITY(1,1) NOT NULL,
	[SecurityClaimGUID] UNIQUEIDENTIFIER NOT NULL,
	[Name] [nvarchar](100) NULL,
	[RowLoadedDateTime] DATETIME2(7) NOT NULL,
	[RowUpdatedDateTime] DATETIME2(7) NOT NULL,
	CONSTRAINT [PK_MarketAuth_tblSecurityClaim] PRIMARY KEY CLUSTERED ([SecurityClaimID] ASC)
) 
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityClaim_SecurityClaimGUID ON MarketAuth.tblSecurityClaim (SecurityClaimGUID)
CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityClaim_Name ON MarketAuth.tblSecurityClaim (Name)


GO
CREATE TABLE [MarketAuth].[tblSecurityRole](
	[SecurityRoleID] INT IDENTITY(1,1) NOT NULL,
	[SecurityRoleGUID] UNIQUEIDENTIFIER NOT NULL,
	[Name] [nvarchar](100) NULL,
	[RowLoadedDateTime] DATETIME2(7) NOT NULL,
	[RowUpdatedDateTime] DATETIME2(7) NOT NULL,
	CONSTRAINT [PK_MarketAuth_tblSecurityRole] PRIMARY KEY CLUSTERED 
	(
		[SecurityRoleID] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRole_SecurityRoleGUID ON MarketAuth.tblSecurityRole (SecurityRoleGUID)
CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRole_Name ON MarketAuth.tblSecurityRole (Name)

GO
CREATE TABLE [MarketAuth].[tblSecurityRoleUser](
	[SecurityRoleUserID] INT IDENTITY(1,1) NOT NULL,
	[SecurityRoleUserGUID] UNIQUEIDENTIFIER NOT NULL,
	[SecurityUserID] INT NOT NULL,
	[SecurityRoleID] INT NOT NULL,
	[RowLoadedDateTime] DATETIME2(7) NOT NULL,
	[RowUpdatedDateTime] DATETIME2(7) NOT NULL,
	CONSTRAINT [PK_MarketAuth_tblSecurityRoleUser] PRIMARY KEY CLUSTERED 
	(
		[SecurityRoleUserID] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRoleUser_SecurityRoleUserGUID ON MarketAuth.tblSecurityRoleUser (SecurityRoleUserGUID)
CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRoleUser_SecurityUserID_SecurityRoleID ON MarketAuth.tblSecurityRoleUser (SecurityUserID, SecurityRoleID)

GO
CREATE TABLE [MarketAuth].[tblSecurityUser](
	[SecurityUserID] INT IDENTITY(1,1) NOT NULL,
	[SecurityUserGUID] UNIQUEIDENTIFIER NOT NULL,
	[RefreshTokenId] UNIQUEIDENTIFIER NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[RowLoadedDateTime] DATETIME2(7) NOT NULL,
	[RowUpdatedDateTime] DATETIME2(7) NOT NULL,
	CONSTRAINT [PK_MarketAuth_tblSecurityUser] PRIMARY KEY CLUSTERED 
	(
		[SecurityUserID] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityUser_SecurityUserGUID ON MarketAuth.tblSecurityUser (SecurityUserGUID)
CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityUser_Email ON MarketAuth.tblSecurityUser (Email)


GO
CREATE TABLE [MarketAuth].[tblSecurityRoleClaim](
	[SecurityRoleClaimID] INT IDENTITY(1,1) NOT NULL,
	[SecurityRoleClaimGUID] UNIQUEIDENTIFIER NOT NULL,
	[SecurityClaimID] INT NOT NULL,
	[SecurityRoleID] INT NOT NULL,
	[RowLoadedDateTime] DATETIME2(7) NOT NULL,
	[RowUpdatedDateTime] DATETIME2(7) NOT NULL,
	CONSTRAINT [PK_MarketAuth_tblSecurityRoleClaim] PRIMARY KEY CLUSTERED 
	(
		[SecurityRoleClaimID] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRoleClaim_SecurityRoleClaimGUID ON MarketAuth.tblSecurityRoleClaim (SecurityRoleClaimGUID)
CREATE UNIQUE NONCLUSTERED INDEX AK_MarketAuth_tblSecurityRoleClaim_SecurityClaimID_SecurityRoleID ON MarketAuth.tblSecurityRoleClaim (SecurityClaimID, SecurityRoleID)
GO

ALTER TABLE [MarketAuth].[tblSecurityRoleUser]  WITH CHECK ADD  CONSTRAINT [FK_MarketAuth_tblSecurityRoleUser_SecurityUserID] FOREIGN KEY([SecurityUserID])
REFERENCES [MarketAuth].[tblSecurityUser] ([SecurityUserID])
GO

ALTER TABLE [MarketAuth].[tblSecurityRoleUser]  WITH CHECK ADD  CONSTRAINT [FK_MarketAuth_tblSecurityRoleUser_SecurityRoleID] FOREIGN KEY([SecurityRoleID])
REFERENCES [MarketAuth].[tblSecurityRole] ([SecurityRoleID])
GO

ALTER TABLE [MarketAuth].[tblSecurityRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_MarketAuth_tblSecurityRoleClaim_SecurityClaimID] FOREIGN KEY([SecurityClaimID])
REFERENCES [MarketAuth].[tblSecurityClaim] ([SecurityClaimID])
GO

ALTER TABLE [MarketAuth].[tblSecurityRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_MarketAuth_tblSecurityRoleClaim_SecurityRoleID] FOREIGN KEY([SecurityRoleID])
REFERENCES [MarketAuth].[tblSecurityRole] ([SecurityRoleID])
GO