using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPD.Repository.Hubs
{
    public interface IUserConnectionRepository
    {
        void AddOrUpdate(UserConnection item);

        void Remove(UserConnection item);

        LiteCollection<UserConnection> All();
    }

    public class UserConnectionRepository : IUserConnectionRepository, IDisposable
    {
        protected LiteDatabase _db;
        protected LiteCollection<UserConnection> _userConnections;
        protected MemoryStream _mem;


        public UserConnectionRepository()
        {
            _db = new LiteDatabase(Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\LiteDB.db"));
            //_mem = new MemoryStream();
            //_db = new LiteDatabase(_mem);

            // Get user connections collection
            _userConnections = _db.GetCollection<UserConnection>("UserConnections");
        }

        public void AddOrUpdate(UserConnection item)
        {

            if (item.Id == 0)
            {
                _userConnections.Insert(item);
            }
            else
            {
                _userConnections.Update(item);
            }
        }

        public void Remove(UserConnection item)
        {
            _userConnections.Delete(item.Id);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public LiteCollection<UserConnection> All()
        {
            return _userConnections;
        }
    }

    public class UserConnection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string ConnectionId { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
