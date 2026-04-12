using Microsoft.AspNetCore.Mvc;
using WebAPI_TP1_BD2.DTOs;
using WebAPI_TP1_BD2.Services;

namespace WebAPI_TP1_BD2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductoController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoResponseDto>>> GetAll()
    {
        var productos = await _productoService.GetAllAsync();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoResponseDto>> GetById(string id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto is null)
            return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductoResponseDto>> Create([FromBody] CreateProductoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var producto = await _productoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductoResponseDto>> Update(string id, [FromBody] UpdateProductoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var producto = await _productoService.UpdateAsync(id, dto);
        if (producto is null)
            return NotFound();
        return Ok(producto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var deleted = await _productoService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}
