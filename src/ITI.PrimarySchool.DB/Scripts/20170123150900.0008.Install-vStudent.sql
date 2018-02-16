create view iti.vStudent
as
    select
        StudentId = s.StudentId,
        FirstName = s.FirstName,
        LastName = s.LastName,
        BirthDate = s.BirthDate,
        ClassId = c.ClassId,
        ClassName = case when c.ClassId = 0 then N'' else c.Name end,
        [Level] = case when c.ClassId = 0 then N'' else c.[Level] end,
        TeacherId = t.TeacherId,
        TeacherFirstName = case when t.TeacherId = 0 then N'' else t.FirstName end,
        TeacherLastName = case when t.TeacherId = 0 then N'' else t.LastName end,
        GitHubLogin = case when g.StudentId is null then N'' else g.GitHubLogin end
    from iti.tStudent s
        inner join iti.tClass c on c.ClassId = s.ClassId
        inner join iti.tTeacher t on t.TeacherId = c.TeacherId
        left outer join iti.tGitHubStudent g on g.StudentId = s.StudentId
    where s.StudentId <> 0;