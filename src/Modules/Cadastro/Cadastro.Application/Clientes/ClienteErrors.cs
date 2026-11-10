using SharedKernel.Domain;

namespace Cadastro.Application.Clientes;

internal static class ClienteErrors
{
    public static readonly Error DocumentoJaExiste = Error.Conflict(
        "Cliente.DocumentoJaExiste",
        "Já existe um cliente cadastrado com este documento.");

    public static readonly Error NaoEncontrado = Error.NotFound(
        "Cliente.NaoEncontrado",
        "Cliente não encontrado.");

    public static readonly Error JaDesativado = Error.Conflict(
        "Cliente.JaDesativado",
        "O cliente já está desativado.");
}
