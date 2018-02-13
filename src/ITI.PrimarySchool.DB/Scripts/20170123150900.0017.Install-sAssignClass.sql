create proc iti.sAssignClass
(
    @StudentId int,
    @ClassId   int
)
as
begin
    update iti.tStudent
    set ClassId = @ClassId
    where StudentId = @StudentId;
    return 0;
end;