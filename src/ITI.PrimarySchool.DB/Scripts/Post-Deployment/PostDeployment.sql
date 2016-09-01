/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists(select * from iti.tUser u where u.UserId = 0)
begin
	insert into iti.tUser(Email, [Password]) values(N'', convert(varbinary(128), newid()));
end;

if exists(select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c where c.CONSTRAINT_NAME = 'DF_tUser_GithubAccessToken')
begin
	alter table iti.tUser drop constraint DF_tUser_GithubAccessToken;
end;
