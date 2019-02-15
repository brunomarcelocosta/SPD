﻿using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class DentistaService : ServiceBase<Dentista>, IDentistaService
    {
        private readonly IDentistaRepository _DentistaRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IUsuarioRepository _UsuarioRepository;

        public DentistaService(IDentistaRepository dentistaRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository,
                               IUsuarioRepository usuarioRepository)
            : base(dentistaRepository)
        {
            _DentistaRepository = dentistaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _UsuarioRepository = usuarioRepository;
        }

        public bool ExisteDentista(Dentista dentista)
        {
            var list = _DentistaRepository.Query().Where(a => a.CRO.Equals(dentista.CRO)).ToList();

            if (list.Count > 0)
                return true;

            return false;
        }

        public bool ExisteDentistaUsuario(Dentista dentista)
        {
            var list = _DentistaRepository.Query().Where(a => a.ID_USUARIO == dentista.USUARIO.ID).ToList();

            if (list.Count > 0)
                return true;

            return false;
        }

        public bool Insert(Dentista dentista, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                if (ExisteDentista(dentista))
                {
                    resultado = "Já existe dentista cadastrado com este CRO.";
                    return false;
                }

                if (ExisteDentistaUsuario(dentista))
                {
                    resultado = "Já existe dentista vinculado com este usuário.";
                    return false;
                }

                dentista.USUARIO = _UsuarioRepository.GetById(dentista.USUARIO.ID);
                dentista.DT_INSERT = DateTime.Now;

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _DentistaRepository.Add(dentista);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Adicionou o dentista {dentista.NOME}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Dentista);

                    SaveChanges(transactionScope);
                }

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool Update(Dentista dentista, Usuario usuario, out string resultado)
        {
            resultado = "";

            var dentistaBD = _DentistaRepository.Query().Where(a => a.CRO.Equals(dentista.CRO)).FirstOrDefault();
            if (dentistaBD.ID != dentista.ID)
            {
                resultado = "Já existe dentista cadastrado com este CRO.";
                return false;
            }

            try
            {
                dentista.DT_INSERT = DateTime.Now;

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _DentistaRepository.UpdateDentista(dentista);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Atualizou o dentista {dentista.NOME}", usuario, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Dentista);

                    SaveChanges(transactionScope);
                }
                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool Delete(int id, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                var dentista = GetById(id);

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _DentistaRepository.Remove(dentista);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Excluiu o dentista {dentista.NOME}", usuario, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.Dentista);

                    SaveChanges(transactionScope);
                }

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

    }
}
