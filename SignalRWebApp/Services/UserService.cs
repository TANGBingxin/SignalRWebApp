using SignalRWebApp.Models;
using SqlSugar;
using System.Reflection;

namespace SignalRWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly ISqlSugarClient _db;

        public UserService(ISqlSugarClient db)
        {
            _db = db;
        }

        public bool CodeFirst()
        {
            try
            {
                _db.DbMaintenance.CreateDatabase();
                string nspace = "SignalRWebApp.Models";
                Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "SignalRWebApp.dll")
                                     .GetTypes().Where(p => p.Namespace == nspace).ToArray();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables(ass);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public UserInfo GetUser(string name, string password)
        {
            UserInfo userInfo = _db.Queryable<UserInfo>().First(x => x.Name == name);
            if(userInfo == null)
            {
                UserInfo newUser = new UserInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Password = password
                };
                return _db.Insertable(newUser).ExecuteReturnEntity();
            }
            else if(userInfo.Password != password)
            {
                return new UserInfo();
            }
            return userInfo;
        }

        public List<UserMessage> GetMessages(int pageIndex, int pageSize)
        {
            return  _db.Queryable<UserMessage>().ToOffsetPage(pageIndex, pageSize);
        }

    }
}

