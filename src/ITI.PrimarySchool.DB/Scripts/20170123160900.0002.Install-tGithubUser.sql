create table iti.tGithubUser
(
    UserId      int,
    GithubId    int,
    AccessToken varchar(64) not null,

    constraint PK_tGithubUser primary key(UserId),
    constraint FK_tGithubUser_UserId foreign key(UserId) references iti.tUser(UserId),
    constraint UK_tGithubUser_GithubId unique(GithubId)
);

insert into iti.tGithubUser(UserId, GithubId, AccessToken) values(0, 0, '');