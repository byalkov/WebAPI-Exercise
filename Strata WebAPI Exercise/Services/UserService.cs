using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System.Linq;

namespace Strata_WebAPI_Exercise.Services
{
    //Simplified authentication service
    public class UserService
    {
        private readonly IRepositoryService DataStoreService;
        
        public UserService(IRepositoryService dataStoreService)
        {
            this.DataStoreService = dataStoreService;
        }

        public Customer ValidateUser(string email, string password)
        {            
            var user = DataStoreService.GetCustomerRepository().FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }
    }
}