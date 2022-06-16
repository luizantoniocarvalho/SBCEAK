//Modelo de operações
export interface Operacoes
{
  operacao_id: string;
  ds_Nome_Operacao: string;
  in_Situacao_Registro: string;
}

//Modelo só com id
export interface OperacoesId
{
  operacao_id: string;
}

//Modelo para get uma operação
export interface ResponseOperacoes
{
  data: Operacoes;
}

//Modelo para update de operação
export interface RequestUpdateOperacoes
{
  ds_Nome_Operacao: string;
  in_Situacao_Registro: string;
}

export interface ResponseUpdateOperacoes
{
  ds_Nome_Operacao: string;
  in_Situacao_Registro: string;
}
