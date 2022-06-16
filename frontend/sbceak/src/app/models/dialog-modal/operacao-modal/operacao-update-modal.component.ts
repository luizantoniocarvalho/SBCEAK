import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { LogService } from 'src/app/services/log.service';
import { OperacaoService } from 'src/app/services/operacao.service';
import Swal from 'sweetalert2';

@Component
({
  selector: 'app-operacao-update-modal',
  templateUrl: './operacao-update-modal.component.html',
  styleUrls: ['./operacao-update-modal.component.css'],
  providers: [DatePipe]
})

export class OperacaoUpdateModalComponent implements OnInit
{
  operacaoForm!: FormGroup;
  formLog!:      FormGroup;
  data: any;
  public codigo       = sessionStorage.getItem('id');

  constructor
  (
    private fb: FormBuilder,
    private fbLog: FormBuilder,
    private operacaoService: OperacaoService,
    private logService: LogService,
    public  dialogRef: MatDialogRef<OperacaoUpdateModalComponent>,
    private datePipe: DatePipe
  ) { }

  ngOnInit(): void
  {


    this.data = this.operacaoService.PesquisarPorIdOperacao(this.codigo!)
    .subscribe(result => {
      this.data = result

      this.operacaoForm.setValue(
        {
          operacao_id: this.data[0].operacao_id,
          ds_Nome_Operacao: this.data[0].ds_Nome_Operacao,
          in_Situacao_Registro: this.data[0].in_Situacao_Registro,
          criou_Registro_id: this.data[0].criou_Registro_id,
          dt_Data_Criacao: this.data[0].dt_Data_Criacao,
          alterou_Registro_id: 1,
          dt_Data_Alteracao: this.datePipe.transform(Date(), 'dd/MM/yyyy HH:mm')
        });
    });

    this.criarOperacaoForm();

    this.criarFormLog();
  }

  criarOperacaoForm()
  {
    this.operacaoForm = this.fb.group
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

  alterarOperacao(): void
  {
    //Realizo a alteração da operação.
    this.operacaoService.AlterarOperacao(this.operacaoForm.value).subscribe(result => {
      if (result)
      {
        //Popular os dados de log no formulário de log.
        this.formLog.setValue({
          dt_Data_Log: this.datePipe.transform(Date(), 'dd/MM/yyyy HH:mm'),
          pessoa_id: 1,
          ds_Log_Realizado: 'O usuário de ID nº 1 alterou a Operação de ID nº ' + result
        });

        //Gravar o registro de Log.
        this.logService.GravarLog(this.formLog.value).subscribe(res => {
          //Dar mensagem de sucesso ao usuário
          this.alertDinamico("Aviso - SBCEAK", "Operação Alterada com Sucesso!", "sucesso");
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
