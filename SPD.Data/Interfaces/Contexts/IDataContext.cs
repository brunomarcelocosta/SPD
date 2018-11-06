using System;
using System.Data.Entity;

namespace SPD.Data.Interfaces.Contexts
{
    /// <summary>
    /// Interface Context
    /// </summary>
    public interface IDataContext : IDisposable
    {
        DbContext Entity { get; }
        dynamic Context { get; set; }
    }
}
