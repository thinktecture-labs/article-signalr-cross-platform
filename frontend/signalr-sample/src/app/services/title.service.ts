import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TitleService {

  public title$ = new BehaviorSubject<string>('Chat Sample');

  constructor() {
  }

  public setTitle(title: string) {
    this.title$.next(title);
  }
}
