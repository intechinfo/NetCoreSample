create procedure iti.sGithubUserUpdate
(
	@UserId      int,
	@AccessToken varchar(64)
)
as
begin
	update iti.tGithubUser set AccessToken = @AccessToken where UserId = @UserId;
	return 0;
end;