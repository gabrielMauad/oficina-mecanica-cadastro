# oficina-mecanica-cadastro

Microsserviço **Cadastro** do sistema de oficina mecânica (Tech Challenge FIAP/SOAT — Fase 4).
Dono dos dados de **clientes**, **veículos** e **serviços** da oficina, com banco PostgreSQL próprio
(schema `cadastro`). Extraído do módulo `Cadastro` do monólito original, mantendo Clean Architecture
(`Domain` → `Application` → `Adapters` → `Infrastructure`/`Web`) e o host em `src/Bootstrap/Api`.

- Rotas: `/api/v1/clientes`, `/api/v1/veiculos`, `/api/v1/servicos` (papel `Oficina`), incluindo
  `GET /api/v1/clientes/documento/{documento}` (somente dígitos).
- Autenticação: JWT HS256 validado com segredo compartilhado (tokens emitidos pelo serviço de
  autenticação e pela Lambda de autenticação por CPF).
- Técnicas: `/healthz`, `/healthz/live`, `/healthz/ready`, `/openapi/v1.json` e `/scalar`.

## Executar localmente

Este serviço usa a rede Docker externa `oficina-local` e o Jaeger (`oficina-jaeger`), criados pelo
`docker-compose.infra.yml` do repositório `oficina-mecanica-app`.

```bash
# 1. no repositório oficina-mecanica-app: cria a rede oficina-local e sobe LocalStack + Jaeger
docker compose -f docker-compose.infra.yml up -d

# 2. neste repositório: sobe o Cadastro e o PostgreSQL próprio
docker compose up --build
```

| Item | Valor |
|---|---|
| API | http://localhost:8082 (Scalar em `/scalar`) |
| PostgreSQL | `localhost:5433`, database `cadastro`, usuário `oficina` / `oficina-dev-pass` |
| Jaeger | http://localhost:16686 |

As migrations do `CadastroDbContext` são aplicadas na inicialização.

## Testes

```bash
dotnet build OficinaMecanica.Cadastro.slnx
dotnet test OficinaMecanica.Cadastro.slnx
```

Os testes de integração (`tests/IntegrationTests`) usam Testcontainers e exigem Docker em execução.
Para rodar só os testes unitários:

```bash
dotnet test tests/Modules/Cadastro/Cadastro.Domain.Tests
dotnet test tests/Modules/Cadastro/Cadastro.Application.Tests
dotnet test tests/SharedKernel/SharedKernel.Application.Tests
```

## Pipeline

`.github/workflows/ci.yml` — em pull request para `main`: restore, build (Release) e testes com .NET 10
(job `Build & Test`, status check obrigatório).
