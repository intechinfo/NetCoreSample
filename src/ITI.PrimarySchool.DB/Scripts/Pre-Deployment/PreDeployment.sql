/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if exists(select *
          from INFORMATION_SCHEMA.COLUMNS c
          where c.TABLE_SCHEMA = 'iti'
            and c.TABLE_NAME = 'tUser'
            and c.COLUMN_NAME = 'Password')
begin
	create table #tUser
	(
		UserId             int not null,
		Email              nvarchar(64) not null,
		[Password]         varbinary(128) not null,
		GithubAccessToken  varchar(64) not null,
		GoogleRefreshToken varchar(64) not null
	);

	insert into #tUser(UserId,   Email,   [Password],   GithubAccessToken,   GoogleRefreshToken)
				select u.UserId, u.Email, u.[Password], u.GithubAccessToken, u.GoogleRefreshToken
				from iti.tUser u;
end;