create proc iti.sClassCreate
(
    @Name      nvarchar(32),
    @Level     varchar(3),
    @TeacherId int
)
as
begin
    insert into iti.tClass(Name, [Level], TeacherId) values(@Name, @Level, @TeacherId);
    return 0;
end;