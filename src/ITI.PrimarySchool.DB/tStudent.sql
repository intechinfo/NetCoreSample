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