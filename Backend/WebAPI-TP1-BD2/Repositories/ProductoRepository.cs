using MongoDB.Driver;
using WebAPI_TP1_BD2.Config;
using WebAPI_TP1_BD2.Models;

namespace WebAPI_TP1_BD2.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly IMongoCollection<Producto> _productos;

    public ProductoRepository(IMongoClient mongoClient, MongoDBSettings settings)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _productos = database.GetCollection<Producto>("productos");
    }

    public async Task<List<Producto>> GetAllAsync()
    {
        return await _productos.Find(_ => true).ToListAsync();
    }

    public async Task<Producto?> GetByIdAsync(string id)
    {
        return await _productos.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Producto> CreateAsync(Producto producto)
    {
        await _productos.InsertOneAsync(producto);
        return producto;
    }

    public async Task<Producto?> UpdateAsync(string id, Producto producto)
    {
        var result = await _productos.ReplaceOneAsync(p => p.Id == id, producto);
        return result.ModifiedCount > 0 ? producto : null;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _productos.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
