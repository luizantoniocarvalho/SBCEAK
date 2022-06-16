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
        public virtual int      operacao_id             { get; set; }
        public virtual string   ds_Nome_Operacao        { get; set; }
        public virtual bool     in_Situacao_Registro    { get; set; }
        public virtual int      criou_Registro_id       { get; set; }  
        public virtual DateTime dt_Data_Criacao         { get; set; }
        public virtual int      alterou_Registro_id     { get; set; }
        public virtual DateTime dt_Data_Alteracao       { get; set; }

        public Operacao() { }
        
        public Operacao(int operacao_id)
        {
            this.operacao_id            =   operacao_id;
            this.ds_Nome_Operacao       =   ds_Nome_Operacao;
            this.in_Situacao_Registro   =   in_Situacao_Registro;
            this.criou_Registro_id      =   criou_Registro_id;
            this.dt_Data_Criacao        =   dt_Data_Criacao;
            this.alterou_Registro_id    =   alterou_Registro_id;
            this.dt_Data_Alteracao      =   dt_Data_Alteracao; 
        }   
    }
}