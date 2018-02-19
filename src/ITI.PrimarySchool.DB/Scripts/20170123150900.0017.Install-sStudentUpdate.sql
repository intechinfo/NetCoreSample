create proc iti.sStudentUpdate
(
    @StudentId   int,
    @FirstName   nvarchar(64),
    @LastName    nvarchar(64),
    @BirthDate   datetime2,
    @GitHubLogin nvarchar(64)
)
as
begin
    set transaction isolation level serializable;
	begin tran;

	if not exists(select * from iti.tStudent s where s.StudentId = @StudentId)
	begin
		rollback;
		return 1;
	end;

	if exists(select * from iti.tStudent s where s.StudentId <> @StudentId and s.FirstName = @FirstName and s.LastName = @LastName)
	begin
		rollback;
		return 2;
	end;

	if @GitHubLogin <> N'' and exists(select * from iti.tGitHubStudent s where s.StudentId <> @StudentId and s.GitHubLogin = @GitHubLogin)
	begin
		rollback;
		return 3;
	end;

	update iti.tStudent
	set FirstName = @FirstName, LastName = @LastName, BirthDate = @BirthDate
	where StudentId = @StudentId;

	if(@GitHubLogin <> N'')
    begin
        if exists(select * from iti.tGitHubStudent s where s.StudentId = @StudentId)
        begin
            update iti.tGitHubStudent
            set GitHubLogin = @GitHubLogin
            where StudentId = @StudentId;
        end
        else
        begin
            insert into iti.tGitHubStudent(StudentId, GitHubLogin) values(@StudentId, @GitHubLogin);
        end;
    end
    else
    begin
        if exists(select * from iti.tGitHubStudent s where s.StudentId = @StudentId)
        begin
            delete from iti.tGitHubStudent where StudentId = @StudentId;
        end;
    end;

	commit;
    return 0;
end;