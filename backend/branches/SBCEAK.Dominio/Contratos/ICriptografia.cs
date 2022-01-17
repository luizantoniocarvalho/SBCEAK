namespace SBCEAK.Infraestrutura.Contratos.Criptografia
{

    public interface IHash
    {
        string Computar(string texto);
    }

    public interface ICriptografia
    {
        string Encriptar(string texto, string chave);
        string Encriptar(string texto);
        string Decriptar(string texto, string chave);
        string Decriptar(string texto);
    }
}