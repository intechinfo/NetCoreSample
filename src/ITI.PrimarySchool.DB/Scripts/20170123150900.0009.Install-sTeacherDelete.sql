create proc iti.sTeacherDelete
(
    @TeacherId nvarchar(32)
)
as
begin
    update iti.tClass
    set TeacherId = 0
    where TeacherId = @TeacherId;

    delete from iti.tTeacher where TeacherId = @TeacherId;
    return 0;
end;