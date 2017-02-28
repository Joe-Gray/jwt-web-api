DECLARE @adminRoleId INT
DECLARE @marketRoleId INT
DECLARE @securityUserId INT
DECLARE @viewMarketRoleId INT
DECLARE @viewMarketClaim INT

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRole WHERE Name = 'Admin')
BEGIN
	INSERT INTO MarketAuth.tblSecurityRole(Name) 
	VALUES ('Admin')
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRole WHERE Name = 'ManageMarket')
BEGIN
	INSERT INTO MarketAuth.tblSecurityRole(Name) 
	VALUES ('ManageMarket')
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRole WHERE Name = 'ViewMarket')
BEGIN
	INSERT INTO MarketAuth.tblSecurityRole(SecurityRoleGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('BE343A29-A5F1-4034-9646-0344F07D1BB7', 'ViewMarket', SYSDATETIME(), SYSDATETIME())
END

SELECT @adminRoleId = SecurityRoleID FROM MarketAuth.tblSecurityRole WHERE Name = 'Admin'
SELECT @marketRoleId = SecurityRoleID FROM MarketAuth.tblSecurityRole WHERE Name = 'ManageMarket'
SELECT @viewMarketRoleId = SecurityRoleID FROM MarketAuth.tblSecurityRole WHERE Name = 'ViewMarket'


IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityClaim WHERE Name = 'Admin')
BEGIN
	
	INSERT INTO MarketAuth.tblSecurityClaim(SecurityClaimGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('978898FD-DF36-442E-B33A-CFA31B325178', 'Admin', SYSDATETIME(), SYSDATETIME())
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityClaim WHERE Name = 'AddMarket')
BEGIN
	
	INSERT INTO MarketAuth.tblSecurityClaim(SecurityClaimGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('BA8076A3-6EFA-4CDD-93FF-1156F3456803', 'AddMarket', SYSDATETIME(), SYSDATETIME())
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityClaim WHERE Name = 'EditMarket')
BEGIN
	INSERT INTO MarketAuth.tblSecurityClaim(SecurityClaimGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('9E4E7F4C-45DA-4602-9682-1635216D989C', 'EditMarket', SYSDATETIME(), SYSDATETIME())
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityClaim WHERE Name = 'DeleteMarket')
BEGIN
	INSERT INTO MarketAuth.tblSecurityClaim(SecurityClaimGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('C134E23F-B33F-491D-95F9-929E2D94DA57', 'DeleteMarket', SYSDATETIME(), SYSDATETIME())
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityClaim WHERE Name = 'ViewMarket')
BEGIN
	INSERT INTO MarketAuth.tblSecurityClaim(SecurityClaimGUID, Name, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('CED9FC91-B19A-42D1-A463-3EB05DE9F502', 'ViewMarket', SYSDATETIME(), SYSDATETIME())
END

SELECT @viewMarketClaim = SecurityClaimID FROM MarketAuth.tblSecurityClaim WHERE Name = 'ViewMarket'

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRoleClaim WHERE SecurityRoleId = @viewMarketRoleId AND SecurityClaimId = @viewMarketClaim)
BEGIN
	INSERT INTO MarketAuth.tblSecurityRoleClaim(SecurityClaimId, SecurityRoleId) 
	VALUES (@viewMarketClaim, @viewMarketRoleId)
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRoleClaim WHERE SecurityRoleId = @adminRoleId)
BEGIN
	INSERT INTO MarketAuth.tblSecurityRoleClaim(SecurityRoleClaimGUID, SecurityClaimId, SecurityRoleId, RowLoadedDateTime, RowUpdatedDateTime) 
	SELECT NEWID(), SecurityClaimId, @adminRoleId, SYSDATETIME(), SYSDATETIME() FROM MarketAuth.tblSecurityClaim
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRoleClaim WHERE SecurityRoleId = @marketRoleId)
BEGIN
	INSERT INTO MarketAuth.tblSecurityRoleClaim(SecurityRoleClaimGUID, SecurityClaimId, SecurityRoleId, RowLoadedDateTime, RowUpdatedDateTime) 
	SELECT NEWID(), SecurityClaimId, @marketRoleId, SYSDATETIME(), SYSDATETIME() FROM MarketAuth.tblSecurityClaim WHERE Name IN ('AddMarket', 'EditMarket', 'DeleteMarket', 'ViewMarket')
END

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityUser WHERE Email = 'test@test.com')
BEGIN
	INSERT INTO MarketAuth.tblSecurityUser(SecurityUserGUID, Email, PasswordHash, RowLoadedDateTime, RowUpdatedDateTime) 
	VALUES ('0279C12B-EBA9-49F6-AF26-030AC2972B3E', 'test@test.com', 'x567edfr5474545ffgfg', SYSDATETIME(), SYSDATETIME())
END

SELECT @securityUserId = SecurityUserID FROM MarketAuth.tblSecurityUser WHERE Email = 'test@test.com'

IF NOT EXISTS(SELECT 1 FROM MarketAuth.tblSecurityRoleUser WHERE SecurityUserId = @securityUserId)
BEGIN
	INSERT INTO MarketAuth.tblSecurityRoleUser(SecurityRoleUserGUID, SecurityUserID, SecurityRoleID, RowLoadedDateTime, RowUpdatedDateTime) 
	SELECT NEWID(), @securityUserId, SecurityRoleID, SYSDATETIME(), SYSDATETIME() FROM MarketAuth.tblSecurityRole
END