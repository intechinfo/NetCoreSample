create proc iti.sAssignTeacher
(
    @ClassId   int,
    @TeacherId int
)
as
begin
    update iti.tClass
    set TeacherId = 0
    where TeacherId = @TeacherId;

    if(@ClassId <> 0)
    begin
        update iti.tClass
        set TeacherId = @TeacherId
        where ClassId = @ClassId;
    end;
    return 0;
end;