create view iti.vUser
as
	select u.UserId, u.Email, u.[Password]
	from iti.tUser u
	where u.UserId <> 0;