create proc iti.sClassUpdate
(
    @ClassId int,
    @Name    nvarchar(32),
    @Level   varchar(3)
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from iti.tClass c where c.ClassId = @ClassId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from iti.tClass c where c.ClassId <> @ClassId and c.[Name] = @Name)
	begin
		rollback;
		return 2;
	end;

    update iti.tClass set [Name] = @Name, [Level] = @Level where ClassId = @ClassId;
	commit;
    return 0;
end;