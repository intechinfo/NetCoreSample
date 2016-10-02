create procedure iti.sUserAddGithubToken
(
	@UserId      int,
	@AccessToken varchar(64)
)
as
begin
	insert into iti.tGithubUser(UserId,  AccessToken)
	                     values(@UserId, @AccessToken);
	return 0;
end;