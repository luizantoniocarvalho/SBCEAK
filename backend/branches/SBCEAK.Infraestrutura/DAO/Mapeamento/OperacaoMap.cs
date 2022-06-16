
using FluentNHibernate.Mapping;
using SBCEAK.Dominio.Entidades;

namespace SBCEAK.Infraestrutura.DAO.Mapeamento
{
    public class OperacaoMap : ClassMap<Operacao>
    {
        public OperacaoMap()
        {
            Table("OPERACOES"); 
            Id(p => p.operacao_id).Column("OPERACAO_ID").GeneratedBy.Sequence("OPERACAO_SEQ");
            Map(p => p.ds_Nome_Operacao).Column("DS_NOME_OPERACAO");     
            Map(p => p.in_Situacao_Registro).Column("IN_SITUACAO_REGISTRO"); 
            Map(p => p.criou_Registro_id).Column("CRIOU_REGISTRO_ID");
            Map(p => p.dt_Data_Criacao).Column("DT_DATA_CRIACAO");
            Map(p => p.alterou_Registro_id).Column("ALTEROU_REGISTRO_ID");
            Map(p => p.dt_Data_Alteracao).Column("DT_DATA_ALTERACAO");
        }
    }
}