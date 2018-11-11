using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Usuario : IUser
    {
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public string NOME { get; set; }
        public string EMAIL { get; set; }
        public string LOGIN { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public string PASSWORD { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public int TENTATIVAS_LOGIN { get; set; }
        public bool IsATIVO { get; set; }
        public bool IsBLOQUEADO { get; set; }
        public bool TROCA_SENHA_OBRIGATORIA { get; set; }

        public virtual ICollection<UsuarioFuncionalidade> ListUsuarioFuncionalidade { get; set; }
        public virtual SessaoUsuario sessaoUsuario { get; set; } // UML - (1) Usuario é associado com (1) SessaoUsuario. Virtual para lazy load

        public IList<Funcionalidade> FUNCIONALIDADES
        {
            get
            {
                return (this.ListUsuarioFuncionalidade == null ? new List<Funcionalidade>() : this.ListUsuarioFuncionalidade.Select(usuariosPerfil => usuariosPerfil.FUNCIONALIDADE).ToList());
            }
        }

        public Usuario()
        {
            this.TENTATIVAS_LOGIN = 0;
            this.TROCA_SENHA_OBRIGATORIA = true;
            this.sessaoUsuario = null;
            this.ListUsuarioFuncionalidade = null;
        }

        public Usuario(SessaoUsuario sessaoUsuario, params UsuarioFuncionalidade[] usuariosPerfis)
            : this()
        {
            this.sessaoUsuario = sessaoUsuario;
            this.ListUsuarioFuncionalidade = usuariosPerfis;
        }

        public string GenerateNewPassword(int size = 8, bool upper = false, bool lower = false, bool number = false, bool code = false)
        {
            string password = "";
            while (!password.ToCharArray().Any(char.IsDigit))
            {
                password = "";
                string senha = "";

                string upperCase = "ABCDEFGHIJKLMNOPQRSTUVXZ";
                string lowerCase = "abcdefghijklmnopqrstuvxz";
                string numberCase = "1234567890";
                string codigoCase = "!@#$%&*()-+.,;{[}]^><:|";

                senha += (upper) ? upperCase : "";
                senha += (lower) ? lowerCase : "";
                senha += (number) ? numberCase : "";
                senha += (code) ? codigoCase : "";

                Random randomNumber = new Random();

                for (int i = 0; i < size; i++)
                {
                    var random = randomNumber.Next(0, senha.Length - 1);
                    var aux = senha.Substring(random, 1);

                    password = String.Concat(password, aux);
                }
            }

            return password.Trim();
        }

        public static string GerarHash(string plaintext)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(plaintext)));
        }

        public bool RedefinirSenha(out string novaSenha)
        {
            this.PASSWORD = GenerateNewPassword(8, true, true, true, false);

            novaSenha = this.PASSWORD;

            this.PASSWORD = GerarHash(this.PASSWORD);
            this.TROCA_SENHA_OBRIGATORIA = true;

            return true;
        }

        #region Autenticação Identity

        public string Id
        {
            get
            {
                return this.ID.ToString();
            }
        }

        public string UserName
        {
            get
            {
                return this.NOME;
            }
            set
            {
                this.NOME = value;
            }
        }

        #endregion

        public static string EncryptID(string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecryptID(string cipher)
        {
            var base64EncodedBytes = Convert.FromBase64String(cipher);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
