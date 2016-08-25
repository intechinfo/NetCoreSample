create procedure iti.sUserCreate
(
	@Email    nvarchar(64),
	@Password varchar(32)
)
as
begin
	insert into iti.tUser(Email, [Password]) values(@Email, @Password);
	return 0;
end;