using Cadastro.Adapters.Controllers;
using Cadastro.Adapters.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.Web.Controllers;

[Authorize(Roles = "Oficina")]
[ApiController]
[Route("api/v1/clientes")]
public class ClienteApiController : ControllerBase
{
    private readonly ClienteController _caController;

    public ClienteApiController(ClienteController caController) => _caController = caController;

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CadastrarClienteRequest request)
    {
        var result = await _caController.Criar(request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Value.ClienteId }, result.Value);
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

    [HttpGet("documento/{documento}")]
    public async Task<IActionResult> ObterPorDocumento(string documento)
    {
        var result = await _caController.ObterPorDocumento(documento);
        if (result.IsFailure) return NotFound(result.Error);
        return Ok(result.Value);
    }

    [HttpPatch("{id}/nome")]
    public async Task<IActionResult> AtualizarNome(Guid id, [FromBody] AtualizarNomeRequest request)
    {
        var result = await _caController.AtualizarNome(id, request);
        if (result.IsFailure) return UnprocessableEntity(result.Error);
        return Ok(result.Value);
    }

    [HttpPatch("{id}/telefone")]
    public async Task<IActionResult> AtualizarTelefone(Guid id, [FromBody] AtualizarTelefoneRequest request)
    {
        var result = await _caController.AtualizarTelefone(id, request);
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
