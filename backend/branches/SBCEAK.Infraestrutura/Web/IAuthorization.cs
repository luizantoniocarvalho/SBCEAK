using Microsoft.AspNetCore.Http;

namespace SBCEAK.Infraestrutura.Web
{
    public interface IAuthorization
    {
        bool Auntenticar(HttpRequest request, string usuario, string senha);
    }
}