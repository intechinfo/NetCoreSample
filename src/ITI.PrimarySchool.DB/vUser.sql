create view iti.vUser
as
	select UserId = u.UserId,
	       Email = u.Email,
		   [Password] = u.[Password],
		   GithubAccessToken = u.GithubAccessToken,
		   GoogleRefreshToken = u.GoogleRefreshToken
	from iti.tUser u
	where u.UserId <> 0;