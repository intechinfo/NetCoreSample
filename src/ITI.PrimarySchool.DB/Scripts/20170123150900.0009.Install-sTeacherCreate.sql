create proc iti.sTeacherCreate
(
    @FirstName nvarchar(32),
    @LastName  nvarchar(32),
	@TeacherId int out
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from iti.tTeacher t where t.FirstName = @FirstName and t.LastName = @LastName)
	begin
		rollback;
		return 1;
	end;

    insert into iti.tTeacher(FirstName, LastName) values(@FirstName, @LastName);
	set @TeacherId = scope_identity();
	commit;
    return 0;
end;