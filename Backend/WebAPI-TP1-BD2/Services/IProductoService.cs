using WebAPI_TP1_BD2.DTOs;

namespace WebAPI_TP1_BD2.Services;

public interface IProductoService
{
    Task<List<ProductoResponseDto>> GetAllAsync();
    Task<ProductoResponseDto?> GetByIdAsync(string id);
    Task<ProductoResponseDto> CreateAsync(CreateProductoDto dto);
    Task<ProductoResponseDto?> UpdateAsync(string id, UpdateProductoDto dto);
    Task<bool> DeleteAsync(string id);
}
