import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { OperacaoService } from '../app/services/operacao.service';
import { TitleHeaderService } from '../app/services/title-header.service';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UppercaseInputDirectiveDirective } from './directive/uppercase-input-directive.directive';
import { HeaderComponent } from './layout/header/header.component';
import { OperacaoInsertModalComponent } from './models/dialog-modal/operacao-modal/operacao-insert-modal.component';
import { OperacaoUpdateModalComponent } from './models/dialog-modal/operacao-modal/operacao-update-modal.component';
import { OperacoesComponent } from './models/operacoes/operacoes.component';


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    OperacoesComponent,
    OperacaoInsertModalComponent,
    OperacaoUpdateModalComponent,
    UppercaseInputDirectiveDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatSidenavModule,
    MatDividerModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    FlexLayoutModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    MatSortModule,
    MatSlideToggleModule,
    MatTooltipModule,
    MatDialogModule,
    MatNativeDateModule
  ],
  providers: [OperacaoService, TitleHeaderService],
  bootstrap: [AppComponent]
})
export class AppModule { }
