
CREATE PROCEDURE [dbo].[SP_INSERT_HISTORICO_CONSULTA] 
(
	@fk_id_consulta int
)

AS

BEGIN


		     INSERT INTO SPD_HISTORICO_CONSULTA 
			 (
			   fk_id_consulta,
			   dt_consulta
			 )
			 VALUES
			 (
			   @fk_id_consulta,
			   GETDATE()
			 )
END


