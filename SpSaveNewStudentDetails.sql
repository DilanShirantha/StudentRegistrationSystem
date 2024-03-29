SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Shirantha Fernando>
-- Create date: <2012/11/25>
-- Description:	<This SP used to Save Student Details when user click on 'Save' button on 'Student Details' Form>
-- =============================================
CREATE PROCEDURE [dbo].[SpSaveNewStudentDetails] 
--Use XML type Parameter to get the collection of student data
@StudentDetailCollection XML

	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;	
	
	--Consider this as a Transaction for more Accuracy since several Insertions and modifications take place
	BEGIN TRANSACTION Tx 			
		
		--Consider all action as a one statement(ROLLBACK Purpose)
		SET XACT_ABORT ON
		
		--To Reset StudentID number
		DECLARE @SetStudentID INT
		
		
		--Data Insert into the 'Registration' table using OPENXML
		DECLARE @nHandel INT
		
		EXEC sp_xml_preparedocument @nHandel OUTPUT, @StudentDetailCollection		
		
		INSERT INTO dbo.Registration
           (StudentId
           ,StudentName
           ,DOB
           ,GradePointAvg
           ,Active)          
		SELECT studentId,studentName,dob,gradePointAvg,active
		FROM OPENXML (@nHandel, 'ArrayOfStudentDetailsXml/StudentDetailsXml',2) WITH (studentId INT ,studentName NVARCHAR(150),dob DATE, gradePointAvg DECIMAL(4,2), active BIT)
		
		EXEC sp_xml_removedocument @nHandel				
		
		
		--Get the Last successful 'StudentID' Number from 'Registration' table
		SELECT @SetStudentID = (SELECT MAX(R.StudentId) FROM dbo.Registration R)

		--Reset StudentID Number
		UPDATE dbo.StudentIdNumberHolder SET StudentId = @SetStudentID	
		
		
		IF (@@ERROR <> 0)
			BEGIN				
				-- Rollback the transaction
				ROLLBACK TRANSACTION Tx					
							
				-- Return with the error		
				RETURN -1		
			END
		
		ELSE	
			BEGIN 
				-- Commit the transaction
				COMMIT TRANSACTION Tx	
				
				--On Success return no error
				RETURN 0;
			END	
END
GO