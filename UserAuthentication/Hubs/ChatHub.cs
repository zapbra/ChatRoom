using Microsoft.AspNetCore.SignalR;
using UserAuthentication.DataService;

namespace UserAuthentication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SharedDb _shared;

        public ChatHub(SharedDb shared) => _shared = shared;


    }
}
