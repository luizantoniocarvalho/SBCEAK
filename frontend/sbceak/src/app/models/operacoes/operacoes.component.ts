import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { LogService } from 'src/app/services/log.service';
import { TitleHeaderService } from 'src/app/services/title-header.service';
import Swal from 'sweetalert2';

import { Operacoes } from '../../classes/operacoes.class';
import { OperacaoInsertModalComponent } from '../dialog-modal/operacao-modal/operacao-insert-modal.component';
import { OperacaoUpdateModalComponent } from '../dialog-modal/operacao-modal/operacao-update-modal.component';
import { OperacaoService } from './../../services/operacao.service';

@Component
({
  selector: 'app-operacoes',
  templateUrl: './operacoes.component.html',
  styleUrls: ['./operacoes.component.css'],
  providers: [DatePipe]
})

export class OperacoesComponent implements OnInit
{
  formFilter!: FormGroup;
  formLog!:    FormGroup;
  dados:       string[] = [''];

  displayedColumns: string[] = ['in_Situacao_Registro', 'ds_Nome_Operacao', 'acao'];

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource = new MatTableDataSource<Operacoes>();

  subscription!: Subscription;

  constructor
  (
    private titleHeaderService: TitleHeaderService,
    private fb: FormBuilder,
    private fbLog: FormBuilder,
    private operacaoService: OperacaoService,
    private logService: LogService,
    public  dialogOperacao:  MatDialog,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void
  {
    this.criarFormFilter();
    this.criarFormLog();

    this.titleHeaderService.setTitle('- Operações');

    this.carregarLista();

    this.dataSource.sort = this.sort;

    this.dataSource.paginator = this.paginator;
  }

  ngAfterViewInit()
  {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  criarFormFilter()
  {
    this.formFilter = this.fb.group
    ({
      operacao_id: [''],
      ds_Nome_Operacao: [''],
      in_Situacao_Registro: [''],
      criou_Registro_id: [''],
      dt_Data_Criacao: [''],
      alterou_Registro_id: [''],
      dt_Data_Alteracao: ['']
    });
  }

  criarFormLog()
  {
    this.formLog = this.fbLog.group
    ({
      dt_Data_Log: [''],
      pessoa_id: [''],
      ds_Log_Realizado: ['']
    });
  }

  carregarLista()
  {
    this.operacaoService.PesquisarPorTodasOperacoes().subscribe((result) =>
    {
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

  AlterarOperacao(id: string)
  {
    sessionStorage.setItem('id', id);

    const dialogRef = this.dialogOperacao.open(OperacaoUpdateModalComponent,
      {
        minWidth: '400px',
        data: this.formFilter.value
      });

      dialogRef.afterClosed().subscribe(result => {
        console.log('The dialog was closed');
      });
  }

  GravarOperacao(): void
  {
    const dialogRef = this.dialogOperacao.open(OperacaoInsertModalComponent,
    {
      minWidth: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  alterarStatusOperacao(id: string)
  {
    //Realizo a alteração do status de operação.
    this.operacaoService.AlterarStatusOperacao(id).subscribe(result => {
      //Popular os dados de log no formulário de log.
      this.formLog.setValue({
        dt_Data_Log: this.datePipe.transform(Date(), 'dd/MM/yyyy HH:mm'),
        pessoa_id: 1,
        ds_Log_Realizado: 'O usuário de ID nº 1 alterou o status da Operação de ID nº ' + id
      });

      //Gravar o registro de Log.
      this.logService.GravarLog(this.formLog.value).subscribe(res => {
        //Dar mensagem de sucesso ao usuário
        this.alertDinamico("Aviso - SBCEAK", "Alteração de Status de Operação realizada com Sucesso!", "sucesso");
      });

    });
  }

  alertDinamico(titulo: string, msg: string, tipo: string)
  {
    if (tipo == "sucesso")
    {
      Swal.fire({
        icon: 'success',
        title: titulo,
        text: msg
      }).then(function()
      {
        window.location.reload();
      });
    }
  }

}
