create table iti.tUser
(
    UserId int identity(0, 1),
    Email  nvarchar(64) collate Latin1_General_CI_AI not null,

    constraint PK_tUser primary key(UserId),
    constraint UK_tUser_Email unique(Email)
);

insert into iti.tUser(Email) values(N'');
