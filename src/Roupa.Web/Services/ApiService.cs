using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Roupa.Web.Models;

namespace Roupa.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public ApiService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task PrepararHeaderAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    // ── AUTH ───────────────────────────────────────────────────────
    public async Task<(bool sucesso, TokenResponse? token, string? erro)> LoginAsync(LoginModel model)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<TokenResponse>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao fazer login.");
    }

    public async Task<(bool sucesso, string? erro)> RegistrarAsync(RegistrarModel model)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/registrar", model);
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao registrar.");
    }

    // ── CLIENTES ───────────────────────────────────────────────────
    public async Task<List<ClienteDto>> ListarClientesAsync(bool apenasAtivos = true)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<ClienteDto>>($"api/clientes?apenasAtivos={apenasAtivos}") ?? [];
    }

    public async Task<ClienteDto?> ObterClienteAsync(Guid id)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<ClienteDto>($"api/clientes/{id}");
    }

    public async Task<(bool sucesso, ClienteDto? dados, string? erro)> CriarClienteAsync(CriarClienteModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PostAsJsonAsync("api/clientes", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<ClienteDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao criar cliente.");
    }

    public async Task<(bool sucesso, ClienteDto? dados, string? erro)> AtualizarClienteAsync(Guid id, AtualizarClienteModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PutAsJsonAsync($"api/clientes/{id}", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<ClienteDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao atualizar cliente.");
    }

    public async Task<(bool sucesso, string? erro)> ToggleAtivoClienteAsync(Guid id)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PatchAsync($"api/clientes/{id}/toggle-ativo", null);
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao alterar status.");
    }

    // ── LAYOUTS ────────────────────────────────────────────────────
    public async Task<List<LayoutDto>> ListarLayoutsAsync()
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<LayoutDto>>("api/layouts") ?? [];
    }

    public async Task<List<LayoutDto>> ListarLayoutsPorClienteAsync(Guid clienteId)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<LayoutDto>>($"api/layouts/cliente/{clienteId}") ?? [];
    }

    public async Task<LayoutDto?> ObterLayoutAsync(Guid id)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<LayoutDto>($"api/layouts/{id}");
    }

    public async Task<(bool sucesso, LayoutDto? dados, string? erro)> CriarLayoutAsync(CriarLayoutModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PostAsJsonAsync("api/layouts", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<LayoutDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao criar layout.");
    }

    // ── PARCEIROS ─────────────────────────────────────────────────
    public async Task<List<ParceiroDto>> ListarParceirosAsync(bool apenasAtivos = true)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<ParceiroDto>>($"api/parceiros?apenasAtivos={apenasAtivos}") ?? [];
    }

    public async Task<(bool sucesso, ParceiroDto? dados, string? erro)> CriarParceiroAsync(CriarParceiroModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PostAsJsonAsync("api/parceiros", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<ParceiroDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao criar parceiro.");
    }

    public async Task<(bool sucesso, ParceiroDto? dados, string? erro)> AtualizarParceiroAsync(Guid id, AtualizarParceiroModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PutAsJsonAsync($"api/parceiros/{id}", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<ParceiroDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao atualizar parceiro.");
    }

    public async Task<(bool sucesso, string? erro)> ToggleAtivoParceiroAsync(Guid id)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PatchAsync($"api/parceiros/{id}/toggle-ativo", null);
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao alterar status.");
    }

    public async Task<(bool sucesso, LayoutDto? dados, string? erro)> AtualizarLayoutAsync(Guid id, CriarLayoutModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PutAsJsonAsync($"api/layouts/{id}", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<LayoutDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao atualizar ficha.");
    }

    public async Task<(bool sucesso, string? erro)> ExcluirLayoutAsync(Guid id)
    {
        await PrepararHeaderAsync();
        var resp = await _http.DeleteAsync($"api/layouts/{id}");
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao excluir ficha.");
    }

    // ── PEDIDOS ────────────────────────────────────────────────────
    public async Task<List<PedidoDto>> ListarPedidosAsync()
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<PedidoDto>>("api/pedidos") ?? [];
    }

    public async Task<PedidoDto?> ObterPedidoAsync(Guid id)
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<PedidoDto>($"api/pedidos/{id}");
    }

    public async Task<(bool sucesso, PedidoDto? dados, string? erro)> CriarPedidoAsync(CriarPedidoModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PostAsJsonAsync("api/pedidos", model);
        if (resp.IsSuccessStatusCode)
            return (true, await resp.Content.ReadFromJsonAsync<PedidoDto>(), null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, null, err?.Erro ?? "Erro ao criar pedido.");
    }

    public async Task<(bool sucesso, string? erro)> AdicionarItemAsync(Guid pedidoId, AdicionarItemModel model)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PostAsJsonAsync($"api/pedidos/{pedidoId}/itens", model);
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao adicionar item.");
    }

    public async Task<(bool sucesso, string? erro)> AlterarStatusPedidoAsync(Guid pedidoId, string acao)
    {
        await PrepararHeaderAsync();
        var resp = await _http.PatchAsync($"api/pedidos/{pedidoId}/{acao}", null);
        if (resp.IsSuccessStatusCode) return (true, null);
        var err = await resp.Content.ReadFromJsonAsync<ErroResponse>();
        return (false, err?.Erro ?? "Erro ao alterar status.");
    }
}

public record ErroResponse(string Erro);
