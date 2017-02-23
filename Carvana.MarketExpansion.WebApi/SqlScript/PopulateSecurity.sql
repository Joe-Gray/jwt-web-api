IF NOT EXISTS(SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
	INSERT INTO AspNetRoles(Id, Name) VALUES ('7D1AD8A9-7C80-484C-8F69-962C96B2308B', 'Admin')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoleClaims WHERE ClaimValue = 'Admin')
BEGIN
	INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId) VALUES ('Security', 'Admin', '7D1AD8A9-7C80-484C-8F69-962C96B2308B')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoles WHERE Name = 'ManageMarket')
BEGIN
	INSERT INTO AspNetRoles(Id, Name) VALUES ('69CE158E-0850-49B6-8F69-13434BD80AAD', 'ManageMarket')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoleClaims WHERE ClaimValue = 'AddMarket')
BEGIN
	INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId) VALUES ('Security', 'AddMarket', '69CE158E-0850-49B6-8F69-13434BD80AAD')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoleClaims WHERE ClaimValue = 'EditMarket')
BEGIN
	INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId) VALUES ('Security', 'EditMarket', '69CE158E-0850-49B6-8F69-13434BD80AAD')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoleClaims WHERE ClaimValue = 'DeleteMarket')
BEGIN
	INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId) VALUES ('Security', 'DeleteMarket', '69CE158E-0850-49B6-8F69-13434BD80AAD')
END

IF NOT EXISTS(SELECT 1 FROM AspNetRoleClaims WHERE ClaimValue = 'ViewMarket')
BEGIN
	INSERT INTO AspNetRoleClaims(ClaimType, ClaimValue, RoleId) VALUES ('Security', 'ViewMarket', '69CE158E-0850-49B6-8F69-13434BD80AAD')
END