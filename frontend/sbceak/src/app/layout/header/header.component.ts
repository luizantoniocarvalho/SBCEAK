import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { delay } from 'rxjs/operators';
import { TitleHeaderService } from 'src/app/services/title-header.service';

@Component
({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit
{
  titulo!: string;

  @ViewChild('drawer') drawer!: MatDrawer;

  constructor(private titleHeaderService: TitleHeaderService)  { }

  ngOnInit(): void
  {
    this.titleHeaderService.getTitle()
    .pipe(delay(0))
    .subscribe(t => this.titulo! = t);
  }

  onClick()
  {
    this.drawer.close();
  }
}
