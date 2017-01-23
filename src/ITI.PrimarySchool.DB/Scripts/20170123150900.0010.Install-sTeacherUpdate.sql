create proc iti.sTeacherUpdate
(
    @TeacherId int,
    @FirstName nvarchar(32),
    @LastName  nvarchar(32)
)
as
begin
    update iti.tTeacher
    set FirstName = @FirstName,
        LastName = @LastName
    where TeacherId = @TeacherId;
    return 0;
end;