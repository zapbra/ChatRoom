using Microsoft.AspNetCore.SignalR;
using UserAuthentication.DataService;

namespace UserAuthentication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessage(string username, string message)
        {
            _logger.LogInformation($"Received message from {username}: {message}");
            try
            {
                _logger.LogInformation("Broadcasting message from {Username} to all clients", username);
                await Clients.All.SendAsync("ReceiveMessage", username, message);
                _logger.LogInformation("Message successfully broadcasted");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error broadcasting message from {username}");
            }

        }

        public async Task SendGroupMessage(string groupName, string username, string message)
        {
            _logger.LogInformation($"Received group message from {username}: {message}");
            try
            {
                _logger.LogInformation($"Broadcasting group message to {groupName} from {username}: {message}");
                await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, username, message);
                _logger.LogInformation("Group message successfully broadcasted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error broadcasting group message to {groupName} from {username}");
            }
        }

        public async Task AddToGroup(string groupName, string username)
        {
            try
            {
                _logger.LogInformation($"{username} is attempting to join group: {groupName}");
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", groupName, username, $"{username} has joined the group {groupName}");
                _logger.LogInformation($"{username} has joined group: {groupName}");
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error connecting user to group {groupName}");
            }

        }

        public async Task RemoveFromGroup(string groupName)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the group {groupName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removed user from group {groupName}");
            }
        }

    }


}
