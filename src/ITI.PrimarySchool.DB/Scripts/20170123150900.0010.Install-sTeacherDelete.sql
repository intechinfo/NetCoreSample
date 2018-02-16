create proc iti.sTeacherDelete
(
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

    update iti.tClass set TeacherId = 0 where TeacherId = @TeacherId;
    delete from iti.tTeacher where TeacherId = @TeacherId;
	commit;
    return 0;
end;