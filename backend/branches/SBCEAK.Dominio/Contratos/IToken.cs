using System;
using SBCEAK.Infraestrutura.Contratos.Criptografia;

namespace SBCEAK.Dominio.Contratos.Entidades
{
    public interface ITokenDeAcesso
    {
        /// <summary>
        ///  Gera um token de acesso
        /// </summary>
        /// <param name="criptografia">Criptografia que gera o token</param>
        /// <param name="usuario">usuário que fará o acesso</param>
        /// <param name="dataCriacao">data de crianção do token</param>
        /// <param name="tempoExpiracao">tempo que o token será válido</param>
        /// <returns></returns>
        string GerarToken(ICriptografia criptografia, string usuario, DateTime dataCriacao, string tempoExpiracao);
        /// <summary>
        /// Verifia se o token é válido
        /// </summary>
        /// <param name="criptografia">Criptografia que será utilizada</param>
        /// <param name="token">Token (texto)</param>
        /// <returns></returns>
        bool TokenValido(ICriptografia criptografia, string token);
        /// <summary>
        /// Pega o tempo de Expiração do Token
        /// </summary>
        /// <param name="criptografia">Criptografia que será utilizada</param>
        /// <param name="token">Token (texto)</param>
        /// <returns></returns>
        string GetTempoExpiracao(ICriptografia criptografia, string token);
        /// <summary>
        /// pega o usuáiro usado para gerar o token
        /// </summary>
        /// <param name="criptografia">Criptografia que será utilizada</param>
        /// <param name="token">Token (texto)</param>
        /// <returns></returns>
        string GetUsuario(ICriptografia criptografia, string token);

    }
}