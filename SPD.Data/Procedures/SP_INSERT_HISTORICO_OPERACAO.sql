
CREATE PROCEDURE SP_INSERT_HISTORICO_OPERACAO 
(
	@ip nvarchar(15) null,
	@descricao nvarchar(4000) null,
	@fk_id_usuario int,
	@fk_id_tipo_operacao int,
	@fk_id_funcionalidade int
)

AS

BEGIN

		INSERT INTO SPD_HISTORICO_OPERACAO
		(
			endereco_ip,
			descricao,
			fk_id_usuario,
			fk_id_tipo_operacao,
			fk_id_funcionalidade,
			dt_operacao
		)
		VALUES
		(
			@ip,
			@descricao,
			@fk_id_usuario,
			@fk_id_tipo_operacao,
			@fk_id_funcionalidade,
			getdate()
		)

END

