import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { Operacoes } from '../classes/operacoes.class';

@Injectable({
  providedIn: 'root'
})

export class OperacaoService
{
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type' : 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  PesquisarPorTodasOperacoes()
  {
    return this.http.get<Operacoes[]>(`${environment.urlSBCEAK}/Operacao/PesquisarPorTodasOperacoes`);
  }

  PesquisarPorIdOperacao(id: string): Observable<Operacoes>
  {
    const url = `${environment.urlSBCEAK}/Operacao/PesquisarPorIdOperacao/${id}`;

    return this.http.get<Operacoes>(url);
  }

  public GravarOperacao(operacao: any): Observable<Operacoes>
  {
    return this.http.post<any>(`${environment.urlSBCEAK}/Operacao/GravarOperacao/`, operacao, this.httpOptions)
  }

  public AlterarStatusOperacao(id: string)
  {
    const url = `${environment.urlSBCEAK}/Operacao/AlterarStatusOperacao/${id}`;

    return this.http.post<Operacoes>(url, id, this.httpOptions);
  }

  public AlterarOperacao(operacao: any): Observable<Operacoes>
  {
    return this.http.post<any>(`${environment.urlSBCEAK}/Operacao/AlterarOperacao/`, operacao, this.httpOptions);
  }

}
