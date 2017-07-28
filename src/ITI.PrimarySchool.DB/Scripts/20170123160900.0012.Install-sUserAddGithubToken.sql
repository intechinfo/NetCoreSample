create procedure iti.sUserAddGithubToken
(
    @UserId      int,
    @GithubId    int,
    @AccessToken varchar(64)
)
as
begin
    insert into iti.tGithubUser(UserId,  GithubId,  AccessToken)
                         values(@UserId, @GithubId, @AccessToken);
    return 0;
end;