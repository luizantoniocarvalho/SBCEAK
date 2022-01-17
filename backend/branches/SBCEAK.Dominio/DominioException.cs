using System;
using System.Collections.Generic;

namespace SBCEAK.Dominio
{
    public class DominioException : Exception
    {
        public DominioException() { }

        public DominioException(string message) : base(message) { }

        public DominioException(List<string> mensagens)
        {
            this.Mensagens = mensagens;
        }

        public List<string> Mensagens { get; set; } = new List<string>();

        public void Estourar()
        {
            if (Mensagens?.Count > 0) throw this;
        }

        public void AdicionarMensagem(string mensagemErro)
        {
            if (Mensagens == null) Mensagens = new List<string>();
            Mensagens.Add(mensagemErro);
        }
    }

    public class SemAcessoException : DominioException { }
}