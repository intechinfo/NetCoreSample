create procedure iti.sPasswordUserCreate
(
    @Email    nvarchar(64),
    @Password varbinary(128),
	@UserId   int out
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from iti.tUser u where u.Email = @Email)
	begin
		rollback;
		return 1;
	end;

    insert into iti.tUser(Email) values(@Email);
    select @UserId = scope_identity();
    insert into iti.tPasswordUser(UserId,  [Password])
                           values(@userId, @Password);
	commit;
    return 0;
end;