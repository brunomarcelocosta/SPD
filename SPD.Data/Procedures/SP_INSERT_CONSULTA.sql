
CREATE  PROCEDURE [dbo].[SP_INSERT_CONSULTA] 
(	
	@fk_id_pre_consulta int,
	@fk_id_dentista int,
	@desc_procedimento nvarchar(255),
	@odontograma varbinary(max)
)

AS

BEGIN
			DECLARE @id_consulta int;

			 INSERT INTO SPD_CONSULTA
			 (
				fk_id_dentista,
				fk_id_pre_consulta,
				desc_procedimento,
				odontograma,
				exame,
				dt_consulta
			 )
			 VALUES
			 (
				@fk_id_dentista,
				@fk_id_pre_consulta,
				@desc_procedimento,
				@odontograma,
				null,
				GETDATE()
			 )

			 SELECT top(1) 
					@id_consulta = id_consulta 
			 FROM SPD_CONSULTA 
				 WHERE fk_id_pre_consulta = @fk_id_pre_consulta 
				   and fk_id_dentista = @fk_id_dentista

		     INSERT INTO SPD_HISTORICO_CONSULTA 
			 (
			   fk_id_consulta,
			   dt_consulta
			 )
			 VALUES
			 (
			   @id_consulta,
			   GETDATE()
			 )
END


