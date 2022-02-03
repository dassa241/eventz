using Eventz.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventz.DAL.Contracts
{
    public interface IUser
    {
        User GetUser(string username, string password);
        string GenerateJSONWebToken(User user);
    }
}
