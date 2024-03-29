USE [CRUD_OPERATION]
GO
/****** Object:  StoredProcedure [dbo].[USP_DELETE_USER]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harshad Gupta
-- Create date: 24 Jan 2024
-- Description:	SP to delete the use
-- =============================================
CREATE PROCEDURE [dbo].[USP_DELETE_USER]
(
	@USER_ID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_MASTER SET IS_ACTIVE=0, MODIFIED_ON = GETDATE(), MODIFIED_BY = -1 
	WHERE USER_ID = @USER_ID;

	SELECT 1 AS FLAG, 'User Deleted' AS MSG
    
END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_USER_DETAILS_BY_ID]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harshad Gupta
-- Create date: 24 Jan 2024
-- Description:	SP to get the details of user
-- =============================================
CREATE PROCEDURE [dbo].[USP_GET_USER_DETAILS_BY_ID]
(
	@USER_ID INT
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT * FROM USER_MASTER WHERE USER_ID = @USER_ID AND IS_ACTIVE = 1;

END

GO
/****** Object:  StoredProcedure [dbo].[USP_GET_USER_LIST]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harshad Gupta
-- Create date: 24 Jan 2024
-- Description:	Sp to get list of all users
-- =============================================
CREATE PROCEDURE [dbo].[USP_GET_USER_LIST]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM USER_MASTER WHERE IS_ACTIVE = 1;
END

GO
/****** Object:  StoredProcedure [dbo].[USP_SAVE_USER_DETAILS]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harshad Gupta
-- Create date: 24 Jan 2024
-- Description:	Stored Procedure to save the user details
-- =============================================
CREATE PROCEDURE [dbo].[USP_SAVE_USER_DETAILS]
(	@USER_ID int,
	@FULL_NAME VARCHAR(250),
	@MOBILE_NO VARCHAR(20),
	@EMAIL VARCHAR(50),
	@USER_NAME VARCHAR(50),
	@PASSWORD VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @FLAG BIT;
	DECLARE @MSG VARCHAR(100);

	IF(@USER_ID = 0)
	BEGIN
	    INSERT INTO USER_MASTER(FULL_NAME,MOBILE_NO,EMAIL,USER_NAME,PASSWORD,IS_ACTIVE,CREATED_ON,CREATED_BY,MODIFIED_ON,MODIFIED_BY)
		VALUES(@FULL_NAME,@MOBILE_NO,@EMAIL,@USER_NAME,@PASSWORD,1,GETDATE(),-1,GETDATE(),-1)

		SET @FLAG = 1;
		SET @MSG = 'New User Added Successfully';
	END
	ELSE
	BEGIN
		UPDATE USER_MASTER SET 
		FULL_NAME = @FULL_NAME,MOBILE_NO = @MOBILE_NO, EMAIL = @EMAIL, USER_NAME = @USER_NAME, PASSWORD = @PASSWORD
		WHERE USER_ID = @USER_ID

		SET @FLAG = 1;
		SET @MSG = 'User Details updated Successfully';
	END

	SELECT @FLAG AS FLAG, @MSG AS MSG
END

GO
/****** Object:  StoredProcedure [dbo].[USP_USER_LOGIN]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harshad Gupta
-- Create date: 24 Jan 2024
-- Description:	SP to check credential for user
-- =============================================
CREATE PROCEDURE [dbo].[USP_USER_LOGIN]
(
   @USERNAME VARCHAR(250),
   @PASSWORD VARCHAR(250)
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @FULL_NAME VARCHAR(250);
	DECLARE @USER_ID VARCHAR(250);
	DECLARE @FLAG BIT;
	DECLARE @MSG VARCHAR(100);

	IF EXISTS(SELECT 1 FROM USER_MASTER WHERE USER_NAME = @USERNAME AND PASSWORD = @PASSWORD AND IS_ACTIVE = 1)
	BEGIN
		SET @FLAG = 1;
		SET @MSG = 'Login Successful';

		SELECT @FLAG AS FLAG, @MSG AS MSG

		SELECT USER_ID,FULL_NAME,MOBILE_NO,EMAIL,USER_NAME FROM USER_MASTER WHERE USER_NAME = @USERNAME AND PASSWORD = @PASSWORD AND IS_ACTIVE=1;
	END
	ELSE
	BEGIN
		SET @FLAG = 0;
		SET @MSG = 'Invalid Username and Password';
		SET @USER_ID = 0;

		SELECT @FLAG AS FLAG, @MSG AS MSG;
		SELECT 0 AS USER_ID,'' AS FULL_NAME,'' AS MOBILE_NO,'' AS EMAIL,'' AS USER_NAME;
	END
END

GO
/****** Object:  Table [dbo].[USER_MASTER]    Script Date: 24-01-2024 20:28:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[USER_MASTER](
	[USER_ID] [int] IDENTITY(1,1) NOT NULL,
	[FULL_NAME] [varchar](100) NULL,
	[MOBILE_NO] [varchar](20) NULL,
	[EMAIL] [varchar](50) NULL,
	[USER_NAME] [varchar](50) NULL,
	[PASSWORD] [varchar](50) NULL,
	[IS_ACTIVE] [int] NULL,
	[CREATED_ON] [datetime] NULL,
	[CREATED_BY] [int] NULL,
	[MODIFIED_ON] [datetime] NULL,
	[MODIFIED_BY] [int] NULL,
 CONSTRAINT [PK_USER_MASTER] PRIMARY KEY CLUSTERED 
(
	[USER_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
