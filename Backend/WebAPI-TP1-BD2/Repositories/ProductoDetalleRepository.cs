using MongoDB.Driver;
using WebAPI_TP1_BD2.Config;
using WebAPI_TP1_BD2.Models;

namespace WebAPI_TP1_BD2.Repositories;

public class ProductoDetalleRepository : IProductoDetalleRepository
{
    private readonly IMongoCollection<Producto> _productos;

    public ProductoDetalleRepository(IMongoClient mongoClient, MongoDBSettings settings)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _productos = database.GetCollection<Producto>("productos");
    }

    public async Task<Producto?> GetProductoAsync(string id)
    {
        return await _productos.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> AddVarianteAsync(string id, Variante variante)
    {
        var update = Builders<Producto>.Update.Push(p => p.Variantes, variante);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> RemoveVarianteAsync(string id, string varianteId)
    {
        var update = Builders<Producto>.Update.PullFilter(p => p.Variantes, v => v.id == varianteId);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> AddReviewAsync(string id, Review review)
    {
        var update = Builders<Producto>.Update.Push(p => p.Reviews, review);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> RemoveReviewAsync(string id, string reviewId)
    {
        var update = Builders<Producto>.Update.PullFilter(p => p.Reviews, r => r.Id == reviewId);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateEspecificacionesAsync(string id, Especificaciones especificaciones)
    {
        var update = Builders<Producto>.Update.Set(p => p.Especificaciones, especificaciones);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateDisponibilidadAsync(string id, Disponibilidad disponibilidad)
    {
        var update = Builders<Producto>.Update.Set(p => p.Disponibilidad, disponibilidad);
        var result = await _productos.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }
}
