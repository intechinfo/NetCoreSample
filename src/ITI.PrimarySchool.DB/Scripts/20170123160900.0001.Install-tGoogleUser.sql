create table iti.tGoogleUser
(
    UserId       int,
    GoogleId     varchar(32) not null,
    RefreshToken varchar(64) not null,

    constraint PK_tGoogleUser primary key(UserId),
    constraint FK_tGoogleUser_UserId foreign key(UserId) references iti.tUser(UserId),
    constraint UK_tGoogleUser_GoogleId unique(GoogleId)
);

insert into iti.tGoogleUser(UserId, GoogleId, RefreshToken) values(0, 0, '');