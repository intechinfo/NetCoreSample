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

if not exists(select * from iti.tTeacher t where t.TeacherId = 0)
begin
	insert into iti.tTeacher(FirstName,                                LastName)
	                  values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32));
end;

if not exists(select * from iti.tClass c where c.ClassId = 0)
begin
	insert into iti.tClass([Level], Name, TeacherId) values('CP', left(convert(nvarchar(36), newid()), 32), 0);
end;

if not exists(select * from iti.tStudent s where s.StudentId = 0)
begin
	insert into iti.tStudent(FirstName,                                LastName,                                 BirthDate,  ClassId)
	                  values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32), '00010101', 0);
end;

if exists(select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c where c.CONSTRAINT_NAME = 'DF_tUser_GithubAccessToken')
begin
	alter table iti.tUser drop constraint DF_tUser_GithubAccessToken;
end;

if exists(select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE c where c.CONSTRAINT_NAME = 'DF_tUser_GoogleRefreshToken')
begin
	alter table iti.tUser drop constraint DF_tUser_GoogleRefreshToken;
end;