using WebAPI_TP1_BD2.Models;

namespace WebAPI_TP1_BD2.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllAsync();
    Task<Producto?> GetByIdAsync(string id);
    Task<Producto> CreateAsync(Producto producto);
    Task<Producto?> UpdateAsync(string id, Producto producto);
    Task<bool> DeleteAsync(string id);
}
