using System.Collections.Concurrent;
using UserAuthentication.Models;

namespace UserAuthentication.DataService
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, User> _connections = new ConcurrentDictionary<string, User>();

        public ConcurrentDictionary<string, User> connections => _connections;
    }
}
