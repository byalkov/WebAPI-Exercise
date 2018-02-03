using AutoMapper;
using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Models.DTO;

namespace Strata_WebAPI_Exercise.Mappings
{
    /// <summary>
    /// One way mapping used. Provisioned separation of Entity to DTO and DTO to Entity mappings.
    /// </summary>
    public class MapEntityToDto : Profile
    {
        public MapEntityToDto()
        {
            CreateMap<LineItem, LineItemDTO>();
            CreateMap<ShoppingCart, ShoppingCartDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<Customer, CustomerDTO>();
        }
    }
}