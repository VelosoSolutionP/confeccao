using Roupa.Domain.Entities;

namespace Roupa.Domain.Interfaces;

public interface IParceiroRepository
{
    Task<Parceiro?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Parceiro>> ListarAsync(bool apenasAtivos = true, CancellationToken ct = default);
    Task AdicionarAsync(Parceiro parceiro, CancellationToken ct = default);
    void Atualizar(Parceiro parceiro);
}
