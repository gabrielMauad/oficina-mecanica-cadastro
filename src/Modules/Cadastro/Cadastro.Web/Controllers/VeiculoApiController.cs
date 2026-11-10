using Cadastro.Adapters.Controllers;
using Cadastro.Adapters.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Web.Controllers;

[Authorize(Roles = "Oficina")]
[ApiController]
[Route("api/v1")]
public class VeiculoApiController : ControllerBase
{
    private readonly VeiculoController _caController;

    public VeiculoApiController(VeiculoController caController) => _caController = caController;

    [HttpPost("veiculos")]
    public async Task<IActionResult> Criar([FromBody] CadastrarVeiculoRequest request)
    {
        var result = await _caController.Criar(request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Value.VeiculoId }, result.Value);
    }

    [HttpGet("veiculos")]
    public async Task<IActionResult> Listar()
    {
        var result = await _caController.Listar();
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("veiculos/{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _caController.ObterPorId(id);
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("clientes/{id}/veiculos")]
    public async Task<IActionResult> ObterVeiculosPorClienteId(Guid id)
    {
        var result = await _caController.ObterVeiculosPorClienteId(id);
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }
}
