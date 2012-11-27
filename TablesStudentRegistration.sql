/*For Student Registration table I used 'SRID' as auto generated field and separate 'StudentId' field because of following reasons
1) As a best practice we never used IDENTITY field as our Business Key
	(eg:- we cant Rollback, Damage Continuity)
2) In case of we changed the pattern of 'StudentId' generation (Like - 'SID1134'), the order of Rows get changed when inserting values 
because it sorts according to the primary key. So I used IDENTITY field to solve that problem and mark it as primary key

On the other hand to avoid data duplication I created a 'UNIQUE INDEX' on 'Registration' table.
*/

CREATE TABLE dbo.Registration(
	 SRID INT IDENTITY(1,1), 	
	 StudentId INT NOT NULL,
	 StudentName NVARCHAR(150) NOT NULL,
	 DOB DATE NOT NULL,
	 GradePointAvg DECIMAL(4,2),	
	 Active BIT NOT NULL,	 
	CONSTRAINT PK_Registration PRIMARY KEY (SRID)
);

CREATE UNIQUE INDEX UIDX_Registration
ON dbo.Registration (StudentId)

----------------------------------------------------------------------------
CREATE TABLE dbo.StudentIdNumberHolder(	
	StudentId INT,
)

INSERT INTO dbo.StudentIdNumberHolder VALUES(0)