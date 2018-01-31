using Strata_WebAPI_Exercise.Entities;
using System.Security.Claims;

namespace Strata_WebAPI_Exercise.Interfaces
{
    public interface ICustomerService
    {
        Customer GetCustomer(int id);
        int? GetClaimsUserId(ClaimsPrincipal claimsPrincipal);
        void UpdateBalance(int userId, double v);
    }
}