create view iti.vClass
as
    select
        ClassId = c.ClassId,
        Name = c.Name,
        [Level] = c.[Level],
        TeacherId = c.TeacherId,
        TeacherFirstName = case when t.TeacherId = 0 then N'' else t.FirstName end,
        TeacherLastName = case when t.TeacherId = 0 then N'' else t.LastName end
    from iti.tClass c
        inner join iti.tTeacher t on t.TeacherId = c.TeacherId
    where c.ClassId <> 0;