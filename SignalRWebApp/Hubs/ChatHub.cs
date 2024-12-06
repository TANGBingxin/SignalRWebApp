using Microsoft.AspNetCore.SignalR;
using SignalRWebApp.Models;
using SqlSugar;

namespace SignalRWebApp.Hubs
{
	//create signalR center
	public class ChatHub : Hub
	{
		private readonly ISqlSugarClient _db;

		public ChatHub(ISqlSugarClient db)
		{
			_db = db;
		}

		public async Task SendMessage(string user,string message)
		{
			//Store message to DB
			DateTime time = DateTime.Now;
			UserMessage userMessage = new UserMessage()
			{
				Id = Guid.NewGuid().ToString(),
                UserName = user,
				Content = message,
				CreateTime = time
        };
			
			_db.Insertable(userMessage).ExecuteCommand();

			//call send method to send message to client
            await Clients.All.SendAsync("ReceiveMessage", user, message, time.ToString());
		}
	} 
}

