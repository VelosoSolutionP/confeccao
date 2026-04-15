namespace Roupa.Web.Models;

public class LoginModel
{
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
}

public class RegistrarModel
{
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
    public string ConfirmacaoSenha { get; set; } = "";
}

public class TokenResponse
{
    public string AccessToken { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public DateTime Expiracao { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
}
