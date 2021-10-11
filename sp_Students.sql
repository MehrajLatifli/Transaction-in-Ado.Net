use Library2




-- sp_SingInStudents


Create OR Alter procedure sp_SingInStudents
@StudentFirstName as nvarchar(max),
@StudentPassword as nvarchar(max),
@S_CardsId as int,
@StudentId_sc as int,
@BookId as int,
@DateOut as datetime,
@DateIn as datetime,
@LibId as int
AS
BEGIN
BEGIN TRANSACTION sp_SingInStudentstr
  DECLARE @ID INT
  SELECT @ID= Library2.dbo.Students.Id 
  from  
  Library2.dbo.Students 
  where 
  @StudentPassword = Library2.dbo.Students.Password
  AND
  @StudentFirstName = Library2.dbo.Students.FirstName
  --
  if (NOT EXISTS
  (
  SELECT Library2.dbo.Students.Id 
  from  
  Library2.dbo.Students 
  where 
  @StudentPassword = Library2.dbo.Students.Password
  AND
  @StudentFirstName = Library2.dbo.Students.FirstName
  )
  )
  BEGIN
     print 'Can not Sing In'
     rollback TRANSACTION sp_SingInStudentstr
  END
  ELSE
  BEGIN
    if ( EXISTS
    (
    SELECT * 
    from  
    Library2.dbo.S_Cards 
    where 
    @S_CardsId=Library2.dbo.S_Cards.Id
    )
    )
    BEGIN
      print 'Can not Insert'
      rollback TRANSACTION sp_SingInStudentstr
    END
    else
    BEGIN
	  if(@ID=@StudentId_sc)
	  BEGIN
        INSERT  Library2.[dbo].[S_Cards] ([Id], [Id_Student], [Id_Book], [DateOut], [DateIn], [Id_Lib]) 
        VALUES (@S_CardsId, @StudentId_sc, @BookId, @DateOut, @DateIn, @LibId)
	    UPDATE Library2.dbo.Books
        SET Library2.dbo.Books.Quantity = Library2.dbo.Books.Quantity -1
	    where @BookId=Library2.dbo.Books.Id
		COMMIT TRANSACTION sp_SingInStudents_tr
	  END
	  else
	  BEGIN
	     print 'Can not Update'
         rollback TRANSACTION sp_SingInStudentstr
	  END
    END
  END
END




SELECT * FROM Students
SELECT * FROM S_Cards
SELECT * FROM Books
                                               --s_C--s
exec sp_SingInStudents N'Vyacheslav', N'VZ1234', 502, 2, 10, N'2001-05-05 00:00:00.000', N'2001-06-12 00:00:00.000', 2

SELECT * FROM Students
SELECT * FROM S_Cards
SELECT * FROM Books




-- sp_FindStudentId


Create OR Alter procedure sp_FindStudentsId
@Id INT output,
@StudentFirstName nvarchar(max),
@StudentPassword nvarchar(max)
AS
BEGIN
if(not EXISTS(
  select Library2.dbo.Students.Id 
  from  
  Library2.dbo.Students 
  where 
  @StudentPassword = Library2.dbo.Students.Password
  AND
  @StudentFirstName = Library2.dbo.Students.FirstName
)
)
BEGIN
  PRINT 'Can not find Student'
  rollback tran
END
else
BEGIN
  SELECT @Id= Library2.dbo.Students.Id 
  from  
  Library2.dbo.Students 
  where 
  @StudentPassword = Library2.dbo.Students.Password
  AND
  @StudentFirstName = Library2.dbo.Students.FirstName
  select @Id
END
END




declare @Id2 as int
EXEC sp_FindStudentsId  @Id2 output,'Vyacheslav', 'VZ123'












