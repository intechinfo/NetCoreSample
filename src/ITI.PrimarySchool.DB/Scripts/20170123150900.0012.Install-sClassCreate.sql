create proc iti.sClassCreate
(
    @Name      nvarchar(32),
    @Level     varchar(3),
    @TeacherId int,
	@ClassId   int out
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from iti.tClass c where c.[Name] = @Name)
	begin
		rollback;
		return 1;
	end;

	if not exists(select * from iti.tTeacher t where t.TeacherId = @TeacherId)
	begin
		rollback;
		return 2;
	end;

    insert into iti.tClass([Name], [Level], TeacherId) values(@Name, @Level, @TeacherId);
	set @ClassId = scope_identity();
	commit;
    return 0;
end;