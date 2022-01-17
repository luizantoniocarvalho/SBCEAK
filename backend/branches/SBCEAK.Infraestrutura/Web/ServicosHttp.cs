using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SBCEAK.Infraestrutura.Web
{

    public class ServicosHttp
    {

        /// <summary>
        /// chama um serviço http
        /// </summary>
        /// <param name="json">informe o argumento no formato JSON</param>
        /// <param name="url">link do serviço que será chamado</param>
        /// <param name="esperar">indica se a thread tem de esperar ou não o retorno do serviço</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(string url, bool esperar = true)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri(url);

                if (esperar)
                {
                    var taskResult = httpClient.GetAsync(url);
                    taskResult.Wait();
                    return taskResult.Result;
                }
                else
                {
                    var result = await httpClient.GetAsync(url);
                    return result;
                }
            }
        }

        public static async Task<HttpResponseMessage> Post(string json, string url, bool esperar = true)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri(url);

                // content da requisição post
                StringContent content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

                if (esperar)
                {
                    var taskResult = httpClient.PostAsync(url, content);
                    taskResult.Wait();
                    return taskResult.Result;
                }
                else
                {
                    var result = await httpClient.PostAsync(url, content);
                    return result;
                }
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string json, string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri(url);

                // resposta do chamada
                HttpResponseMessage result = null;

                // content da requisição post
                StringContent content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                result = await httpClient.PostAsync(url, content);
                return result;

            }
        }

        public static async Task<HttpResponseMessage> Post(string json, string url, IAuthorization authorization, HttpRequest request, string usuario, string senha, bool esperar = true)
        {
            authorization.Auntenticar(request, usuario, senha);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new System.Uri(url);


                // content da requisição post
                StringContent content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

                if (esperar)
                {
                    var taskResult = httpClient.PostAsync(url, content);
                    taskResult.Wait();
                    return taskResult.Result;
                }
                else
                {
                    var result = await httpClient.PostAsync(url, content);
                    return result;
                }
            }
        }
    }
}