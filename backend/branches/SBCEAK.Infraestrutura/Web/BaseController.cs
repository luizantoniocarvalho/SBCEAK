using System;
using System.Diagnostics;
using System.Net;
using SBCEAK.Dominio;
using SBCEAK.Infraestrutura.Criptografia;
using SBCEAK.Infraestrutura.Log;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace SBCEAK.Infraestrutura.Web
{
    public class AcessoNegadoException : Exception { }

    public class BaseController : Controller
    {
        public const string COOKIE_USUARIO = "usuario";
        public const string COOKIE_TOKEN = "token";
        public const string COOKIE_NOME_USUARIO = "nomeUsuario";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
        CriptografiaRijndael criptografiaRijndael = new CriptografiaRijndael();

        protected string cookieMatricula = "";

        public BaseController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (HostingEnvironment.IsProduction() || HostingEnvironment.EnvironmentName.Trim().ToUpper() == "HOMOLOG" || HostingEnvironment.IsDevelopment())
                {
                    string matricula = "";
                    try { matricula = context.HttpContext.Request.Form["__matricula"]; } catch (System.Exception) { }

                    if (!string.IsNullOrWhiteSpace(matricula))
                    {
                        Logar(matricula);
                    }
                    else if(HostingEnvironment.IsDevelopment() && string.IsNullOrWhiteSpace(matricula))
                    {
                        string cookieUsuario = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)];
                        string cookieToken = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_TOKEN)];
                        //Execução Local
                        if (cookieUsuario == null & cookieToken == null)
                        {
                            CookieOptions cookieOptions = new CookieOptions();
                            cookieOptions.IsEssential = true;
                            double tempoExpiracao = Convert.ToDouble(60 * 24);
                            cookieOptions.Expires = DateTime.Now.AddMinutes(tempoExpiracao);
                            Response.Cookies.Append(criptografiaRijndael.Encriptar(BaseController.COOKIE_USUARIO), criptografiaRijndael.Encriptar("004000"), cookieOptions);
                        }
                        else
                        {
                            //Está em intra-dev ou já gerou algum cookie
                            if (!AtualizarTokenDeAcesso())
                            {
                                context.Result = Redirect("http://intra-dev");
                                base.OnActionExecuting(context);
                                return;
                            }
                        }
                        base.OnActionExecuting(context);
                        return;

                    }
                    else if (!AtualizarTokenDeAcesso())
                    {
                        if (HostingEnvironment.IsProduction()) context.Result = Redirect("http://intranet");
                        if (HostingEnvironment.EnvironmentName.Trim().ToUpper() == "HOMOLOG") context.Result = Redirect("http://intra-hom");
                        base.OnActionExecuting(context);
                        return;
                    }
                    base.OnActionExecuting(context);
                    return;
                }
                if ( HostingEnvironment.EnvironmentName.Trim().ToUpper() == "LOCAL")
                {
                    CookieOptions cookieOptions = new CookieOptions();
                    cookieOptions.IsEssential = true;
                    double tempoExpiracao = Convert.ToDouble(60 * 24);
                    cookieOptions.Expires = DateTime.Now.AddMinutes(tempoExpiracao);
                    Response.Cookies.Append(criptografiaRijndael.Encriptar(BaseController.COOKIE_USUARIO), criptografiaRijndael.Encriptar("004000"), cookieOptions);
                    base.OnActionExecuting(context);
                    return;
                }

                throw new AcessoNegadoException();
            }
            catch (AcessoNegadoException)
            {
                if (HostingEnvironment.IsProduction()) context.Result = Redirect("http://intranet/semacesso.asp");
                if (HostingEnvironment.EnvironmentName.Trim().ToUpper() == "HOMOLOG") context.Result = Redirect("http://intra-hom/semacesso.asp");
                if (HostingEnvironment.IsDevelopment()) context.Result = Redirect("http://intra-dev/semacesso.asp");

                base.OnActionExecuting(context);
                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void Logar(string matricula)
        {
            string tempoExpiracaoToken = Configuration.GetSection("TempoExpiracaoCookie").Get<string>();

            string matriculaDecriptada = criptografiaRijndael.Decriptar(matricula);

            AcessoSistema acessoSistema = new AcessoSistema();
            acessoSistema.AcessarSistema(matriculaDecriptada, Configuration.GetSection("SiglaSistema").Get<string>(), Configuration);

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.IsEssential = true;
            double tempoExpiracao = Convert.ToDouble(tempoExpiracaoToken);
            cookieOptions.Expires = DateTime.Now.AddMinutes(tempoExpiracao);

            cookieMatricula = criptografiaRijndael.Encriptar(matriculaDecriptada);
            Response.Cookies.Append(criptografiaRijndael.Encriptar(BaseController.COOKIE_USUARIO), cookieMatricula, cookieOptions);
        }

        public bool AtualizarTokenDeAcesso()
        {
            string cookieUsuario = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)];
            string cookieToken = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_TOKEN)];

            string tempoExpiracaoToken = Configuration.GetSection("TempoExpiracaoCookie").Get<string>();
            string usuarioDecriptado = "";
            if (!string.IsNullOrWhiteSpace(cookieUsuario))
            {
                usuarioDecriptado = criptografiaRijndael.Decriptar(cookieUsuario);
            }
            else if (!string.IsNullOrWhiteSpace(cookieMatricula))
            {
                usuarioDecriptado = criptografiaRijndael.Decriptar(cookieMatricula);
            }
            else
            {
                throw new AcessoNegadoException();
            }

            AcessoSistema acessoSistema = new AcessoSistema();
            acessoSistema.AcessarSistema(usuarioDecriptado, Configuration.GetSection("SiglaSistema").Get<string>(), Configuration);

            // grava o token para o usuário informado
            if (!string.IsNullOrWhiteSpace(cookieUsuario) || !string.IsNullOrWhiteSpace(cookieMatricula))
            {
                return GerarTokenDeAcesso(usuarioDecriptado, tempoExpiracaoToken);
            }
            return false;
        }

        protected virtual bool GerarTokenDeAcesso(string usuarioDecriptado, string tempoExpiracaoToken)
        {
            UsuarioSistema usuarioSistema = new UsuarioSistema();
            var usuarioLogado = usuarioSistema.BuscarNomeUsuarioLogado(usuarioDecriptado, Configuration);

            TokenAcesso tokenAcesso = new TokenAcesso();
            string token = tokenAcesso.GerarToken(new CriptografiaRijndael(), usuarioDecriptado, DateTime.Now, tempoExpiracaoToken);

            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.IsEssential = true;
            double tempoExpiracao = Convert.ToDouble(tempoExpiracaoToken);
            cookieOptions.Expires = DateTime.Now.AddMinutes(tempoExpiracao);

            Response.Cookies.Append(criptografiaRijndael.Encriptar(COOKIE_USUARIO), criptografiaRijndael.Encriptar(usuarioDecriptado), cookieOptions);
            Response.Cookies.Append(criptografiaRijndael.Encriptar(COOKIE_TOKEN), token, cookieOptions);
            Response.Cookies.Append(criptografiaRijndael.Encriptar(COOKIE_NOME_USUARIO), criptografiaRijndael.Encriptar(usuarioLogado), cookieOptions);

            return true;
        }

        protected void Logar(string path, IConfiguration configuration, int idSistema, DateTime dataExpurgo, int idTipoLog, string username)
        {
            GerenciadorDeLog.Logar(path, configuration, idSistema, dataExpurgo, idTipoLog, username);
        }

        protected string PegaUsernameLogado()
        {
            string matricula = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)];
            
            if (HostingEnvironment.EnvironmentName.Trim().ToUpper() == "LOCAL" || (matricula == null && HostingEnvironment.IsDevelopment()))
                return "004000";

            var userName = criptografiaRijndael.Decriptar(Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)]);
            return userName;
        }

        protected string PegarNomeUsernameLogado()
        {
            string matricula = Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)];

            if (HostingEnvironment.EnvironmentName.Trim().ToUpper() == "LOCAL" || (matricula == null && HostingEnvironment.IsDevelopment()))
                return "Desenvolvedor";
                
            var userName = criptografiaRijndael.Decriptar(Request.Cookies[criptografiaRijndael.Encriptar(COOKIE_USUARIO)]);
            UsuarioSistema usuarioSistema = new UsuarioSistema();
            return usuarioSistema.BuscarNomeUsuarioLogado(userName, Configuration); ;
        }

        public IActionResult UsuarioLogado()
        {
            return new JsonResult(new { usuario = PegaUsernameLogado() + " - " + PegarNomeUsernameLogado() });
        }

    }

    public class ControllerExtension
    {
        public static IActionResult TratarExcecao(Exception exception, HttpContext httpContext, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {

            if (exception is DominioException || exception is ArgumentException)
            {
                httpContext.Response.StatusCode = (int)statusCode;
                return new JsonResult(new { error = exception.Message });
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { error = "Ocorreu um erro inesperado no servidor." });
            }
        }

        public static IActionResult TratarListaExcecao(Exception exception, HttpContext httpContext, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (exception is DominioException)
            {
                httpContext.Response.StatusCode = (int)statusCode;
                var dominioException = exception as DominioException;
                if (dominioException.Mensagens == null || dominioException.Mensagens.Count == 0)
                {
                    return new JsonResult(new { error = dominioException.Message });
                }
                return new JsonResult(new
                {
                    error = ((DominioException)exception).Mensagens.ToArray(),
                });
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new JsonResult(new { error = "Ocorreu um erro inesperado no servidor." });
            }
        }


        public static string PegarChaveDeConfiguração(IConfiguration configuration, string chave)
        {
            return configuration.GetSection(chave).Get<string>();
        }
    }



    public class UsuarioSistema
    {
        /// <summary>
        ///  Pega a Configuração no appseteing da aplicação que etá chamando o metodo.
        ///  Ex : Para recuperar o nome do usuário no sistema Sitam, deve confiturar o atributo ControleDeacesso no appseting do Sitam
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public string BuscarNomeUsuarioLogado(string userName, IConfiguration configuration)
        {
            string ControleDeAcessoUrl = configuration.GetSection("ControleDeAcessoUrl").Get<string>() + "/Usuario/BuscarNomeUsuarioLogado";
            Console.Write(ControleDeAcessoUrl);
            string jsonLogin = JsonConvert.SerializeObject(new { UserName = userName });
            var resposta = SBCEAK.Infraestrutura.Web.ServicosHttp.Post(jsonLogin, ControleDeAcessoUrl, true).Result;
            UsuarioLogadoModel respostaModel = JsonConvert.DeserializeObject<UsuarioLogadoModel>(resposta.Content.ReadAsStringAsync().Result);

            if (respostaModel.Nome == "")
                throw new AcessoNegadoException();

            return respostaModel.Nome;
        }
    }
}