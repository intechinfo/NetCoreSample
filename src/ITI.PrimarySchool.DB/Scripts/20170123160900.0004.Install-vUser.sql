create view iti.vUser
as
    select UserId = u.UserId,
           Email = u.Email,
           [Password] = case when p.[Password] is null then 0x else p.[Password] end,
           GithubAccessToken = case when gh.AccessToken is null then '' else gh.AccessToken end,
           GithubId = case when gh.GithubId is null then '' else gh.GithubId end,
           GoogleRefreshToken = case when gl.RefreshToken is null then '' else gl.RefreshToken end,
           GoogleId = case when gl.GoogleId is null then '' else gl.GoogleId end
    from iti.tUser u
        left outer join iti.tPasswordUser p on p.UserId = u.UserId
        left outer join iti.tGithubUser gh on gh.UserId = u.UserId
        left outer join iti.tGoogleUser gl on gl.UserId = u.UserId
    where u.UserId <> 0;