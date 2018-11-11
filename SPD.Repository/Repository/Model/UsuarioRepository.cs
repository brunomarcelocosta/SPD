﻿using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public bool Bloquear(Usuario usuario)
        {
            usuario.IsBLOQUEADO = true;

            this.Update(usuario);

            return true;
        }

        public void Atualizar(Usuario usuario)
        {
            this.Save(usuario, where => where.ID.Equals(usuario.ID));
        }

        public Usuario GetByLoginSenha(string login, string password)
        {
            // A sensibilidade de caixa alta ou baixa em queries linq para entities é definida na configuração do servidor do bd.
            // Os dois filtros a seguir funcionam como uma solução client side para este problema e que não requer alteração no servidor do banco de dados. 
            // Esta solução causa uma redução no desempenho proporcional ao número de registros com mesmo texto, mas diferença de caixa.
            var caseInsensitiveResult = this.Query().Where(usuario => usuario.LOGIN.Equals(login, StringComparison.InvariantCulture) && usuario.PASSWORD.Equals(password, StringComparison.InvariantCulture)).AsEnumerable();
            var caseSensitiveResult = caseInsensitiveResult.Where(usuario => usuario.LOGIN.Equals(login, StringComparison.InvariantCulture) && usuario.PASSWORD.Equals(password, StringComparison.InvariantCulture)).SingleOrDefault();

            return caseSensitiveResult;
        }

        public Usuario GetByLogin(string login)
        {
            var user = this.QueryAsNoTracking().Where(usuario => usuario.LOGIN.ToUpper() == login.ToUpper()).SingleOrDefault();
            return user;
        }

        public bool EmailJaCadastrado(String email)
        {
            return this.DomainContext.DataContext.Entity.Set<Usuario>().Where(usuario => usuario.EMAIL.ToUpper() == email.ToUpper()).Count() > 0;
        }

        public bool NomeJaUtilizado(string nome)
        {
            return this.DomainContext.DataContext.Entity.Set<Usuario>().Where(usuario => usuario.NOME.ToUpper() == nome.ToUpper()).Count() > 0;
        }

        public bool DesBloquear(Usuario usuario)
        {
            usuario.IsBLOQUEADO = false;

            this.Atualizar(usuario);

            return true;
        }

        public bool IsAtivo(Usuario usuario)
        {
            return QueryAsNoTracking().Where(x => x.ID == usuario.ID).Select(y => y.IsATIVO).FirstOrDefault();
        }

        public bool IsBloqueado(Usuario usuario)
        {
            return this.QueryAsNoTracking().Where(x => x.ID == usuario.ID).Select(y => y.IsBLOQUEADO).FirstOrDefault();
        }

        public Usuario ZerarTentativas(Usuario usuario)
        {
            usuario.TENTATIVAS_LOGIN = 0;

            this.Atualizar(usuario);

            return this.GetById(usuario.ID);

        }

        public Usuario IncrementarTentativas(Usuario usuario)
        {
            usuario.TENTATIVAS_LOGIN++;

            this.Atualizar(usuario);

            return this.GetById(usuario.ID);
        }
    }
}
