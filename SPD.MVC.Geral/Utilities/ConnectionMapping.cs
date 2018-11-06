using System;
using System.Collections.Generic;
using System.Linq;

namespace SPD.MVC.Geral.Utilities
{
    /// <summary>
    /// Providencia armazenamento e mapeamento de conexões de clientes no sistema.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _Connections = new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _Connections.Count;
            }
        }

        public void Add(T key, string connectionID)
        {
            lock (this._Connections)
            {
                HashSet<string> connections;

                if (!this._Connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();

                    this._Connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionID);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;

            if (this._Connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetConnectionsExcept(T key)
        {
            var selectedConnections = new List<string>();
            var allConnections = new List<HashSet<string>>();

            foreach (var pair in _Connections.Where(internalKey => !internalKey.Key.Equals(key)))
            {
                allConnections.Add(pair.Value);
            }
            
            foreach (var connection in allConnections)
            {
                var connectionBuffer = new String[connection.Count()];

                connection.CopyTo(connectionBuffer);

                selectedConnections.AddRange(connectionBuffer);
            }

            return selectedConnections;
        }

        public void Remove(T key, string connectionID)
        {
            lock (this._Connections)
            {
                HashSet<string> connections;

                if (!this._Connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionID);

                    if (connections.Count == 0)
                    {
                        this._Connections.Remove(key);
                    }
                }
            }
        }
    }
}
