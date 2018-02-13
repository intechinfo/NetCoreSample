create proc iti.sTeacherCreate
(
    @FirstName nvarchar(32),
    @LastName  nvarchar(32)
)
as
begin
    insert into iti.tTeacher(FirstName, LastName) values(@FirstName, @LastName);
    return 0;
end;