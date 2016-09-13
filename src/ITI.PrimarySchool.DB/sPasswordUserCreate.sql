create procedure iti.sPasswordUserCreate
(
	@Email    nvarchar(64),
	@Password varbinary(128)
)
as
begin
	insert into iti.tUser(Email) values(@Email);
	declare @userId int;
	select @userId = scope_identity();
	insert into iti.tPasswordUser(UserId,  [Password])
	                       values(@userId, @Password);
	return 0;
end;