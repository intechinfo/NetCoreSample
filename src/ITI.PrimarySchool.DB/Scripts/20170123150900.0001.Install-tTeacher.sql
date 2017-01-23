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