create table iti.tGitHubStudent
(
    StudentId   int,
    GitHubLogin nvarchar(32) not null,

    constraint PK_tGitHubStudent primary key(StudentId),
    constraint FK_tGitHubStudent_tStudent foreign key(StudentId) references iti.tStudent(StudentId),
    constraint UK_tGitHubStudent_GitHubLogin unique(GitHubLogin)
);

insert into iti.tGitHubStudent(StudentId, GitHubLogin)
                        values(0,         N'');