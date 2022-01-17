using System;
using System.Collections.Generic;
using SBCEAK.Dominio;
using SBCEAK.Dominio.Entidades;
using SBCEAK.Dominio.Repositorios;
//using SBCEAK.Util;
//using SBCEAK.Dominio.Enuns;
using System.Linq;

namespace SBCEAK.Dominio.Entidades
{

    public class Operacao : EntidadeBase
    { 
        public virtual int      Operacao_id             { get; set; }
        public virtual string   ds_Nome_Operacao        { get; set; }
        public virtual string   in_Situacao_Registro    { get; set; }  

        public Operacao() { }
        
        public Operacao(int operacao_id)
        {
            this.Operacao_id            =   operacao_id;
            this.ds_Nome_Operacao       =   ds_Nome_Operacao;
            this.in_Situacao_Registro   =   in_Situacao_Registro; 
        }   
    }
}