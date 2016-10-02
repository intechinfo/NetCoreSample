create view iti.vAuthenticationProvider
as
	select usr.UserId, usr.ProviderName
	from (select UserId = u.UserId,
			  ProviderName = 'PrimarySchool'
		  from iti.tPasswordUser u
		  union
		  select UserId = u.UserId,
			  ProviderName = 'GitHub'
		  from iti.tGithubUser u
		  union
		  select UserId = u.UserId,
			  ProviderName = 'Google'
		  from iti.tGoogleUser u) usr
	where usr.UserId <> 0;