using SBCEAK.Dominio.Entidades;
using System;

namespace SBCEAK.Apresentacao.Models
{
    public class OperacaoModel
    {
        public int      operacao_id             { get; set; }
        public string   ds_Nome_Operacao        { get; set; }         
        public bool     in_Situacao_Registro    { get; set; }
        public int      criou_Registro_id       { get; set; }        
        public DateTime dt_Data_Criacao         { get; set; }
        public int      alterou_Registro_id     { get; set; }
        public DateTime dt_Data_Alteracao       { get; set; }

        public static OperacaoModel EntidadeParaModel(Operacao operacao)
        {
            return new OperacaoModel() 
            { 
                operacao_id             =   operacao.operacao_id, 
                ds_Nome_Operacao        =   operacao.ds_Nome_Operacao.ToUpper(),
                in_Situacao_Registro    =   operacao.in_Situacao_Registro,
                criou_Registro_id       =   operacao.criou_Registro_id,
                dt_Data_Criacao         =   operacao.dt_Data_Criacao,
                alterou_Registro_id     =   operacao.alterou_Registro_id,
                dt_Data_Alteracao       =   operacao.dt_Data_Alteracao
            };
        }

        public static Dominio.Entidades.Operacao ModelParaEntidade(OperacaoModel solicitacaoModel)  
        {
            Operacao solicitacao = new Operacao();

            solicitacao.operacao_id             =   solicitacaoModel.operacao_id;
            solicitacao.ds_Nome_Operacao        =   solicitacaoModel.ds_Nome_Operacao.ToUpper();
            solicitacao.in_Situacao_Registro    =   solicitacaoModel.in_Situacao_Registro;
            solicitacao.criou_Registro_id       =   solicitacaoModel.criou_Registro_id;
            solicitacao.dt_Data_Criacao         =   solicitacaoModel.dt_Data_Criacao;
            solicitacao.alterou_Registro_id     =   solicitacaoModel.alterou_Registro_id;
            solicitacao.dt_Data_Alteracao       =   solicitacaoModel.dt_Data_Alteracao;
            
            return solicitacao;
        }
    }
}

