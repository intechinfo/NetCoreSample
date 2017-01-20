create procedure iti.sUserAddPassword
(
	@UserId   int,
	@Password varbinary(128)
)
as
begin
	insert into iti.tPasswordUser(UserId,  [Password])
	                       values(@UserId, @Password);
	return 0;
end;