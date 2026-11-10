using Cadastro.Adapters.Controllers;
using Cadastro.Adapters.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Web.Controllers;

[Authorize(Roles = "Oficina")]
[ApiController]
[Route("api/v1/servicos")]
public class ServicoApiController : ControllerBase
{
    private readonly ServicoController _caController;

    public ServicoApiController(ServicoController caController) => _caController = caController;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] AdicionarServicoRequest request)
    {
        var result = await _caController.Criar(request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Value.ServicoId }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var result = await _caController.Listar();
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _caController.ObterPorId(id);
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPatch("{id}/descricao")]
    public async Task<IActionResult> AtualizarDescricao(Guid id, [FromBody] AtualizarDescricaoRequest request)
    {
        var result = await _caController.AtualizarDescricao(id, request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return Ok(result.Value);
    }

    [HttpPatch("{id}/preco")]
    public async Task<IActionResult> AtualizarPreco(Guid id, [FromBody] AtualizarPrecoRequest request)
    {
        var result = await _caController.AtualizarPreco(id, request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Desativar(Guid id)
    {
        var result = await _caController.Desativar(id);
        if (result.IsFailure) return NotFound(result.Error);
        return NoContent();
    }
}
