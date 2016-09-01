create view iti.vUser
as
	select u.UserId, u.Email, u.[Password], u.GoogleRefreshToken
	from iti.tUser u
	where u.UserId <> 0;