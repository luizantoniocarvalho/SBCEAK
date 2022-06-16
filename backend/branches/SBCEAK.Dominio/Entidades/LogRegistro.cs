using System;
using System.Collections.Generic;
using SBCEAK.Dominio;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
using System.Linq;

namespace SBCEAK.Dominio.Entidades
{

    public class LogRegistro : EntidadeBase
    { 
        public virtual int          log_id              { get; set; }
        public virtual DateTime     dt_Data_Log         { get; set; }
        public virtual int          pessoa_id           { get; set; }  
        public virtual string       ds_Log_Realizado    { get; set; }

        public LogRegistro() { }
        
        public LogRegistro(int log_id)
        {
            this.log_id             =   log_id;
            this.dt_Data_Log        =   dt_Data_Log;
            this.pessoa_id          =   pessoa_id;
            this.ds_Log_Realizado   =   ds_Log_Realizado;
        }   
    }
}