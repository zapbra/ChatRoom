using Microsoft.AspNetCore.SignalR;
using UserAuthentication.DataService;

namespace UserAuthentication.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(string message) {
            await Clients.All.SendAsync("messageReceived", message);
        }
    }
}
