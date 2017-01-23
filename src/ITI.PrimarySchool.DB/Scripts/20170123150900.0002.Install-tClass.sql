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