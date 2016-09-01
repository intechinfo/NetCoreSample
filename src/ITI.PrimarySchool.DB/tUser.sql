create table iti.tUser
(
	UserId             int identity(0, 1),
	Email              nvarchar(64) not null,
	[Password]         varbinary(128) not null,
	GoogleRefreshToken varchar(64) not null constraint DF_tUser_GoogleRefreshToken default(''),

    constraint PK_tUser primary key(UserId),
	constraint UK_tUser_Email unique(Email)
);