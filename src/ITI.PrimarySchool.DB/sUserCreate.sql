create procedure iti.sUserCreate
(
	@Email             nvarchar(64),
	@Password          varbinary(128),
	@GithubAccessToken varchar(64)
)
as
begin
	insert into iti.tUser(Email, [Password], GithubAccessToken) values(@Email, @Password, @GithubAccessToken);
	return 0;
end;