﻿using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IPreConsultaRepository : IRepositoryBase<PreConsulta>
    {
        void UpdatePreConsulta(PreConsulta preConsulta, Agenda agenda);

        void Insert(PreConsulta preConsulta);

        void Delete(PreConsulta preConsulta);
    }
}
