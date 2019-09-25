
CREATE  PROCEDURE [dbo].[SP_SELECT_CONSULTA] 
AS

BEGIN

		SELECT 
		      d.dt_consulta as DataConsulta,
			  e.nome as Dentista,
			  d.nm_paciente as Paciente,
			  b.desc_procedimento as Descricao
	    FROM
			SPD_HISTORICO_CONSULTA a,
			SPD_CONSULTA b,
			SPD_PRE_CONSULTA c,
			SPD_AGENDA d,
			SPD_DENTISTA e
	  WHERE a.fk_id_consulta = b.id_consulta
	    and b.fk_id_pre_consulta = c.id_pre_consulta
		and c.fk_id_agenda = d.id_agenda
		and b.fk_id_dentista = e.id_dentista

END


