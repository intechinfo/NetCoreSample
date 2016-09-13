create table iti.tGoogleUser
(
	UserId       int,
	RefreshToken varchar(64) not null,

	constraint PK_tGoogleUser primary key(UserId),
	constraint FK_tGoogleUser_UserId foreign key(UserId) references iti.tUser(UserId)
);