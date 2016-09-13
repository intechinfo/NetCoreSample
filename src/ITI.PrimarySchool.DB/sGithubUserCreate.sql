create procedure iti.sGithubUserCreate
(
	@Email       nvarchar(64),
	@AccessToken varchar(64)
)
as
begin
	insert into iti.tUser(Email) values(@Email);
	declare @userId int;
	select @userId = scope_identity();
	insert into iti.tGithubUser(UserId,  AccessToken)
	                     values(@userId, @AccessToken);
	return 0;
end;