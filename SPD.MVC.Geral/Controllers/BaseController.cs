using SPD.Model.Model;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;

namespace SPD.MVC.Geral.Controllers
{
    public class BaseController : Controller
    {
        protected Context SetupContext(int? usuarioID = null, string enderecoIP = null)
        {
            Context context = null;

            if (usuarioID.HasValue && usuarioID.Value != 0)
            {
                context = new Context() { usuarioID = usuarioID.Value, usuarioIP = enderecoIP, usuarioSessionID = this.Session.SessionID };
            }
            else
            {
                context = new Context() { usuarioIP = enderecoIP, usuarioSessionID = this.Session.SessionID };
            }

            this.SetupInternalContext(this, context);

            return context;
        }

        private void SetupInternalContext(object instance, Context context)
        {
            foreach (var property in instance.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var entity = property.GetValue(instance);

                if (entity == null)
                {
                    throw new Exception("Was not possible to create the context for this application.");
                }
                else
                {
                    var contextProperty = entity.GetType().GetProperty(typeof(Context).Name);

                    if (contextProperty != null)
                    {
                        contextProperty.SetValue(entity, context);
                    }
                }

                this.SetupInternalContext(entity, context);
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        public int ReturnIdade(string dt_nascimento)
        {
            var dt_nasc = Convert.ToDateTime(dt_nascimento);
            int idade = DateTime.Now.Year - dt_nasc.Year;
            if (DateTime.Now.Month < dt_nasc.Month || (DateTime.Now.Month == dt_nasc.Month && DateTime.Now.Day < dt_nasc.Day))
                idade--;

            return idade;
        }

    }
}
