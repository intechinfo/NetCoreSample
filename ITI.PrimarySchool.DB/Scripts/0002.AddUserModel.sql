create table iti.tUser
(
    UserId int identity(0, 1),
    Email  nvarchar(64) not null,

    constraint PK_tUser primary key(UserId),
    constraint UK_tUser_Email unique(Email)
);

insert into iti.tUser(Email) values(N'');

create table iti.tGoogleUser
(
    UserId       int,
    RefreshToken varchar(64) not null,

    constraint PK_tGoogleUser primary key(UserId),
    constraint FK_tGoogleUser_UserId foreign key(UserId) references iti.tUser(UserId)
);

insert into iti.tGoogleUser(UserId, RefreshToken) values(0, '');

create table iti.tGithubUser
(
    UserId      int,
    AccessToken varchar(64) not null,

    constraint PK_tGithubUser primary key(UserId),
    constraint FK_tGithubUser_UserId foreign key(UserId) references iti.tUser(UserId)
);

insert into iti.tGithubUser(UserId, AccessToken) values(0, '');

create table iti.tPasswordUser
(
    UserId     int,
    [Password] varbinary(128) not null,

    constraint PK_tPasswordUser primary key(UserId),
    constraint FK_tPasswordUser_UserId foreign key(UserId) references iti.tUser(UserId)
);

insert into iti.tPasswordUser(UserId, [Password]) values(0, convert(varbinary(128), newid()));
GO

create view iti.vUser
as
    select UserId = u.UserId,
           Email = u.Email,
           [Password] = case when p.[Password] is null then 0x else p.[Password] end,
           GithubAccessToken = case when gh.AccessToken is null then '' else gh.AccessToken end,
           GoogleRefreshToken = case when gl.RefreshToken is null then '' else gl.RefreshToken end
    from iti.tUser u
        left outer join iti.tPasswordUser p on p.UserId = u.UserId
        left outer join iti.tGithubUser gh on gh.UserId = u.UserId
        left outer join iti.tGoogleUser gl on gl.UserId = u.UserId
    where u.UserId <> 0;
GO

create view iti.vAuthenticationProvider
as
    select usr.UserId, usr.ProviderName
    from (select UserId = u.UserId,
              ProviderName = 'PrimarySchool'
          from iti.tPasswordUser u
          union all
          select UserId = u.UserId,
              ProviderName = 'GitHub'
          from iti.tGithubUser u
          union all
          select UserId = u.UserId,
              ProviderName = 'Google'
          from iti.tGoogleUser u) usr
    where usr.UserId <> 0;
GO

create procedure iti.sGithubUserCreate
(
    @Email       nvarchar(64),
    @AccessToken varchar(64)
)
as
begin
    insert into iti.tUser(Email) values(@Email);
    declare @userId int;
    select @userId = scope_identity();
    insert into iti.tGithubUser(UserId,  AccessToken)
                         values(@userId, @AccessToken);
    return 0;
end;
GO

create procedure iti.sGithubUserUpdate
(
    @UserId      int,
    @AccessToken varchar(64)
)
as
begin
    update iti.tGithubUser set AccessToken = @AccessToken where UserId = @UserId;
    return 0;
end;
GO

create procedure iti.sGoogleUserCreate
(
    @Email        nvarchar(64),
    @RefreshToken varchar(64)
)
as
begin
    insert into iti.tUser(Email) values(@Email);
    declare @userId int;
    select @userId = scope_identity();
    insert into iti.tGoogleUser(UserId,  RefreshToken)
                         values(@userId, @RefreshToken);
    return 0;
end;
GO

create procedure iti.sGoogleUserUpdate
(
    @UserId       int,
    @RefreshToken varchar(64)
)
as
begin
    update iti.tGoogleUser set RefreshToken = @RefreshToken where UserId = @UserId;
    return 0;
end;
GO

create procedure iti.sPasswordUserCreate
(
    @Email    nvarchar(64),
    @Password varbinary(128)
)
as
begin
    insert into iti.tUser(Email) values(@Email);
    declare @userId int;
    select @userId = scope_identity();
    insert into iti.tPasswordUser(UserId,  [Password])
                           values(@userId, @Password);
    return 0;
end;
GO

create procedure iti.sPasswordUserUpdate
(
    @UserId   int,
    @Password varbinary(128)
)
as
begin
    update iti.tPasswordUser set [Password] = @Password where UserId = @UserId;
    return 0;
end;
GO

create procedure iti.sUserAddGithubToken
(
    @UserId      int,
    @AccessToken varchar(64)
)
as
begin
    insert into iti.tGithubUser(UserId,  AccessToken)
                         values(@UserId, @AccessToken);
    return 0;
end;
GO

create procedure iti.sUserAddGoogleToken
(
    @UserId       int,
    @RefreshToken varchar(64)
)
as
begin
    insert into iti.tGoogleUser(UserId,  RefreshToken)
                         values(@UserId, @RefreshToken);
    return 0;
end;
GO

create procedure iti.sUserAddPassword
(
    @UserId   int,
    @Password varbinary(128)
)
as
begin
    insert into iti.tPasswordUser(UserId,  [Password])
                           values(@UserId, @Password);
    return 0;
end;
GO

create procedure iti.sUserDelete
(
    @UserId int
)
as
begin
    delete from iti.tPasswordUser where UserId = @UserId;
    delete from iti.tGoogleUser where UserId = @UserId;
    delete from iti.tGithubUser where UserId = @UserId;
    delete from iti.tUser where UserId = @UserId;
    return 0;
end;
GO

create procedure iti.sUserUpdate
(
    @UserId int,
    @Email  nvarchar(64)
)
as
begin
    update iti.tUser
    set Email = @Email
    where UserId = @UserId;
    return 0;
end;
GO