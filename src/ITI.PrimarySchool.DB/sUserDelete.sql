create procedure iti.sUserDelete
(
	@UserId int
)
as
begin
	delete from iti.tUser where UserId = @UserId;
	return 0;
end;