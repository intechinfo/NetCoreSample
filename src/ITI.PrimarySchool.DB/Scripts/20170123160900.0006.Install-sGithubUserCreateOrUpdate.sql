create procedure iti.sGithubUserCreateOrUpdate
(
    @Email       nvarchar(64),
    @GithubId    int,
    @AccessToken varchar(64)
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from iti.tGithubUser u where u.GithubId = @GithubId)
	begin
		update iti.tGithubUser set AccessToken = @AccessToken where GithubId = @GithubId;
		commit;
		return 0;
	end;

    declare @userId int;
	select @userId = u.UserId from iti.tUser u where u.Email = @Email;

	if @userId is null
	begin
		insert into iti.tUser(Email) values(@Email);
		set @userId = scope_identity();
	end;
    
    insert into iti.tGithubUser(UserId,  GithubId,  AccessToken)
                         values(@userId, @GithubId, @AccessToken);
	commit;
    return 0;
end;