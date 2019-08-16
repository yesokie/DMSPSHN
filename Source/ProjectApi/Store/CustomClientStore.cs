using IdentityServer4.Models;
using MongoUtil.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Store
{
    public class CustomClientStore : IdentityServer4.Stores.IClientStore
    {
        //protected IRepository _dbRepository;

        public CustomClientStore()
        {
            //_dbRepository = repository;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            //var client = _dbRepository.Single<Client>(c => c.ClientId == clientId);
            var client = await BaseProcs.mongo.GetOneAsync<Client>(c => c.ClientId == clientId);
            return client;
            //return Task.FromResult(client);
        }
    }
}
