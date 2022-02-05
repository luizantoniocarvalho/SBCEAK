import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortable } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { TitleHeaderService } from 'src/app/services/title-header.service';

import { Operacoes } from '../operacoes.class';
import { OperacaoService } from './../../services/operacao.service';

@Component({
  selector: 'operacoes.component',
  templateUrl: './operacoes.component.html',
  styleUrls: ['./operacoes.component.css']
})

export class OperacoesComponent implements OnInit
{
  formFilter!: FormGroup;

  displayedColumns: string[] = ['in_Situacao_Registro', 'ds_Nome_Operacao', 'acao'];

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource = new MatTableDataSource<Operacoes>();

  subscription!: Subscription;

  constructor (
    private titleHeaderService: TitleHeaderService,
    private fb: FormBuilder,
    private operacaoService: OperacaoService
    )
  { }

  ngOnInit(): void
  {
    this.criarFormFilter();
    this.titleHeaderService.setTitle('- Operações');
    this.carregarLista();
    //this.sort.sort(({id: 'Nome', start: 'asc'}) as MatSortable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  criarFormFilter() {
    this.formFilter = this.fb.group({
      operacao_id: '',
      ds_Nome_Operacao: '',
      in_Situacao_Registro: ''
    });
  }

  carregarLista()
  {
    this.operacaoService.PesquisarPorTodasOperacoes().subscribe((result) =>
    {
      //console.log(result);
      this.dataSource.data = result;
    });
  }

  applyFilter(event: Event)
  {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator)
    {
      this.dataSource.paginator.firstPage();
    }
  }
}
