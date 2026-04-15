using MediatR;
using Roupa.Application.Common;

namespace Roupa.Application.Auth.Commands;

public record RegistrarCommand(string Nome, string Email, string Senha, string ConfirmacaoSenha) : IRequest<Result>;
