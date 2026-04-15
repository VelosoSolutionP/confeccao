namespace Roupa.Application.Common;

public class Result<T>
{
    public bool Sucesso { get; private set; }
    public T? Dados { get; private set; }
    public string? Erro { get; private set; }

    private Result() { }

    public static Result<T> Ok(T dados) => new() { Sucesso = true, Dados = dados };
    public static Result<T> Falha(string erro) => new() { Sucesso = false, Erro = erro };
}

public class Result
{
    public bool Sucesso { get; private set; }
    public string? Erro { get; private set; }

    private Result() { }

    public static Result Ok() => new() { Sucesso = true };
    public static Result Falha(string erro) => new() { Sucesso = false, Erro = erro };
}
