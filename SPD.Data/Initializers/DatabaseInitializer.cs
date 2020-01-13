using SPD.Data.Contexts;
using SPD.Data.Utilities;
using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;

namespace SPD.Data.Initializers
{
    /// <summary>
    /// Responsável por fornecer métodos para inicialização do banco de dados da aplicação com gravação de dados padrões
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public sealed class DatabaseInitializer<TContext> : CreateDatabaseIfNotExists<TContext> where TContext : BaseContext<TContext>
    {
        private const string SCHEMAKEY = "###SCHEMA###";

        internal TContext Context { get; private set; }

        public DatabaseInitializer(TContext context)
        {
            this.Context = context;
        }

        private Funcionalidade[] Funcionalidades()
        {
            Funcionalidade login = new Funcionalidade()
            {
                NOME = "Efetuar Login"

            };

            Funcionalidade logoff = new Funcionalidade()
            {
                NOME = "Efetuar Logoff"

            };

            Funcionalidade usuario = new Funcionalidade()
            {
                NOME = "Listar Usuários"

            };

            Funcionalidade editusuario = new Funcionalidade()
            {
                NOME = "Editar Usuários"

            };

            Funcionalidade deleteusuario = new Funcionalidade()
            {
                NOME = "Excluir Usuários"

            };

            Funcionalidade addusuario = new Funcionalidade()
            {
                NOME = "Novos Usuários"

            };

            Funcionalidade descusuario = new Funcionalidade()
            {
                NOME = "Desconectar Usuários"

            };

            Funcionalidade desblusuario = new Funcionalidade()
            {
                NOME = "Desbloquear Usuários"

            };

            Funcionalidade redefinirSenha = new Funcionalidade()
            {
                NOME = "Redefinir Senha"

            };

            Funcionalidade agenda = new Funcionalidade()
            {
                NOME = "Listar Agenda"

            };

            Funcionalidade agendaADD = new Funcionalidade()
            {
                NOME = "Adicionar Agenda"

            };

            Funcionalidade agendaEDIT = new Funcionalidade()
            {
                NOME = "Editar Agenda"

            };

            Funcionalidade agendaDEL = new Funcionalidade()
            {
                NOME = "Excluir Agenda"

            };

            Funcionalidade paciente = new Funcionalidade()
            {
                NOME = "Listar Pacientes"

            };

            Funcionalidade addPaciente = new Funcionalidade()
            {
                NOME = "Adicionar Pacientes"

            };

            Funcionalidade editPaciente = new Funcionalidade()
            {
                NOME = "Editar Pacientes"

            };

            Funcionalidade delPaciente = new Funcionalidade()
            {
                NOME = "Excluir Pacientes"

            };

            Funcionalidade dentista = new Funcionalidade()
            {
                NOME = "Listar Dentistas"

            };

            Funcionalidade addDentista = new Funcionalidade()
            {
                NOME = "Adicionar Dentistas"

            };

            Funcionalidade editDentista = new Funcionalidade()
            {
                NOME = "Editar Dentistas"

            };

            Funcionalidade delDentista = new Funcionalidade()
            {
                NOME = "Excluir Dentistas"

            };

            Funcionalidade preConsulta = new Funcionalidade()
            {
                NOME = "Listar Pré Atendimentos"

            };

            Funcionalidade addPreConsulta = new Funcionalidade()
            {
                NOME = "Iniciar Pré Atendimentos"

            };

            Funcionalidade editPreConsulta = new Funcionalidade()
            {
                NOME = "Editar Pré Atendimentos"

            };

            Funcionalidade delPreConsulta = new Funcionalidade()
            {
                NOME = "Excluir Pré Atendimentos"

            };

            Funcionalidade addconsulta = new Funcionalidade()
            {
                NOME = "Iniciar Consulta"

            };

            Funcionalidade listconsulta = new Funcionalidade()
            {
                NOME = "Listar Consultas"

            };

            Funcionalidade histconsulta = new Funcionalidade()
            {
                NOME = "Histórico de Consultas"
            };

            Funcionalidade historico = new Funcionalidade()
            {
                NOME = "Visualizar Histórico de Operação"

            };

            Funcionalidade notificacao = new Funcionalidade()
            {
                NOME = "Receber Notificações"

            };

            return new Funcionalidade[] { login, logoff, usuario, editusuario, deleteusuario, addusuario, desblusuario, descusuario,
                                          redefinirSenha, agenda,agendaADD,agendaEDIT, agendaDEL, paciente, addPaciente, editPaciente, delPaciente, dentista, addDentista,
                                          editDentista, delDentista,preConsulta, addPreConsulta, editPreConsulta, delPreConsulta, listconsulta,
                                          addconsulta, histconsulta, historico, notificacao };

        }

        private List<UsuarioFuncionalidade> ListUsuarioFuncionalidades(Usuario usuario, Funcionalidade[] funcionalidades)
        {
            var userFuncs = new List<UsuarioFuncionalidade>();

            foreach (var item in funcionalidades)
            {
                userFuncs.Add(new UsuarioFuncionalidade()
                {
                    Usuario = usuario,
                    Funcionalidade = item
                });
            }

            return userFuncs;
        }

        private IEnumerable<TipoOperacao> PreparaTiposOperacoes()
        {
            var tiposOperacoes = new List<TipoOperacao>();

            foreach (var nome in Enum.GetNames(typeof(Model.Enums.SPD_Enums.Tipo_Operacao)))
            {
                tiposOperacoes.Add(new TipoOperacao()
                {
                    CODIGO_TIPO_OPERACAO = nome,
                    DESCRICAO_TIPO_OPERACAO = nome,
                });
            }

            return tiposOperacoes;
        }

        private void ExecuteSqlCommand()
        {
            var scriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var searchPattern = String.Format(CultureInfo.InvariantCulture, "*.{0}.sql", Enum.GetName(this.Context.ContextType.GetType(), this.Context.ContextType));

            foreach (var sqlFilePath in Directory.GetFiles(scriptsPath, searchPattern, SearchOption.TopDirectoryOnly))
            {
                using (var sqlFile = new StreamReader(sqlFilePath))
                {
                    var content = sqlFile.ReadToEnd();

                    if (content.Contains(DatabaseInitializer<TContext>.SCHEMAKEY))
                    {
                        content = content.Replace(DatabaseInitializer<TContext>.SCHEMAKEY, this.Context.Schema);
                    }

                    this.Context.Database.ExecuteSqlCommand(content);
                }
            }
        }

        /// <summary>
        /// Método responsável pela inicialização do sistema
        /// </summary>
        internal void Initialize()
        {
            Usuario usuarioAdministrador = new Usuario()
            {
                NOME = "Administrador",
                EMAIL = "brunomarcelo.1995@gmail.com",
                LOGIN = "adminsis",
                PASSWORD = Usuario.GerarHash("a12345678"),
                TROCA_SENHA_OBRIGATORIA = true,
                IsATIVO = true,
            };

            var funcionalidades = Funcionalidades();
            var usuarioFuncionalidades = ListUsuarioFuncionalidades(usuarioAdministrador, Funcionalidades());
            var tiposOperacoes = this.PreparaTiposOperacoes();

            //this.Context.Set<Funcionalidade>().AddRange(funcionalidades);
            this.Context.Set<Usuario>().Add(usuarioAdministrador);
            this.Context.Set<TipoOperacao>().AddRange(tiposOperacoes);
            this.Context.Set<UsuarioFuncionalidade>().AddRange(usuarioFuncionalidades);

            try
            {
                // Salva as alterações no banco de dados
                this.Context.SaveChanges();

                // Executa scripts sql personalizados
                ExecuteSqlCommand();

                // Chama o restante da seed
                base.Seed(this.Context);
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                ContextDebugger.ShowInDebugConsole(dbEntityValidationException);

                throw dbEntityValidationException;
            }
        }

        protected override void Seed(TContext context)
        {
            this.Initialize();
        }

        //internal void SetUsersAndFuncs(Usuario usuarioAdministrador)
        //{
        //    try
        //    {
        //        var funcionalidades = Funcionalidades();

        //        // Salva as alterações no banco de dados
        //        this.Context.SaveChanges();

        //        // Executa scripts sql personalizados
        //        ExecuteSqlCommand();

        //        // Chama o restante da seed
        //        base.Seed(this.Context);
        //    }
        //    catch (DbEntityValidationException dbEntityValidationException)
        //    {
        //        ContextDebugger.ShowInDebugConsole(dbEntityValidationException);

        //        throw dbEntityValidationException;
        //    }
        //}

    }
}
