using SharedKernel.Domain;

namespace Cadastro.Application.Servicos;

internal static class ServicoErrors
{
    public static readonly Error NomeJaExiste = Error.Conflict(
        "Servico.NomeJaExiste",
        "Já existe um servico cadastrado com este nome.");

    public static readonly Error NaoEncontrado = Error.NotFound(
        "Servico.NaoEncontrado",
        "Servico não encontrado.");

    public static readonly Error JaDesativado = Error.Conflict(
        "Servico.JaDesativado",
        "O servico já está desativado.");
}

