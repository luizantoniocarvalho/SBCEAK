using SBCEAK.Dominio.Entidades;
using System;

namespace SBCEAK.Apresentacao.Models
{
    public class LogModel
    {
        public int          log_id              { get; set; }
        public DateTime     dt_Data_Log         { get; set; }         
        public int          pessoa_id           { get; set; }        
        public string       ds_Log_Realizado    { get; set; }

        public static LogModel EntidadeParaModel(LogRegistro log)
        {
            return new LogModel() 
            { 
                log_id              =   log.log_id, 
                dt_Data_Log         =   log.dt_Data_Log,
                pessoa_id           =   log.pessoa_id,
                ds_Log_Realizado    =   log.ds_Log_Realizado.ToUpper(),
            };
        }

        public static Dominio.Entidades.LogRegistro ModelParaEntidade(LogModel solicitacaoModel)  
        {
            LogRegistro solicitacao = new LogRegistro();

            solicitacao.log_id              =   solicitacaoModel.log_id;
            solicitacao.dt_Data_Log         =   solicitacaoModel.dt_Data_Log;
            solicitacao.pessoa_id           =   solicitacaoModel.pessoa_id;
            solicitacao.ds_Log_Realizado    =   solicitacaoModel.ds_Log_Realizado.ToUpper();
            
            return solicitacao;
        }
    }
}
