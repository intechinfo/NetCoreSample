create procedure iti.sUserCreate
(
	@Email              nvarchar(64),
	@Password           varbinary(128),
	@GithubAccessToken  varchar(64),
	@GoogleRefreshToken varchar(64)
)
as
begin
	insert into iti.tUser(Email,  [Password], GithubAccessToken,  GoogleRefreshToken)
	               values(@Email, @Password,  @GithubAccessToken, @GoogleRefreshToken);
	return 0;
end;