using System.Collections.Generic;
using SBCEAK.Dominio;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace SBCEAK.Dominio.Servicos
{
    public class OperacaoServico
    {
        IOperacaoRepositorio operacaoRepositorio;

        public OperacaoServico(IOperacaoRepositorio operacaoRepositorio)
        {
            this.operacaoRepositorio = operacaoRepositorio;
        }

        public IList<Operacao> PesquisarPorNomeOperacao(string nome)
        {
            using (operacaoRepositorio)
            {
                if (string.IsNullOrWhiteSpace(nome)) throw new DominioException("Parâmetros obrigatórios não informados.");

                return operacaoRepositorio.PesquisarPorNomeOperacao(nome);
            }
        } 

        public int GravarOperacao(Dominio.Entidades.Operacao solicitacao)   
        {
            using(operacaoRepositorio)
            {
                try
                {
                    if (solicitacao.Operacao_id != 0)
                    {
                        //Alteração.
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.Operacao_id;
                    }
                    else
                    {
                        //Inclusão
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.Operacao_id;
                    }
                }
                catch (System.Exception e)
                {
                    throw new Exception("Erro ao salvar a solicitação de transferência!\n" + e.Message);
                }
            }
        }
    } 
}