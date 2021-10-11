use Library2




-- sp_SingInTeachers


Create OR Alter procedure sp_SingInTeachers
@TeacherFirstName as nvarchar(max),
@TeacherPassword as nvarchar(max),
@T_CardsId as int,
@TeacherId_tc as int,
@BookId as int,
@DateOut as datetime,
@DateIn as datetime,
@LibId as int
AS
BEGIN
BEGIN TRANSACTION sp_SingInTeacherstr
  DECLARE @ID INT
  SELECT @ID= Library2.dbo.Teachers.Id 
  from  
  Library2.dbo.Teachers 
  where 
  @TeacherPassword = Library2.dbo.Teachers.Password
  AND
  @TeacherFirstName = Library2.dbo.Teachers.FirstName
  --
  if (NOT EXISTS
  (
  SELECT Library2.dbo.Teachers.Id 
  from  
  Library2.dbo.Teachers 
  where 
  @TeacherPassword = Library2.dbo.Teachers.Password
  AND
  @TeacherFirstName = Library2.dbo.Teachers.FirstName
  )
  )
  BEGIN
     print 'Can not Sing In'
     rollback TRANSACTION sp_SingInTeacherstr
  END
  ELSE
  BEGIN
    if ( EXISTS
    (
    SELECT * 
    from  
    Library2.dbo.T_Cards 
    where 
    @T_CardsId=Library2.dbo.T_Cards.Id
    )
    )
    BEGIN
      print 'Can not Insert'
      rollback TRANSACTION sp_SingInTeacherstr
    END
    else
    BEGIN
	  if(@ID=@TeacherId_tc)
	  BEGIN
        INSERT  Library2.[dbo].[T_Cards] ([Id], [Id_Teacher], [Id_Book], [DateOut], [DateIn], [Id_Lib]) 
        VALUES (@T_CardsId, @TeacherId_tc, @BookId, @DateOut, @DateIn, @LibId)
	    UPDATE Library2.dbo.Books
        SET Library2.dbo.Books.Quantity = Library2.dbo.Books.Quantity -1
	    where @BookId=Library2.dbo.Books.Id
		COMMIT TRANSACTION sp_SingInTeachers_tr
	  END
	  else
	  BEGIN
	     print 'Can not Update'
         rollback TRANSACTION sp_SingInTeacherstr
	  END
    END
  END
END




SELECT * FROM Teachers
SELECT * FROM T_Cards
SELECT * FROM Books
                                            --t_C--t
exec sp_SingInTeachers N'Grigory', N'GJ1234', 500, 1, 10, N'2001-05-05 00:00:00.000', N'2001-06-12 00:00:00.000', 2

SELECT * FROM Teachers
SELECT * FROM T_Cards
SELECT * FROM Books




-- sp_FindTeacherId


Create OR Alter procedure sp_FindTeachersId
@Id INT output,
@TeacherFirstName nvarchar(max),
@TeacherPassword nvarchar(max)
AS
BEGIN
if(not EXISTS(
  select Library2.dbo.Teachers.Id 
  from  
  Library2.dbo.Teachers 
  where 
  @TeacherPassword = Library2.dbo.Teachers.Password
  AND
  @TeacherFirstName = Library2.dbo.Teachers.FirstName
)
)
BEGIN
  PRINT 'Can not find Teacher'
  rollback tran
END
else
BEGIN
  SELECT @Id= Library2.dbo.Teachers.Id 
  from  
  Library2.dbo.Teachers 
  where 
  @TeacherPassword = Library2.dbo.Teachers.Password
  AND
  @TeacherFirstName = Library2.dbo.Teachers.FirstName
  select @Id
END
END




declare @Id2 as int
EXEC sp_FindTeachersId  @Id2 output,'Grigory', 'GJ124'


