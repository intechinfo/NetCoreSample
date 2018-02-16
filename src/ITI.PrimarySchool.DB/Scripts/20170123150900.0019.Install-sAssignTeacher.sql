create proc iti.sAssignTeacher
(
    @ClassId   int,
    @TeacherId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from iti.tTeacher t where t.TeacherId = @TeacherId)
	begin
		rollback;
		return 1;
	end;

	if not exists(select * from iti.tClass c where c.ClassId = @ClassId)
	begin
		rollback;
		return 2;
	end;

	if exists(select * from iti.tClass c where c.ClassId = @ClassId and c.TeacherId <> 0 and c.TeacherId <> @TeacherId)
	begin
		rollback;
		return 3;
	end;

    update iti.tClass set TeacherId = 0 where TeacherId = @TeacherId;
    if(@ClassId <> 0) update iti.tClass set TeacherId = @TeacherId where ClassId = @ClassId;
	commit;
    return 0;
end;