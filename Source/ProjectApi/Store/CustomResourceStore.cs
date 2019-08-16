using IdentityServer4.Models;
using IdentityServer4.Stores;
using MongoUtil.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Store
{
    public class CustomResourceStore : IResourceStore
    {
        //protected IRepository _dbRepository;

        //public CustomResourceStore(IRepository repository)
        //{
        //    _dbRepository = repository;
        //}

        private IEnumerable<ApiResource> GetAllApiResources()
        {
            //return _dbRepository.All<ApiResource>();
            return BaseProcs.mongo.GetAll<ApiResource>();
        }

        private IEnumerable<IdentityResource> GetAllIdentityResources()
        {
            //return _dbRepository.All<IdentityResource>();
            return BaseProcs.mongo.GetAll<IdentityResource>();
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            //if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            //return Task.FromResult(_dbRepository.Single<ApiResource>(a => a.Name == name));
            return await BaseProcs.mongo.GetOneAsync<ApiResource>(a => a.Name == name);
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            //var list = _dbRepository.Where<ApiResource>(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)));

            //return Task.FromResult(list.AsEnumerable());
            return await BaseProcs.mongo.GetAllAsync<ApiResource>(a => a.Scopes.Any(s => scopeNames.Contains(s.Name)));
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            //var list = _dbRepository.Where<IdentityResource>(e => scopeNames.Contains(e.Name));

            //return Task.FromResult(list.AsEnumerable());
            return await BaseProcs.mongo.GetAllAsync<IdentityResource>(e => scopeNames.Contains(e.Name));
        }

        public Resources GetAllResources()
        {
            var result = new Resources(GetAllIdentityResources(), GetAllApiResources());
            return result;
        }

        private Func<IdentityResource, bool> BuildPredicate(Func<IdentityResource, bool> predicate)
        {
            return predicate;
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var result = new Resources(GetAllIdentityResources(), GetAllApiResources());
            return Task.FromResult(result);
        }
    }
}
