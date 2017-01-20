create schema iti;
GO

create table iti.tTeacher
(
    TeacherId int identity(0, 1),
    FirstName nvarchar(32) not null,
    LastName  nvarchar(32) not null,

    constraint PK_tTeacher primary key(TeacherId),
    constraint UK_tTeacher_FirstName_LastName unique(FirstName, LastName),
    constraint CK_tTeacher_FirstName check(FirstName <> N''),
    constraint CK_tTeacher_LastName check(LastName <> N'')
);

insert into iti.tTeacher(FirstName,                                LastName)
                  values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32));

create table iti.tClass
(
    ClassId   int identity(0, 1),
    [Level]   varchar(3) not null,
    Name      nvarchar(32) not null,
    TeacherId int not null,

    constraint PK_tClass primary key(ClassId),
    constraint UK_tClass_Name unique(Name),
    constraint CK_tClass_Level check([Level] in ('CP', 'CE1', 'CE2', 'CM1', 'CM2')),
    constraint FK_tClass_tTeacher foreign key(TeacherId) references iti.tTeacher(TeacherId)
);
GO

create unique index IX_tClass_TeacherId on iti.tClass(TeacherId) where TeacherId <> 0;

insert into iti.tClass([Level], Name, TeacherId) values('CP', left(convert(nvarchar(36), newid()), 32), 0);

create table iti.tStudent
(
    StudentId int identity(0, 1),
    FirstName nvarchar(32) not null,
    LastName  nvarchar(32) not null,
    BirthDate datetime2 not null,
    ClassId   int not null,

    constraint PK_tStudent primary key(StudentId),
    constraint FK_tStudent_tClass foreign key(ClassId) references iti.tClass(ClassId),
    constraint UK_tStudent_FirstName_LastName unique(FirstName, LastName),
    constraint CK_tStudent_FirstName check(FirstName <> N''),
    constraint CK_tStudent_LastName check(LastName <> N'')
);

insert into iti.tStudent(FirstName,                                LastName,                                 BirthDate,  ClassId)
                  values(left(convert(nvarchar(36), newid()), 32), left(convert(nvarchar(36), newid()), 32), '00010101', 0);

create table iti.tGitHubStudent
(
    StudentId   int,
    GitHubLogin nvarchar(32) not null,

    constraint PK_tGitHubStudent primary key(StudentId),
    constraint FK_tGitHubStudent_tStudent foreign key(StudentId) references iti.tStudent(StudentId),
    constraint UK_tGitHubStudent_GitHubLogin unique(GitHubLogin)
);

insert into iti.tGitHubStudent(StudentId, GitHubLogin)
                        values(0,         N'');
GO

create view iti.vTeacher
as
    select
        TeacherId = t.TeacherId,
        FirstName = t.FirstName,
        LastName = t.LastName
    from iti.tTeacher t
    where t.TeacherId <> 0;
GO

create view iti.vClass
as
    select
        ClassId = c.ClassId,
        Name = c.Name,
        [Level] = c.[Level],
        TeacherId = c.TeacherId,
        TeacherFirstName = case when t.TeacherId = 0 then N'' else t.FirstName end,
        TeacherLastName = case when t.TeacherId = 0 then N'' else t.LastName end
    from iti.tClass c
        inner join iti.tTeacher t on t.TeacherId = c.TeacherId
    where c.ClassId <> 0;
GO

create view iti.vStudent
as
    select
        StudentId = s.StudentId,
        FirstName = s.FirstName,
        LastName = s.LastName,
        BirthDate = s.BirthDate,
        ClassId = c.ClassId,
        ClassName = case when c.ClassId = 0 then N'' else c.Name end,
        [Level] = case when c.ClassId = 0 then N'' else c.[Level] end,
        TeacherId = t.TeacherId,
        TeacherFirstName = case when t.TeacherId = 0 then N'' else t.FirstName end,
        TeacherLastName = case when t.TeacherId = 0 then N'' else t.LastName end,
        GitHubLogin = case when g.StudentId is null then N'' else g.GitHubLogin end
    from iti.tStudent s
        inner join iti.tClass c on c.ClassId = s.ClassId
        inner join iti.tTeacher t on t.TeacherId = c.TeacherId
        left outer join iti.tGitHubStudent g on g.StudentId = s.StudentId
    where s.StudentId <> 0;
GO

create proc iti.sTeacherCreate
(
    @FirstName nvarchar(32),
    @LastName  nvarchar(32)
)
as
begin
    insert into iti.tTeacher(FirstName, LastName) values(@FirstName, @LastName);
    return 0;
end;
GO

create proc iti.sTeacherDelete
(
    @TeacherId nvarchar(32)
)
as
begin
    update iti.tClass
    set TeacherId = 0
    where TeacherId = @TeacherId;

    delete from iti.tTeacher where TeacherId = @TeacherId;
    return 0;
end;
GO

create proc iti.sTeacherUpdate
(
    @TeacherId int,
    @FirstName nvarchar(32),
    @LastName  nvarchar(32)
)
as
begin
    update iti.tTeacher
    set FirstName = @FirstName,
        LastName = @LastName
    where TeacherId = @TeacherId;
    return 0;
end;
GO

create proc iti.sClassCreate
(
    @Name      nvarchar(32),
    @Level     varchar(3),
    @TeacherId int
)
as
begin
    insert into iti.tClass(Name, [Level], TeacherId) values(@Name, @Level, @TeacherId);
    return 0;
end;
GO

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
GO

create proc iti.sClassUpdate
(
    @ClassId int,
    @Name    nvarchar(32),
    @Level   varchar(3)
)
as
begin
    update iti.tClass
    set Name = @Name,
        [Level] = @Level
    where ClassId = @ClassId;
    return 0;
end;
GO

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
GO

create proc iti.sStudentDelete
(
    @StudentId int
)
as
begin
    delete from iti.tGitHubStudent where StudentId = @StudentId;
    delete from iti.tStudent where StudentId = @StudentId;
    return 0;
end;
GO

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
    update iti.tStudent
    set FirstName = @FirstName,
        LastName = @LastName,
        BirthDate = @BirthDate
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
    return 0;
end;
GO

create proc iti.sAssignClass
(
    @StudentId int,
    @ClassId   int
)
as
begin
    update iti.tStudent
    set ClassId = @ClassId
    where StudentId = @StudentId;
    return 0;
end;
GO

create proc iti.sAssignTeacher
(
    @ClassId   int,
    @TeacherId int
)
as
begin
    update iti.tClass
    set TeacherId = 0
    where TeacherId = @TeacherId;

    if(@ClassId <> 0)
    begin
        update iti.tClass
        set TeacherId = @TeacherId
        where ClassId = @ClassId;
    end;
    return 0;
end;
GO