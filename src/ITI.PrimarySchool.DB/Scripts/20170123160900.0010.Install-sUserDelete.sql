create procedure iti.sUserDelete
(
    @UserId int
)
as
begin
    delete from iti.tPasswordUser where UserId = @UserId;
    delete from iti.tGoogleUser where UserId = @UserId;
    delete from iti.tGithubUser where UserId = @UserId;
    delete from iti.tUser where UserId = @UserId;
    return 0;
end;