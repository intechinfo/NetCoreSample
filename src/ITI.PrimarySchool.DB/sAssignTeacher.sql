create proc iti.sAssignTeacher
(
	@ClassId   int,
	@TeacherId int
)
as
begin
	update iti.tClass
	set TeacherId = @TeacherId
	where ClassId = @ClassId;
	return 0;
end;