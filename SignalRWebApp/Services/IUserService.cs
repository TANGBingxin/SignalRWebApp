using System;
using SignalRWebApp.Models;
namespace SignalRWebApp.Services
{
	public interface IUserService
	{
		bool CodeFirst();

		UserInfo GetUser(string name, string password);

		List<UserMessage> GetMessages(int pageIndex, int pageSize);
	}
}

