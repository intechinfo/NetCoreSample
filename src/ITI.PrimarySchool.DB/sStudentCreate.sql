create procedure iti.sStudentCreate
(
	@FirstName nvarchar(64),
	@LastName  nvarchar(64),
	@BirthDate datetime2,
	@ClassId   int
)
as
begin
	insert into iti.tStudent(FirstName, LastName, BirthDate, ClassId) values(@FirstName, @LastName, @BirthDate, @ClassId);
	return 0;
end;