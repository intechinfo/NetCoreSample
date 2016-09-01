create procedure iti.sUserCreate
(
	@Email              nvarchar(64),
	@Password           varbinary(128),
	@GoogleRefreshToken varchar(64)
)
as
begin
	insert into iti.tUser(Email, [Password], GoogleRefreshToken) values(@Email, @Password, @GoogleRefreshToken);
	return 0;
end;