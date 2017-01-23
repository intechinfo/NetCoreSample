create procedure iti.sGithubUserUpdate
(
    @GithubId    int,
    @AccessToken varchar(64)
)
as
begin
    update iti.tGithubUser set AccessToken = @AccessToken where GithubId = @GithubId;
    return 0;
end;