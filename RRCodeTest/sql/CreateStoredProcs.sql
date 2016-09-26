/****** Object:  StoredProcedure [dbo].[GetEntitiesByType]    Script Date: 26/09/2016 1:06:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Indika Katugampala
-- Create date: 25/09/2016
-- Description:	Returns records of the Entity table with matching type
-- =============================================
CREATE PROCEDURE [dbo].[GetEntitiesByType] 
	@type varchar(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		  [Id]
		, [Created]
		, [Type]
		, [Content]
	FROM
		dbo.Entity
	WHERE
		[Type] = @type
END

GO


