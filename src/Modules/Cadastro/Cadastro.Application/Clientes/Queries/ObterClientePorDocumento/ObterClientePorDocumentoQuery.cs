using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;

public sealed record ObterClientePorDocumentoQuery(string Documento)
    : IRequest<Result<Cliente>>;
