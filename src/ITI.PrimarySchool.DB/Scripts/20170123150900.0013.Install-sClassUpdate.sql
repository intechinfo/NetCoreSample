create proc iti.sClassUpdate
(
    @ClassId int,
    @Name    nvarchar(32),
    @Level   varchar(3)
)
as
begin
    update iti.tClass
    set Name = @Name,
        [Level] = @Level
    where ClassId = @ClassId;
    return 0;
end;