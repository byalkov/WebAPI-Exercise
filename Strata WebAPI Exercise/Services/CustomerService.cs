using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Strata_WebAPI_Exercise.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public CustomerService(IRepositoryService repositoryService) : base(repositoryService)
        {
        }

        /// <summary>
        /// Extract User Sid from Claims token
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns>UserId or Null if the Sid is not in the token</returns>
        public int? GetClaimsUserId(ClaimsPrincipal claimsPrincipal)
        {
            var userIdString = claimsPrincipal.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.Sid))?.Value;

            if (int.TryParse(userIdString, out int userId))
                return userId;
            else return null;
        }

        /// <summary>
        /// Get customer details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            var res = _repositoryService.GetCustomerRepository().FirstOrDefault(x => x.CustomerId == id);
            return res;
        }

        public void UpdateBalance(int userId, double newBalance)
        {
            var res = _repositoryService.GetCustomerRepository().FirstOrDefault(x => x.CustomerId == userId);
            res.AccountBalance = newBalance;
            _repositoryService.UpdateCustomerRepository(res);
        }
    }
}