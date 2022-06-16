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

        public IList<Operacao> PesquisarPorTodasOperacoes()
        {
            using (operacaoRepositorio)
            {
                return operacaoRepositorio.PesquisarPorTodasOperacoes();
            }
        }

        public IList<Operacao> PesquisarPorNomeOperacao(string nome)
        {
            using (operacaoRepositorio)
            {
                if (string.IsNullOrWhiteSpace(nome)) throw new DominioException("Parâmetros obrigatórios não informados.");

                return operacaoRepositorio.PesquisarPorNomeOperacao(nome);
            }
        } 

        public IList<Operacao> PesquisarPorIdOperacao(int id)
        {
            using(operacaoRepositorio)
            {
                if (id == 0) throw new DominioException("Parâmetros obrigatórios não informados.");

                return operacaoRepositorio.PesquisarPorIdOperacao(id);
            }
        }
        
        public int GravarOperacao(Dominio.Entidades.Operacao solicitacao)   
        {
            using(operacaoRepositorio)
            {
                try
                {
                    if (solicitacao.operacao_id != 0)
                    {
                        //Alteração.
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.operacao_id;
                    }
                    else
                    {
                        //Inclusão
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.operacao_id;
                    }
                }
                catch (System.Exception e)
                {
                    throw new Exception("Erro ao salvar a tabela de Operação!\n" + e.Message);
                }
            }
        }
        
        public int AlterarOperacao(Dominio.Entidades.Operacao solicitacao)
        {
            using(operacaoRepositorio)
            {
                try
                {
                    if (solicitacao.operacao_id != 0)
                    {
                        //Alteração.
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.operacao_id;
                    }
                    else
                    {
                        return solicitacao.operacao_id;
                    }                    
                }
                catch (System.Exception e)
                {
                    throw new Exception("Erro ao alterar a tabela de Operação!\n" + e.Message);
                }
            }
        }

        // public IList<Operacao> AlterarStatusOperacao(IList<Operacao> solicitacao)
        public int AlterarStatusOperacao(Dominio.Entidades.Operacao solicitacao)
        {
            //Dominio.Entidades.Operacao
            using(operacaoRepositorio)
            {
                try
                {
                    if (solicitacao.operacao_id != 0)
                    {
                        //Alteração.
                        operacaoRepositorio.Salvar(solicitacao);

                        return solicitacao.operacao_id;
                    }
                    else
                    {
                        return solicitacao.operacao_id;
                    }                    
                }
                catch (System.Exception e)
                {
                    throw new Exception("Erro ao alterar o status de Operação!\n" + e.Message);
                }
            }
        }

    } 
}