import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

import { Logs } from '../classes/logs.class';

@Injectable({
  providedIn: 'root'
})

export class LogService
{
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type' : 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  // PesquisarPorTodasOperacoes()
  // {
  //   return this.http.get<Operacoes[]>(`${environment.urlSBCEAK}/Operacao/PesquisarPorTodasOperacoes`);
  // }

  // PesquisarPorIdOperacao(id: string): Observable<Operacoes>
  // {
  //   const url = `${environment.urlSBCEAK}/Operacao/PesquisarPorIdOperacao/${id}`;

  //   return this.http.get<Operacoes>(url);
  // }

  public GravarLog(log: any): Observable<Logs>
  {
    return this.http.post<any>(`${environment.urlSBCEAK}/Log/GravarLog/`, log, this.httpOptions);
  }
}
