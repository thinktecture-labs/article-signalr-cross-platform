import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SignalRService } from './signal-r.service';

@Injectable({
  providedIn: 'root'
})
export class TictactoeService {

  public userPlayed$ = new Subject<number>();

  constructor(private readonly signalRService: SignalRService) {

  }

  private playRound(value: number) {
    this.userPlayed$.next(value);
  }
}
