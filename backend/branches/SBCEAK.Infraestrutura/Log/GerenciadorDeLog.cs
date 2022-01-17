using System;
using System.Collections.Generic;
using System.IO;
using SBCEAK.Infraestrutura.Web;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SBCEAK.Infraestrutura.Log
{
    public class GerenciadorDeLog
    {
        public const long TAMANHO_LOG = 60200;

        public static async void Logar(string path, IConfiguration configuration, int idSistema, DateTime dataExpurgo, int idTipoLog, string username = "")
        {
            var log = new
            {
                Sistema = new { Id = idSistema },
                Username = username,
                Data = DateTime.Now,
                Descricao = path,
                dataExpurgo = dataExpurgo,
                TipoLog = new { IdTipoLog = idTipoLog }
            };

            try
            {
                await ServicosHttp.Post(JsonConvert.SerializeObject(log), configuration.GetSection("SalvarLog").Get<string>(), false);
            }
            catch
            {
                //feito desta forma pois nao faz sentido parar a aplicação caso ocorra erro no log e nao tem tratamento caso de erro no log.
            }
        }
        public static void Gravar(string conteudo, string nomeArquivo, bool ambienteDedesenvolvimento)
        {
            string diretorio = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
            string arquivoLog = "";
            if (ambienteDedesenvolvimento)
            {
                arquivoLog = $@"{diretorio}\{nomeArquivo}";
            }
            else
            {
                arquivoLog = $@"{diretorio}\LogServicos\{nomeArquivo}";
            }

            StreamWriter sw = new StreamWriter(arquivoLog, true);
            using (sw)
            {
                if (sw.BaseStream.Length > TAMANHO_LOG)
                {
                    sw.Close();
                    string[] fileLines = File.ReadAllLines(arquivoLog);
                    List<string> newLines = new List<string>();
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (i == 0) continue;
                        newLines.Add(fileLines[i]);
                    }

                    File.Delete(arquivoLog);

                    newLines.Add(conteudo);

                    using (StreamWriter streamWriter = new StreamWriter(arquivoLog, true))
                    {
                        foreach (var linha in newLines)
                        {
                            streamWriter.WriteLine(linha);
                        }
                    }
                }
                else
                {
                    sw.WriteLine(conteudo);
                }
            }
        }

        public static void GravarLogServico(string host, string url, string nomeArquivo, bool ambienteDedesenvolvimento)
        {
            string txtLog = $"O Host {host} fez um request para {url} em " + DateTime.Now;
            Gravar(txtLog, nomeArquivo, ambienteDedesenvolvimento);
        }

    }
}