using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<ICollection<Address>> GetAllAddressAsync(string userId);
        Task AddAddressAsync(Address address);
        Task DeleteAddressByIdAsync(int id);
        Task UpdateAddressAsync(Address address, int id);
    }
}
