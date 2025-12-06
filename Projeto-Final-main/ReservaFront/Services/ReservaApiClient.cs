using System.Net.Http.Json;
using ReservaFront.Models;

namespace ReservaFront.Services
{
    public class ReservaApiClient
    {
        private readonly HttpClient _http;

        public ReservaApiClient(HttpClient http)
        {
            _http = http;
        }

        // ---------- CLIENTES ----------

        // Lista todos os clientes
        public async Task<List<Cliente>> GetClientesAsync()
        {
            return await _http.GetFromJsonAsync<List<Cliente>>("api/Clientes") 
                   ?? new List<Cliente>();
        }

        // Busca um cliente pelo ID
        public async Task<Cliente?> GetClienteAsync(int id)
        {
            return await _http.GetFromJsonAsync<Cliente>($"api/Clientes/{id}");
        }

        // Cria um novo cliente
        public async Task<bool> CriarCliente(Cliente cliente)
        {
            var resp = await _http.PostAsJsonAsync("api/Clientes", cliente);
            return resp.IsSuccessStatusCode;
        }

        // Atualiza cliente existente
        public async Task<bool> UpdateClienteAsync(int id, Cliente cliente)
        {
            var resp = await _http.PutAsJsonAsync($"api/Clientes/{id}", cliente);
            return resp.IsSuccessStatusCode;
        }

        // Exclui cliente
        public async Task<bool> DeleteClienteAsync(int id)
        {
            var resp = await _http.DeleteAsync($"api/Clientes/{id}");
            return resp.IsSuccessStatusCode;
        }

        //----------Login----------
        public class LoginResponse
{
    public string? Mensagem { get; set; }
    public int UsuarioId { get; set; }
    public string? Nome { get; set; }
}

public async Task<LoginResponse?> LoginAsync(string nome, string senha)
{
    var resp = await _http.PostAsJsonAsync("api/Auth/login", new { Nome = nome, Senha = senha });

    if (!resp.IsSuccessStatusCode)
        return null;

    return await resp.Content.ReadFromJsonAsync<LoginResponse>();
}

        //----------MESAS----------
        //Lista todas as mesas

public async Task<List<Mesa>> GetMesasAsync()
{
    return await _http.GetFromJsonAsync<List<Mesa>>("api/Mesas")
           ?? new List<Mesa>();
}


    }
}
