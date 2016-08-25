create procedure iti.sUserUpdate
(
	@UserId   int,
	@Email    nvarchar(64),
	@Password varchar(32)
)
as
begin
	update iti.tUser set Email = @Email, [Password] = @Password where UserId = @UserId;
	return 0;
end;