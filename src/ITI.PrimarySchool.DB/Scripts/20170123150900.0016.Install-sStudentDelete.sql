create proc iti.sStudentDelete
(
    @StudentId int
)
as
begin
    delete from iti.tGitHubStudent where StudentId = @StudentId;
    delete from iti.tStudent where StudentId = @StudentId;
    return 0;
end;