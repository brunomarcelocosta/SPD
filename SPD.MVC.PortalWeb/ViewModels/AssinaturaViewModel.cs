using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class AssinaturaViewModel
    {
        public int ID { get; set; }

        public string NOME_RESPONSAVEL { get; set; }

        public string CPF_RESPONSAVEL { get; set; }

        public byte[] ASSINATURA { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}