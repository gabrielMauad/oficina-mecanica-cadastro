namespace Cadastro.Application.Clientes.Queries.ListarClientes;

public sealed record ClienteListItem(
    Guid Id,
    string Nome,
    string Documento,
    string Email,
    string Telefone,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm
);
