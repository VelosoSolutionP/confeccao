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
    public async Task<List<ClienteDto>> ListarClientesAsync()
    {
        await PrepararHeaderAsync();
        return await _http.GetFromJsonAsync<List<ClienteDto>>("api/clientes") ?? [];
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
