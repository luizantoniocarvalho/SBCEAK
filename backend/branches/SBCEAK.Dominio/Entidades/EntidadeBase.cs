using System;
using System.Collections.Generic;
using System.Text;

namespace SBCEAK.Dominio.Entidades
{
    public abstract class EntidadeBase
    {
        private int id;
        public virtual int Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is EntidadeBase)) return false;
            return Id == (obj as EntidadeBase).Id;
        }
        protected virtual void ValidarNaoNulo(string campoParaValidacao, string campoParaExibicao)
        {
            if (campoParaValidacao == null)
                throw new DominioException("O campo " + campoParaExibicao + " não pode ser nulo");
        }

        protected virtual void ValidarTamanhoMaximo(string campoParaValidacao, int tamanhoMaximo, string campoParaExibicao)
        {
            ValidarNaoNulo(campoParaValidacao, campoParaExibicao);
            if (campoParaValidacao.Length > tamanhoMaximo)
                throw new DominioException("O campo " + campoParaExibicao + " não pode ter mais do que " + tamanhoMaximo + " caracteres");
        }

        protected virtual void ValidarTamanhoMaximoNaoObrigatorio(string campoParaValidacao, int tamanhoMaximo, string campoParaExibicao)
        {
            if (string.IsNullOrWhiteSpace(campoParaValidacao)) return;
            if (campoParaValidacao.Length > tamanhoMaximo)
                throw new DominioException("O campo " + campoParaExibicao + " não pode ter mais do que " + tamanhoMaximo + " caracteres");
        }


        protected virtual void ValidarRange(int valorParaValidacao, int tamanhoMaximo, int tamanhoMinimo, string campoParaExibicao)
        {
            if (valorParaValidacao > tamanhoMaximo || valorParaValidacao < tamanhoMinimo)
                throw new DominioException("O campo " + campoParaExibicao + " deve ser entre " + tamanhoMinimo + " e " + tamanhoMaximo);
        }

        protected virtual void ValidarRange(decimal valorParaValidacao, decimal tamanhoMaximo, decimal tamanhoMinimo, string campoParaExibicao)
        {
            if (valorParaValidacao > tamanhoMaximo || valorParaValidacao < tamanhoMinimo)
                throw new DominioException("O campo " + campoParaExibicao + " deve ser entre " + tamanhoMinimo + " e " + tamanhoMaximo);
        }
    }
}
