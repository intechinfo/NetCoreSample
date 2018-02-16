create proc iti.sClassDelete
(
    @ClassId   int
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

    update iti.tStudent set ClassId = 0 where ClassId = @ClassId;
    delete from iti.tClass where ClassId = @ClassId;
	commit;
    return 0;
end;