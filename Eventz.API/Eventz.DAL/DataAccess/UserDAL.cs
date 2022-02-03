using Dapper;
using Eventz.BLL.ViewModels;
using Eventz.DAL.Contracts;
using Eventz.DAL.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eventz.DAL
{
   public class UserDAL: DbAccess, IUser
    {
        private readonly IConfiguration _config;
        public UserDAL(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = ClaimUserDetails(user);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(365),
            signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Claim[] ClaimUserDetails(User user)
        {
            return new[] {

                new Claim("UserName", user.FirstName),
                new Claim("UserId", user.UserIdx.ToString()),
                new Claim("Name",user.FirstName+ " " + user.LastName),
            };
        }

        public User GetUser(string username, string password)
        {
            try
            {
                password = CryptoEncrypt.Encrypt(password, true);
                var para = new DynamicParameters();
                para.Add("@Action", "ValidateUser");
                para.Add("@Username", username);
                para.Add("@Password", password);
                return ReturnObject<User>("spGetUser", para);
            }
            catch (Exception)
            {

                throw new Exception("unable to get user");
            }
          
        }

    }
}
