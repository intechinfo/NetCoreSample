create proc iti.sClassDelete
(
    @ClassId   int
)
as
begin
    update iti.tStudent
    set ClassId = 0
    where ClassId = @ClassId;

    delete from iti.tClass where ClassId = @ClassId;
    return 0;
end;