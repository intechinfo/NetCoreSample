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
	insert into iti.tUser(Email) values(N'');
end;

if not exists(select * from iti.tPasswordUser u where u.UserId = 0)
begin
	insert into iti.tPasswordUser(UserId, [Password]) values(0, convert(varbinary(128), newid()));
end;

if not exists(select * from iti.tGithubUser u where u.UserId = 0)
begin
	insert into iti.tGithubUser(UserId, AccessToken) values(0, '');
end;

if not exists(select * from iti.tGoogleUser u where u.UserId = 0)
begin
	insert into iti.tGoogleUser(UserId, RefreshToken) values(0, '');
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

if not exists(select * from iti.tGitHubStudent s where s.StudentId = 0)
begin
	insert into iti.tGitHubStudent(StudentId, GitHubLogin)
	                        values(0,         N'');
end;

if object_id('tempdb..#tUser') is not null
begin
	insert into iti.tPasswordUser(UserId,   [Password])
						   select u.UserId, u.[Password]
						   from #tUser u
						   where u.[Password] <> 0x0 and u.UserId <> 0;

	insert into iti.tGithubUser(UserId,   AccessToken)
	                     select u.UserId, u.GithubAccessToken
						 from #tUser u
						 where u.GithubAccessToken <> N'' and u.UserId <> 0;

	insert into iti.tGoogleUser(UserId,   RefreshToken)
	                     select u.UserId, u.GoogleRefreshToken
						 from #tUser u
						 where u.GoogleRefreshToken <> N'' and u.UserId <> 0;

	drop table #tUser;
end;