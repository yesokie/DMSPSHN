using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoUtil.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Store
{
    public class CustomPersistedGrantStore : IPersistedGrantStore
    {
        //protected IRepository _dbRepository;

        public CustomPersistedGrantStore()
        {
            //_dbRepository = repository;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            //var result = _dbRepository.Where<PersistedGrant>(i => i.SubjectId == subjectId);
            //return Task.FromResult(result.AsEnumerable());
            var result = await BaseProcs.mongo.GetAllAsync<PersistedGrant>(p => p.SubjectId == subjectId);
            return result;
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            //var result = _dbRepository.Single<PersistedGrant>(i => i.Key == key);
            //return Task.FromResult(result);
            var result = await BaseProcs.mongo.GetOneAsync<PersistedGrant>(p => p.Key == key);
            return result;
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            //_dbRepository.Delete<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId);
            //return Task.FromResult(0);
            var result = await BaseProcs.mongo.Remove<PersistedGrant>(p => p.SubjectId == subjectId && p.ClientId == clientId);
            
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            //_dbRepository.Delete<PersistedGrant>(i => i.SubjectId == subjectId && i.ClientId == clientId && i.Type == type);
            //return Task.FromResult(0);
            var result = await BaseProcs.mongo.Remove<PersistedGrant>(p => p.SubjectId == subjectId && p.ClientId == clientId && p.Type == type);
            
        }

        public async Task RemoveAsync(string key)
        {
            //_dbRepository.Delete<PersistedGrant>(i => i.Key == key);
            //return Task.FromResult(0);
            var result = await BaseProcs.mongo.Remove<PersistedGrant>(p => p.Key == key);
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            //_dbRepository.Add<PersistedGrant>(grant);
            //return Task.FromResult(0);
            await BaseProcs.Add(grant);
        }
    }
}
