using Microsoft.AspNetCore.Mvc;
using WebAPI_TP1_BD2.DTOs;
using WebAPI_TP1_BD2.Models;
using WebAPI_TP1_BD2.Services;

namespace WebAPI_TP1_BD2.Controllers;

[ApiController]
[Route("api/producto/{id}/[controller]")]
public class ProductoDetalleController : ControllerBase
{
    private readonly IProductoDetalleService _service;
    private readonly IProductoService _productoService;

    public ProductoDetalleController(IProductoDetalleService service, IProductoService productoService)
    {
        _service = service;
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<ProductoResponseDto>> Get(string id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto is null)
            return NotFound();
        return Ok(producto);
    }

    [HttpPost("variantes")]
    public async Task<ActionResult> AddVariante(string id, [FromBody] Variante variante)
    {
        var result = await _service.AddVarianteAsync(id, variante);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("variantes/{varianteId}")]
    public async Task<ActionResult> RemoveVariante(string id, string varianteId)
    {
        var result = await _service.RemoveVarianteAsync(id, varianteId);
        return result ? NoContent() : NotFound();
    }

    [HttpPost("reviews")]
    public async Task<ActionResult> AddReview(string id, [FromBody] Review review)
    {
        var result = await _service.AddReviewAsync(id, review);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("reviews/{reviewId}")]
    public async Task<ActionResult> RemoveReview(string id, string reviewId)
    {
        var result = await _service.RemoveReviewAsync(id, reviewId);
        return result ? NoContent() : NotFound();
    }

    [HttpPut("especificaciones")]
    public async Task<ActionResult> UpdateEspecificaciones(string id, [FromBody] Especificaciones especificaciones)
    {
        var result = await _service.UpdateEspecificacionesAsync(id, especificaciones);
        return result ? Ok() : NotFound();
    }

    [HttpPut("disponibilidad")]
    public async Task<ActionResult> UpdateDisponibilidad(string id, [FromBody] Disponibilidad disponibilidad)
    {
        var result = await _service.UpdateDisponibilidadAsync(id, disponibilidad);
        return result ? Ok() : NotFound();
    }
}
