create procedure iti.sUserUpdate
(
	@UserId             int,
	@Email              nvarchar(64),
	@Password           varbinary(128),
	@GithubAccessToken  varchar(64),
	@GoogleRefreshToken varchar(64)
)
as
begin
	update iti.tUser
	set Email = @Email,
	    [Password] = @Password,
	    GithubAccessToken = @GithubAccessToken,
		GoogleRefreshToken = @GoogleRefreshToken
	where UserId = @UserId;
	return 0;
end;