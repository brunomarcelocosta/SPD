using SPD.Model.Model;
using SPD.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface.Model
{
    public interface IAgendaRepository : IRepositoryBase<Agenda>
    {
        void Insert(Agenda agenda);

        void Delete(Agenda agenda);
    }
}
