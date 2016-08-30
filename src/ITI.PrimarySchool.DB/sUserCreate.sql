create procedure iti.sUserCreate
(
	@Email    nvarchar(64),
	@Password varbinary(128)
)
as
begin
	insert into iti.tUser(Email, [Password]) values(@Email, @Password);
	return 0;
end;