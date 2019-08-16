using CommonLib.Cache;
using GraphQLAuthorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Models
{
    public class LoginSession : AuthorizationUser
    {
        public string FullName { get; set; }
        public string _id { get; set; }//id của tài khoản đăng nhập
        public List<string> Menus { get; set; }

        public new static LoginSession Current(HttpContext context)
        {

            LoginSession _user = null;
            try
            {
                RedisConnection cache = new RedisConnection();

                string objstr = context.Session.GetString("token");
                if (!string.IsNullOrEmpty(objstr))
                    _user = JsonConvert.DeserializeObject<LoginSession>(objstr);
                else
                {
                    var authHeader = context.Request.Headers["Authorization"];
                    //Console.WriteLine("authHeader:" + authHeader.ToString());
                    if (authHeader != StringValues.Empty)
                    {

                        string accesstoken = authHeader;
                        if (accesstoken.StartsWith("Bearer"))
                        {
                            accesstoken = accesstoken.Substring(7);
                        }
                        _user = cache.GetById<LoginSession>(accesstoken);
                        if (_user != null)
                            context.Session.SetString("token", JsonConvert.SerializeObject(_user));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _user;
        }
    }
}
