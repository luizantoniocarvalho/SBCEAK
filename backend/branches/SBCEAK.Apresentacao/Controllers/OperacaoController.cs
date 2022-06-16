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
        /// Retorna a operação com o ID passado.
        /// </summary>
        /// <remarks>
        /// Parâmetros:
        ///
        ///  
        /// operacao_id (Tipo: Integer - ID da Operação).
        /// </remarks>

        [HttpGet("PesquisarPorIdOperacao/{id}")]
        public JsonResult PesquisarPorIdOperacao(int id)
        {
            try
            {
                IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK", Infraestrutura.Database.POSTGRE));              
                OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);

                IList<Operacao> operacaoList = new List<Operacao>();

                operacaoList = operacaoServico.PesquisarPorIdOperacao(id);

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

        /// <summary>
        ///Realiza a alteração do status de Operação.
        /// </summary>
        /// <remarks>
        /// Parâmetros:
        /// 
        /// 
        ///Operacao_id (Tipo: integer - Chave Primária).
        /// </remarks>
        
        [HttpPost("AlterarStatusOperacao/{id}")]
        public IActionResult AlterarStatusOperacao(int id)
        {
            //Crio a variável session que recebe a ligação com o banco de dados.
            var session = NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK");

            //Crio a variável operacaoRepositoropo que recebe o DAO com a sessão já criada.
            IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(session);

            //Crio a variável operacaoServico com as operações do serviço com o repositório já criado.
            OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);
            
            //Crio a variável operacaoList que vai receber a pesquisa da operação cadastrada dentro de uma lista.
            IList<Operacao> operacaoList = new List<Operacao>();
            
            try
            {
                //Realizo o método de pesquisa por id.
                operacaoList = operacaoServico.PesquisarPorIdOperacao(id);

                //Verifico o resultado do campo in-situacao_registro, e troco para o contrário do que existe.
                if (operacaoList[0].in_Situacao_Registro == true)
                {
                    operacaoList[0].in_Situacao_Registro = false;
                }
                else
                {
                    operacaoList[0].in_Situacao_Registro  = true;
                }    
                //Alterei tenho que registra quem alterou e a data.
                operacaoList[0].criou_Registro_id = operacaoList[0].criou_Registro_id;
                operacaoList[0].dt_Data_Criacao = operacaoList[0].dt_Data_Criacao;
                operacaoList[0].alterou_Registro_id = 1;
                operacaoList[0].dt_Data_Alteracao = DateTime.Now;

                //Crio a variável operacaoModel do tipo model vazia.
                OperacaoModel operacaoModel = new OperacaoModel();

                //Carrego a variável operacaoModel com as informações que estão 
                //na variável operacaoList.
                operacaoModel.operacao_id           = operacaoList[0].operacao_id;
                operacaoModel.ds_Nome_Operacao      = operacaoList[0].ds_Nome_Operacao;
                operacaoModel.in_Situacao_Registro  = operacaoList[0].in_Situacao_Registro;
                operacaoModel.criou_Registro_id     = operacaoList[0].criou_Registro_id;
                operacaoModel.dt_Data_Criacao       = operacaoList[0].dt_Data_Criacao;
                operacaoModel.alterou_Registro_id   = operacaoList[0].alterou_Registro_id;
                operacaoModel.dt_Data_Alteracao     = operacaoList[0].dt_Data_Alteracao;

                //Converte a Model para Entidade.
                Dominio.Entidades.Operacao operacaoEditado = OperacaoModel.ModelParaEntidade(operacaoModel);

                //Como já foi realizado um método, ao final dele a sessão é fechada, por isso
                //tenho que abrir uma nova sessão, um novo repositório e um novo serviço para
                //executar o novo método.
                session = NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK");
                operacaoRepositorio = new OperacaoDAO(session);
                operacaoServico = new OperacaoServico(operacaoRepositorio);
                
                //Realizo o método de gravar que no caso como já tem id será um update.
                operacaoServico.GravarOperacao(operacaoEditado);    

                //Retorno a entidade com os dados do registro.
                return new JsonResult(operacaoEditado);

            }
            catch(System.Exception e)
            {
                return ControllerExtension.TratarListaExcecao(e, HttpContext);
            }
        }

        ///<summary>
        ///Realiza a alteração dos dados de Operação.
        ///</summary>
        ///<remarks>
        ///Parâmetros:
        ///
        ///Operacao_id (Tipo: integer - Chave Primária).
        /// 
        ///In_Situacao_Registro (Tipo: Booleano - Status da Operação).
        ///</remarks>
        [HttpPost("AlterarOperacao")]
        public IActionResult AlterarOperacao(OperacaoModel operacaoModel)
        {
            var session = NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK");

            IOperacaoRepositorio operacaoRepositorio = new OperacaoDAO(session);

            OperacaoServico operacaoServico = new OperacaoServico(operacaoRepositorio);
            try
            {
                //Busco os dados do id que quero alterar.
                IList<Operacao> operacaoList = new List<Operacao>();
                operacaoList = operacaoServico.PesquisarPorIdOperacao(operacaoModel.operacao_id);

                //Verifico se é uma mudança de status ou de descrição.
                if (operacaoList[0].in_Situacao_Registro != operacaoModel.in_Situacao_Registro)
                {
                    //Alteração de Status.
                    operacaoModel.ds_Nome_Operacao = operacaoList[0].ds_Nome_Operacao;
                }

                Dominio.Entidades.Operacao operacaoEditado = OperacaoModel.ModelParaEntidade(operacaoModel);

                session = NHibernateSession.OpenSession<OperacaoMap>(configuration, "SBCEAK");
                operacaoRepositorio = new OperacaoDAO(session);
                operacaoServico = new OperacaoServico(operacaoRepositorio);

                operacaoServico.AlterarOperacao(operacaoEditado);

                return Ok(operacaoEditado.operacao_id);
            }
            catch (System.Exception e)
            {
                return ControllerExtension.TratarListaExcecao(e, HttpContext);
            }
        }
    }
}
