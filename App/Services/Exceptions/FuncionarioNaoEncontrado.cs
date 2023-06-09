namespace App.Services.Exceptions;

public class FuncionarioNaoEncontrado : Exception
{
    public FuncionarioNaoEncontrado(string mensagem)
        : base(mensagem) { }
}
