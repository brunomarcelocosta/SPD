﻿using Microsoft.AspNet.Identity;
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
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public string Password { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public int TentativasLogin { get; set; }
        public bool isAtivo { get; set; }
        public bool isBloqueado { get; set; }
        public bool TrocaSenhaObrigatoria { get; set; }

        public virtual ICollection<UsuarioFuncionalidade> ListUsuarioFuncionalidade { get; set; }
        public virtual SessaoUsuario SessaoUsuario { get; set; } // UML - (1) Usuario é associado com (1) SessaoUsuario. Virtual para lazy load

        public Usuario()
        {
            this.TentativasLogin = 0;
            this.TrocaSenhaObrigatoria = true;
            this.SessaoUsuario = null;
            this.ListUsuarioFuncionalidade = null;
        }

        public Usuario(SessaoUsuario sessaoUsuario, params UsuarioFuncionalidade[] usuariosPerfis)
            : this()
        {
            this.SessaoUsuario = sessaoUsuario;
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
            this.Password = GenerateNewPassword(8, true, true, true, false);

            novaSenha = this.Password;

            this.Password = GerarHash(this.Password);
            this.TrocaSenhaObrigatoria = true;

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
                return this.Nome;
            }
            set
            {
                this.Nome = value;
            }
        }

        #endregion

        public bool EmailJaCadastrado(String email)
        {

            if (this._UsuarioRepository.EmailJaCadastrado(email))
            {
                return true;
            }

            return false;
        }

        public bool NomeJaUtilizado(String nome)
        {
            if (this._UsuarioRepository.NomeJaUtilizado(nome))
            {
                return true;
            }

            return false;
        }

        public bool LoginJaUtilizado(Usuario novoUsuario)
        {
            return this.Login.Equals(novoUsuario.Login, StringComparison.InvariantCulture);
        }

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
