import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class TitleHeaderService
{
  title: Subject<string> = new Subject();

  constructor() { }

  setTitle(title: string)
  {
    this.title.next(title);
  }

  getTitle()
  {
    return this.title.asObservable();
  }
}
