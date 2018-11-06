using Newtonsoft.Json;
using System;
using System.Net;

namespace SPD.MVC.Geral.Utilities
{
    /// <summary>
    /// Classe utilitária para requisição e parsing de dados de CEP.
    /// </summary>
    public static class WsCEP
    {
        // estrutura do Webservice de CEP
        public class CEPStructure
        {
            public bool erro { get; set; }
            public string erroMsg { get; set; }
            public string cep { get; set; }
            public string logradouro { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string localidade { get; set; }
            public string uf { get; set; }
            public int uf_id { get; set; }
            public string unidade { get; set; }
            public string ibge { get; set; }
            public string gia { get; set; }
            public string pais { get; set; }
        }

        // buscar CEP via Webservice
        public static CEPStructure GetCEP(string cep)
        {
            var url_API = string.Format("https://viacep.com.br/ws/{0}/json/", cep.Replace("-", ""));

            CEPStructure jsonCEP = new CEPStructure() {
                erro = false,
                erroMsg = "",
                cep = "",
                logradouro = "",
                complemento = "",
                bairro = "",
                localidade = "",
                uf = "",
                uf_id = 0,
                unidade = "",
                ibge = "",
                gia = "",
                pais = ""
            };

            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                var responseData = wc.DownloadString(url_API);
                jsonCEP = JsonConvert.DeserializeObject<CEPStructure>(responseData);

                if (jsonCEP.erro)
                {
                    throw new Exception("O servidor remoto retornou um erro: (404) Não Localizado.");
                }

                jsonCEP.pais = "Brasil";
            }
            catch (Exception ex)
            {
                jsonCEP.erro = true;
                jsonCEP.erroMsg = ex.Message;
            }

            return jsonCEP;
        }
    }
}
