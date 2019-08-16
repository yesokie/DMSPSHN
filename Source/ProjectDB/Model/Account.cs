using MongoDB.Bson.Serialization.Attributes;
using MongoUtil.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    public class Account
    {
        [BsonId]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string NationId { get; set; }//CMT
        public bool Mariaged { get; set; }
        protected string PhoneNumber;
        public DateTime CreatedTime { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
