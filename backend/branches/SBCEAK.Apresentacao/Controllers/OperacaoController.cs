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
    public class OperacaoController : ControllerBase
    {
        IConfiguration configuration;

        public OperacaoController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }         

        /// <summary>
        /// Retorna a lista de Operações.
        /// </summary>
        /// <remarks>
        /// Parâmetros:
        ///
        ///  
        /// Ds_Operacao (Tipo: String - Descrição da Operação).
        /// </remarks>

        [HttpPost("PesquisarPorNomeOperacao")]
        public JsonResult PesquisarPorNomeOperacao(OperacaoModel pesquisaModel)
        {
            try
            {
                IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK", Infraestrutura.Database.POSTGRE));              
                OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);

                IList<Operacao> operacaoList = new List<Operacao>();

                operacaoList = operacaoServico.PesquisarPorNomeOperacao(pesquisaModel.ds_Nome_Operacao);

                var operacaoModel = new List<OperacaoModel>();

                foreach (var operacao in operacaoList)
                {
                    operacaoModel.Add(OperacaoModel.EntidadeParaModel(operacao));
                }                
                HttpContext.Response.StatusCode = 200;

                return new JsonResult(operacaoModel);
            }
            catch (DominioException e) { return new JsonResult(new { erro = e.Message }); }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = 500;
                return new JsonResult(new { erro = "Ops! Ocorreu um erro no servidor." });
            }
        }

        /// <summary>
        /// Retorna a lista de Operações.
        /// </summary>
        /// <remarks>
        /// Parâmetros:
        ///
        /// </remarks>

        [HttpGet("PesquisarPorTodasOperacoes")]
        public JsonResult PesquisarPorTodasOperacoes()
        {
            try
            {
                IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK", Infraestrutura.Database.POSTGRE));              
                OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);

                IList<Operacao> operacaoList = new List<Operacao>();

                operacaoList = operacaoServico.PesquisarPorTodasOperacoes();

                var operacaoModel = new List<OperacaoModel>();

                foreach (var operacao in operacaoList)
                {
                    operacaoModel.Add(OperacaoModel.EntidadeParaModel(operacao));
                }                
                HttpContext.Response.StatusCode = 200;

                return new JsonResult(operacaoModel);
            }
            catch (DominioException e) { return new JsonResult(new { erro = e.Message }); }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = 500;
                return new JsonResult(new { erro = "Ops! Ocorreu um erro no servidor." });
            }
        }

        ///<summary>
        ///Realiza a gravação dos dados de Operação.
        ///</summary>
        ///<remarks>
        ///Parâmetros:
        ///
        ///Operacao_id (Tipo: integer - Chave Primária).
        /// 
        ///Ds_Operacao (Tipo: String - Descrição da Operação).
        /// 
        ///In_Situacao_Registro (Tipo: Booleano - Status da Operação).
        ///</remarks>
        [HttpPost("GravarOperacao")]
        public IActionResult GravarOperacao(OperacaoModel operacaoModel)
        {
            var session = NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK");

            IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(session);

            try
            {
                OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);

                Dominio.Entidades.Operacao operacaoEditado = OperacaoModel.ModelParaEntidade(operacaoModel);

                operacaoServico.GravarOperacao(operacaoEditado);

                return Ok(operacaoEditado.operacao_id);
            }
            catch (System.Exception e)
            {
                return ControllerExtension.TratarListaExcecao(e, HttpContext);
            }
        }
    }
}
