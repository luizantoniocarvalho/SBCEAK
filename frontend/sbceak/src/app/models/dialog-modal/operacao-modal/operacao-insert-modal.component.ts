import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { LogService } from 'src/app/services/log.service';
import Swal from 'sweetalert2';

import { OperacaoService } from 'src/app/services/operacao.service';

@Component
({
  selector: 'app-operacao-insert-modal',
  templateUrl: './operacao-insert-modal.component.html',
  styleUrls: ['./operacao-insert-modal.component.css'],
  providers: [DatePipe]
})

export class OperacaoInsertModalComponent implements OnInit
{
  operacaoForm!: FormGroup;
  formLog!:      FormGroup;

  constructor
  (
    private fb: FormBuilder,
    private fbLog: FormBuilder,
    private operacaoServico: OperacaoService,
    private logService: LogService,
    public dialogRef: MatDialogRef<OperacaoInsertModalComponent>,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void
  {
    this.criarOperacaoForm();

    this.criarFormLog();
  }

  criarOperacaoForm()
  {
    this.operacaoForm = this.fb.group({
      ds_Nome_Operacao: ['', [Validators.required]],
      in_Situacao_Registro: [true],
      criou_Registro_id: [1],
      dt_Data_Criacao: [this.datePipe.transform(Date(), 'dd/MM/yyyy HH:mm')]
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

  createOperacao()
  {
    //Realizo a inclusão da operação.
    this.operacaoServico.GravarOperacao(this.operacaoForm.value).subscribe(result => {
      if (result)
      {
        //Popular os dados de log no formulário de log.
        this.formLog.setValue({
          dt_Data_Log: this.datePipe.transform(Date(), 'dd/MM/yyyy HH:mm'),
          pessoa_id: 1,
          ds_Log_Realizado: 'O usuário de ID nº 1 incluiu a Operação de ID nº ' + result
        });

        //Gravar o registro de Log.
        this.logService.GravarLog(this.formLog.value).subscribe(res => {
          //Dar mensagem de sucesso ao usuário
          this.alertDinamico("Aviso - SBCEAK", "Operação Cadastrada com Sucesso!", "sucesso");
        });

        this.operacaoForm.reset();

        this.dialogRef.close();
      }
    });
  }

  cancel(): void
  {
    this.dialogRef.close();

    this.operacaoForm.reset();
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
