create procedure iti.sGithubUserCreate
(
    @Email       nvarchar(64),
    @GithubId    int,
    @AccessToken varchar(64)
)
as
begin
    insert into iti.tUser(Email) values(@Email);
    declare @userId int;
    select @userId = scope_identity();
    insert into iti.tGithubUser(UserId,  GithubId,  AccessToken)
                         values(@userId, @GithubId, @AccessToken);
    return 0;
end;