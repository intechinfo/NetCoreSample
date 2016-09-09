create proc iti.sStudentUpdate
(
	@StudentId int,
	@FirstName nvarchar(64),
	@LastName  nvarchar(64),
	@BirthDate datetime2
)
as
begin
	update iti.tStudent
	set FirstName = @FirstName,
	    LastName = @LastName,
		BirthDate = @BirthDate
	where StudentId = @StudentId;
	return 0;
end;