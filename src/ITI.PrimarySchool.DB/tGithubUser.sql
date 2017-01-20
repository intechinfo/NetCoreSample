create table iti.tGithubUser
(
	UserId      int,
	AccessToken varchar(64) not null,

	constraint PK_tGithubUser primary key(UserId),
	constraint FK_tGithubUser_UserId foreign key(UserId) references iti.tUser(UserId)
);