create procedure iti.sStudentCreate
(
    @FirstName   nvarchar(64),
    @LastName    nvarchar(64),
    @BirthDate   datetime2,
    @GitHubLogin nvarchar(64),
	@StudentId   int out
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	if exists(select * from iti.tStudent s where s.FirstName = @FirstName and s.LastName = @LastName)
	begin
		rollback;
		return 1;
	end;

	if @GitHubLogin <> N'' and exists(select * from iti.tGitHubStudent s where s.GitHubLogin = @GitHubLogin)
	begin
		rollback;
		return 2;
	end;

	insert into iti.tStudent(FirstName, LastName, BirthDate, ClassId)
	                  values(@FirstName, @LastName, @BirthDate, 0);
	set @StudentId = scope_identity();
	if @GitHubLogin <> N'' insert into iti.tGitHubStudent(StudentId, GitHubLogin) values(@StudentId, @GitHubLogin);

	commit;
	return 0;
end;