using System;
using SBCEAK.Dominio.Contratos.Entidades;
using SBCEAK.Infraestrutura.Contratos.Criptografia;

namespace SBCEAK.Infraestrutura.Web
{
    public class TokenAcesso : ITokenDeAcesso
    {
        private int INDICE_TEMPO_EXPIRACAO = 2;
        private int INDICE_DATA_CRIACAO = 1;
        private int INDICE_USUARIO = 0;

        public string GerarToken(ICriptografia criptografia, string texto, DateTime dataCriacao, string tempoExpiracao)
        {
            try
            {
                string textoParaCriptografar = $"{texto}|{dataCriacao}|{tempoExpiracao}";
                string token = criptografia.Encriptar(textoParaCriptografar);
                return token;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public string GetTempoExpiracao(ICriptografia criptografia, string token)
        {
            string tokenDecriptado = criptografia.Decriptar(token);
            string[] splitedToken = tokenDecriptado.Split("|");
            return splitedToken[INDICE_TEMPO_EXPIRACAO];
        }

        public string GetUsuario(ICriptografia criptografia, string token)
        {
            string tokenDecriptado = criptografia.Decriptar(token);
            string[] splitedToken = tokenDecriptado.Split("|");
            return splitedToken[INDICE_USUARIO];
        }

        public bool TokenValido(ICriptografia criptografia, string token)
        {
            string tokenDecriptado = criptografia.Decriptar(token);
            string[] splitedToken = tokenDecriptado.Split("|");

            DateTime expiracao = Convert.ToDateTime(splitedToken[INDICE_DATA_CRIACAO]);
            string minutosExpiracao = splitedToken[INDICE_TEMPO_EXPIRACAO];

            double diferencaMinutos = expiracao.Subtract(DateTime.Now).TotalMinutes;
            return diferencaMinutos > 0;
        }
    }
}