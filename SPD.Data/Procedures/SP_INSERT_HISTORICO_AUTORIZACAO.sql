

CREATE PROCEDURE [dbo].[SP_INSERT_HISTORICO_AUTORIZACAO] 
(
	@fk_id_paciente int,
	@assinatura varbinary(max),
	@nome nvarchar(255),
	@cpf nvarchar(23)
)

AS

BEGIN

		DECLARE @ID_ASSINATURA INT;
		DECLARE @count INT;

		SET @count = (SELECT
							COUNT(*)
						FROM 
						   SPD_ASSINATURA a
						WHERE
							 a.cpf_responsavel = @cpf
					  );
		IF(@count > 0)
		BEGIN
		     UPDATE SPD_ASSINATURA SET nm_responsavel = @nome, assinatura = @assinatura WHERE cpf_responsavel = @cpf
		END

		ELSE
		BEGIN
		     INSERT INTO SPD_ASSINATURA 
			 (
			   nm_responsavel,
			   cpf_responsavel,
			   assinatura,
			   dt_insert
			 )
			 VALUES
			 (
			   @nome,
			   @cpf,
			   @assinatura,
			   GETDATE()
			 )
		END


		SET @ID_ASSINATURA = (SELECT MAX(a.id_assinatura) FROM SPD_ASSINATURA a WHERE a.cpf_responsavel = @cpf);

		INSERT INTO SPD_HISTORICO_AUTORIZACAO
		(
			fk_id_paciente,
			fk_id_assinatura,
			dt_insert
		)
		VALUES
		(
			@fk_id_paciente,
			@ID_ASSINATURA,
			getdate()
		)

		SELECT @ID_ASSINATURA

END




