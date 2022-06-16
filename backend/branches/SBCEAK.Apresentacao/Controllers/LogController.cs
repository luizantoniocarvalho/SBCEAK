using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SBCEAK.Infraestrutura.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using SBCEAK.Apresentacao.Models;
using SBCEAK.Dominio.Servicos;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using SBCEAK.Infraestrutura.DAO;
using SBCEAK.Infraestrutura.DAO.Mapeamento;
using SBCEAK.Dominio;
using SBCEAK.Infraestrutura.Nhibernate;
using SBCEAK.Infraestrutura;

namespace SBCEAK.Apresentacao.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        IConfiguration configuration;

        public LogController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }         

        /// <summary>
        /// Retorna a lista de Operações.
        /// </summary>
        /// <remarks>
        /// Parâmetros:
        ///
        /// </remarks>

        //[HttpGet("PesquisarPorTodasOperacoes")]
        // public JsonResult PesquisarPorTodasOperacoes()
        // {
        //     try
        //     {
        //         IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK", Infraestrutura.Database.POSTGRE));              
        //         OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);

        //         IList<Operacao> operacaoList = new List<Operacao>();

        //         operacaoList = operacaoServico.PesquisarPorTodasOperacoes();

        //         var operacaoModel = new List<OperacaoModel>();

        //         foreach (var operacao in operacaoList)
        //         {
        //             operacaoModel.Add(OperacaoModel.EntidadeParaModel(operacao));
        //         }                
        //         HttpContext.Response.StatusCode = 200;

        //         return new JsonResult(operacaoModel);
        //     }
        //     catch (DominioException e) { return new JsonResult(new { erro = e.Message }); }
        //     catch (Exception)
        //     {
        //         HttpContext.Response.StatusCode = 500;
        //         return new JsonResult(new { erro = "Ops! Ocorreu um erro no servidor." });
        //     }
        // }

        ///<summary>
        ///Realiza a gravação dos dados de Log de Registro.
        ///</summary>
        ///<remarks>
        ///Parâmetros:
        ///
        ///Log_id (Tipo: integer - Chave Primária).
        /// 
        ///Dt_Data_Log (Tipo: date - Data e Hora do registro do Log).
        /// 
        /// Pessoa_id (Tipo: integer - Chavre estrangeira referente a tabela Pessoas)
        /// 
        /// Ds_Log_Realizado (Tipo: string - Registra as alterações no registro)
        ///</remarks>
        [HttpPost("GravarLog")]
        public IActionResult GravarLog(LogModel logModel)
        {
            var session = NHibernateSession.OpenSession<LogMap>(configuration, "SBCEAK");

            ILogRepositorio logRepositorio = new LogDAO(session);

            try
            {
                LogServico logServico = new LogServico(logRepositorio);

                Dominio.Entidades.LogRegistro logEditado = LogModel.ModelParaEntidade(logModel);

                logServico.GravarLog(logEditado);

                return Ok(logEditado.log_id);
            }
            catch (System.Exception e)
            {
                return ControllerExtension.TratarListaExcecao(e, HttpContext);
            }
        }
    }
}
