using System.Collections.Generic;
using SBCEAK.Dominio;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace SBCEAK.Dominio.Servicos
{
    public class LogServico
    {
        ILogRepositorio logRepositorio;

        public LogServico(ILogRepositorio logRepositorio)
        {
            this.logRepositorio = logRepositorio;
        }

        public IList<LogRegistro> PesquisarPorTodasLogs()
        {
            using (logRepositorio)
            {
                return logRepositorio.PesquisarPorTodasLogs();
            }
        }

        public int GravarLog(Dominio.Entidades.LogRegistro solicitacao)   
        {
            using(logRepositorio)
            {
                try
                {                    
                    logRepositorio.Salvar(solicitacao);

                    return solicitacao.log_id;                    
                }
                catch (System.Exception e)
                {
                    throw new Exception("Erro ao salvar a tabela de Logs!\n" + e.Message);
                }
            }
        }        
    } 
}