create procedure iti.sGoogleUserCreate
(
	@Email        nvarchar(64),
	@RefreshToken varchar(64)
)
as
begin
	insert into iti.tUser(Email) values(@Email);
	declare @userId int;
	select @userId = scope_identity();
	insert into iti.tGoogleUser(UserId,  RefreshToken)
	                     values(@userId, @RefreshToken);
	return 0;
end;