using SBCEAK.Dominio.Entidades;

namespace SBCEAK.Apresentacao.Models
{
    public class OperacaoModel
    {
        public int      Operacao_id             { get; set; }
        public string   ds_Nome_Operacao        { get; set; }         
        public bool     in_Situacao_Registro    { get; set; }        

        public static OperacaoModel EntidadeParaModel(Operacao operacao)
        {
            return new OperacaoModel() 
            { 
                Operacao_id             =   operacao.Operacao_id, 
                ds_Nome_Operacao        =   operacao.ds_Nome_Operacao.ToUpper(),
                in_Situacao_Registro    =   operacao.in_Situacao_Registro
            };
        }

        public static Dominio.Entidades.Operacao ModelParaEntidade(OperacaoModel solicitacaoModel)  
        {
            Operacao solicitacao = new Operacao();

            solicitacao.Operacao_id             =   solicitacaoModel.Operacao_id;
            solicitacao.ds_Nome_Operacao        =   solicitacaoModel.ds_Nome_Operacao.ToUpper();
            solicitacao.in_Situacao_Registro    =   solicitacaoModel.in_Situacao_Registro;
            
            return solicitacao;
        }
    }
}

