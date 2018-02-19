create view iti.vTeacherClass
as
	select
		TeacherId = t.TeacherId,
		ClassId = coalesce(ClassId, 0),
		[Name] = coalesce([Name], '')
	from iti.tTeacher t
		left outer join iti.tClass c on c.TeacherId = t.TeacherId
	where t.TeacherId <> 0;