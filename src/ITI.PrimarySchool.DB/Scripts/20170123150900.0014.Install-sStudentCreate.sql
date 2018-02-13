create procedure iti.sStudentCreate
(
    @FirstName   nvarchar(64),
    @LastName    nvarchar(64),
    @BirthDate   datetime2,
    @ClassId     int,
    @GitHubLogin nvarchar(64)
)
as
begin
    declare @studentId int;
    insert into iti.tStudent(FirstName, LastName, BirthDate, ClassId) values(@FirstName, @LastName, @BirthDate, @ClassId);
    set @studentId = scope_identity();
    if(@GitHubLogin <> N'')
    begin
        insert into iti.tGitHubStudent(StudentId, GitHubLogin) values(@studentId, @GitHubLogin);
    end;
    return 0;
end;