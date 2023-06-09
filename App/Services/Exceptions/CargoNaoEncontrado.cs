namespace App.Services.Exceptions;
public class CargoNaoEncontrado : Exception
{
  public CargoNaoEncontrado(string message) : base(message)
  {
  }
}