import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Operacoes } from '../models/operacoes.class';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class OperacaoService
{
  constructor(private http: HttpClient) { }

  PesquisarPorTodasOperacoes()
  {
    //console.log(filtros);

    return this.http.get<Operacoes[]>(`${environment.urlSBCEAK}/Operacao/PesquisarPorTodasOperacoes`);
  }
}
