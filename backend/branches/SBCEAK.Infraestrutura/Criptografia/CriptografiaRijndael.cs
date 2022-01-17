using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SBCEAK.Infraestrutura.Contratos.Criptografia;

namespace SBCEAK.Infraestrutura.Criptografia
{
    public class CriptografiaRijndael : ICriptografia
    {
        private const string cryptoKey = "sbceakcryptoKEY#2018#@vs_1";
        private const string initializerVector = "@ini@VETOR$2018$";

        private static Rijndael CriarInstanciaRijndael(string chave)
        {
            if (!(!string.IsNullOrWhiteSpace(chave)) && (chave.Length == 16 || chave.Length == 24 || chave.Length == 32))
                throw new Exception("A chave precisa ter 16, 24 ou 32 caracteres.");

            if (initializerVector.Length != 16)
                throw new Exception("o vetor de inicialização deve possuir 16 caracteres.");

            Rijndael algoritmo = Rijndael.Create();
            algoritmo.Key = Encoding.ASCII.GetBytes(chave);
            algoritmo.IV = Encoding.ASCII.GetBytes(initializerVector);

            return algoritmo;
        }

        public string Encriptar(string texto, string chave = cryptoKey)
        {
            if (string.IsNullOrWhiteSpace(texto))
                throw new Exception("O texto que será criptografado não pode ser vazio.");

            using (Rijndael algoritmo = CriarInstanciaRijndael(chave))
            {
                ICryptoTransform encryptor = algoritmo.CreateEncryptor(algoritmo.Key, algoritmo.IV);
                using (MemoryStream memoryStreamResultado = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStreamResultado, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(texto);
                        }
                    }

                    return ArrayBytesToHexString(memoryStreamResultado.ToArray());
                }
            }
        }

        public string Decriptar(string textoEncriptado, string chave = cryptoKey)
        {
            if (string.IsNullOrWhiteSpace(textoEncriptado))
                throw new Exception("O texto que será decriptografado não pode ser vazio.");

            if (textoEncriptado.Length % 2 != 0)
                throw new Exception("O conteúdo a ser decriptografado é inválido.");

            string textoDecriptografado = "";

            using (Rijndael algoritmo = CriarInstanciaRijndael(chave))
            {
                ICryptoTransform decryptor = algoritmo.CreateDecryptor(algoritmo.Key, algoritmo.IV);
                using (MemoryStream memoryStream = new MemoryStream(HexStringToArrayBytes(textoEncriptado)))
                {
                    using (CryptoStream cryptoScream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoScream))
                            textoDecriptografado = reader.ReadToEnd();
                    }
                }
            }

            return textoDecriptografado;
        }
        private static string ArrayBytesToHexString(byte[] conteudo)
        {
            string[] arrayHex = Array.ConvertAll(
                conteudo, b => b.ToString("X2"));
            return string.Concat(arrayHex);
        }

        private static byte[] HexStringToArrayBytes(string conteudo)
        {
            int qtdeBytesEncriptados =
                conteudo.Length / 2;
            byte[] arrayConteudoEncriptado =
                new byte[qtdeBytesEncriptados];
            for (int i = 0; i < qtdeBytesEncriptados; i++)
            {
                arrayConteudoEncriptado[i] = Convert.ToByte(
                    conteudo.Substring(i * 2, 2), 16);
            }

            return arrayConteudoEncriptado;
        }

        public string Encriptar(string texto)
        {
            return this.Encriptar(texto, cryptoKey);
        }

        public string Decriptar(string texto)
        {
            return this.Decriptar(texto, cryptoKey);
        }
    }
}