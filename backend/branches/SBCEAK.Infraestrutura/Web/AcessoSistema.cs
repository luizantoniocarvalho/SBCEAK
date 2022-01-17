using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SBCEAK.Infraestrutura.Web
{
    public class RespostaControleDeAcessoModel
    {
        public bool acesso { get; set; }
    }

    public class UsuarioLogadoModel
    {
        public string Nome { get; set; }
    }

    public class AcessoSistema
    {
        internal void AcessarSistema(string userName, string sigla, IConfiguration configuration)
        {
            string ControleDeAcessoUrl = configuration.GetSection("ControleDeAcessoUrl").Get<string>() + "/ControleDeAcesso/AcessarSistema";
            string jsonLogin = JsonConvert.SerializeObject(new { UserName = userName, Sigla = sigla });
            var resposta = SBCEAK.Infraestrutura.Web.ServicosHttp.Post(jsonLogin, ControleDeAcessoUrl, true).Result;

            RespostaControleDeAcessoModel respostaModel = JsonConvert.DeserializeObject<RespostaControleDeAcessoModel>(resposta.Content.ReadAsStringAsync().Result);
            if (!respostaModel.acesso) throw new AcessoNegadoException();
        }

        public bool TemAcessoAoSistema(string userName, string sigla, IConfiguration configuration)
        {
            string ControleDeAcessoUrl = configuration.GetSection("ControleDeAcessoUrl").Get<string>() + "/ControleDeAcesso/AcessarSistema";
            string jsonLogin = JsonConvert.SerializeObject(new { UserName = userName, Sigla = sigla });
            var resposta = SBCEAK.Infraestrutura.Web.ServicosHttp.Post(jsonLogin, ControleDeAcessoUrl, true).Result;
            RespostaControleDeAcessoModel respostaModel = JsonConvert.DeserializeObject<RespostaControleDeAcessoModel>(resposta.Content.ReadAsStringAsync().Result);
            return respostaModel.acesso;
        }
    }
}