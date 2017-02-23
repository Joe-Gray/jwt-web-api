DROP TABLE [MarketSecurityUserRole]
DROP TABLE [MarketSecurityClaim]
DROP TABLE [MarketSecurityRole]
DROP TABLE [MarketSecurityUser]

GO
CREATE TABLE [dbo].[MarketSecurityClaim](
	[Id] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[MarketSecurityRoleId] [nvarchar](100) NOT NULL,
	CONSTRAINT [PKMarketSecurityClaim] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
) 
GO

CREATE UNIQUE NONCLUSTERED INDEX [IUXMarketSecurityClaimName] ON [dbo].[MarketSecurityClaim]
(
	[Name] ASC
)
GO

CREATE NONCLUSTERED INDEX [IXMarketSecurityClaimMarketSecurityRoleId] ON [dbo].[MarketSecurityClaim]
(
	[MarketSecurityRoleId] ASC
)
GO

CREATE TABLE [dbo].[MarketSecurityRole](
	[Id] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NULL
	CONSTRAINT [PKMarketSecurityRole] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IUXMarketSecurityRoleName] ON [dbo].[MarketSecurityRole]
(
	[Name] ASC
)
GO


CREATE TABLE [dbo].[MarketSecurityUserRole](
	[MarketSecurityUserId] [nvarchar](100) NOT NULL,
	[MarketSecurityRoleId] [nvarchar](100) NOT NULL,
	CONSTRAINT [PKMarketSecurityUserRole] PRIMARY KEY CLUSTERED 
	(
		[MarketSecurityUserId] ASC,
		[MarketSecurityRoleId] ASC
	)
)
GO

CREATE NONCLUSTERED INDEX [IXMarketSecurityUserRoleMarketSecurityUserId] ON [dbo].[MarketSecurityUserRole]
(
	[MarketSecurityUserId] ASC
)
GO

CREATE NONCLUSTERED INDEX [IXMarketSecurityUserRoleMarketSecurityRoleId] ON [dbo].[MarketSecurityUserRole]
(
	[MarketSecurityRoleId] ASC
)
GO

CREATE TABLE [dbo].[MarketSecurityUser](
	[Id] [nvarchar](100) NOT NULL,
	[RefreshTokenId] [nvarchar](100) NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	CONSTRAINT [PKMarketSecurityUser] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UIXMarketSecurityUserEmail] ON [dbo].[MarketSecurityUser]
(
	[Email] ASC
)
GO


ALTER TABLE [dbo].[MarketSecurityClaim]  WITH CHECK ADD  CONSTRAINT [FKMarketSecurityClaimMarketSecurityRoleId] FOREIGN KEY([MarketSecurityRoleId])
REFERENCES [dbo].[MarketSecurityRole] ([Id])
GO

ALTER TABLE [dbo].[MarketSecurityUserRole]  WITH CHECK ADD  CONSTRAINT [FKMarketSecurityUserRoleMarketSecurityUserId] FOREIGN KEY([MarketSecurityUserId])
REFERENCES [dbo].[MarketSecurityUser] ([Id])
GO

ALTER TABLE [dbo].[MarketSecurityUserRole]  WITH CHECK ADD  CONSTRAINT [FKMarketSecurityUserRoleMarketSecurityRoleId] FOREIGN KEY([MarketSecurityRoleId])
REFERENCES [dbo].[MarketSecurityRole] ([Id])
GO


