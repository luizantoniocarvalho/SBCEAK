
using FluentNHibernate.Mapping;
using SBCEAK.Dominio.Entidades;

namespace SBCEAK.Infraestrutura.DAO.Mapeamento
{
    public class LogMap : ClassMap<LogRegistro>
    {
        public LogMap()
        {
            Table("LOGS"); 
            Id(p => p.log_id).Column("LOG_ID").GeneratedBy.Sequence("LOG_SEQ");
            Map(p => p.dt_Data_Log).Column("DT_DATA_LOG");     
            Map(p => p.pessoa_id).Column("PESSOA_ID");
            Map(p => p.ds_Log_Realizado).Column("DS_LOG_REALIZADO");
        }
    }
}