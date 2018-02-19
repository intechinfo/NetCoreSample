create proc iti.sStudentDelete
(
    @StudentId int
)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if not exists(select * from iti.tStudent s where s.StudentId = @StudentId)
	begin
		rollback;
		return 1;
	end;

    delete from iti.tGitHubStudent where StudentId = @StudentId;
    delete from iti.tStudent where StudentId = @StudentId;

	commit;
    return 0;
end;