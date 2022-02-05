
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
        }
    }
}