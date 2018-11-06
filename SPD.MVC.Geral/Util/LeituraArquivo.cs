using SPD.MVC.Geral.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace SPD.MVC.Geral.Util
{
    public static class LeituraArquivo
    {

        /// <summary>
        /// Método responsável por ler arquivo Json contendo opções do menu do sistema
        /// </summary>
        /// <returns>Estrutura do menu</returns>
        public static List<MenuViewModel> LerArquivoMenu()
        {
            var serializer = new JavaScriptSerializer();
            var objMenu = new List<MenuViewModel>();

            var nomeArquivo = "SGTAN.MenuSettings.json";
            var arquivo = LerArquivoBase(nomeArquivo);

            if (string.IsNullOrEmpty(arquivo))
            {
                return objMenu;
            }

            using (StreamReader r = new StreamReader(arquivo))
            {
                var json = r.ReadToEnd();
                objMenu = serializer.Deserialize<List<MenuViewModel>>(json);
            }

            return objMenu;
        }

        /// <summary>
        /// Método responsável por ler arquivo Json contendo configurações do sistema
        /// </summary>
        /// <returns>Estrutura de configuração</returns>
        public static ConfigViewModel LerArquivoConfig()
        {
            var serializer = new JavaScriptSerializer();
            var objConfig = new ConfigViewModel();

            var nomeArquivo = "SGTAN.Modulos.json";
            var arquivo = LerArquivoBase(nomeArquivo);

            if (string.IsNullOrEmpty(arquivo))
            {
                return objConfig;
            }

            using (StreamReader r = new StreamReader(arquivo))
            {
                var json = r.ReadToEnd();
                objConfig = serializer.Deserialize<ConfigViewModel>(json);
            }

            return objConfig;
        }

        /// <summary>
        /// Método responsável por retornar caminho completo do arquivo recebido com parâmetro
        /// </summary>
        /// <param name="nomeArquivo">Arquivo a ser lido</param>
        /// <returns>caminho completo do arquivo</returns>
        private static string LerArquivoBase(string nomeArquivo)
        {
            var path = Path.Combine(HostingEnvironment.MapPath("~"), nomeArquivo);
            //var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
            return path;
        }
    }
}
