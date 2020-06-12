import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  public gameRunning$ = new BehaviorSubject<boolean>(false);
  public gameOver$ = new BehaviorSubject<boolean>(false);
  public winner: string;

  constructor(private readonly signalRService: SignalRService) { }

  public async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.gameRunning$ = this.signalRService.gameRunning$;
    this.subscription.add(this.signalRService.gameOver$.subscribe(result => {
      if (result === 'Tie') {
        this.winner = 'Unentschieden';
      } else if (result === 'Lost') {
        this.winner = 'Der Gegner hat die Verbindung verloren oder aufgegeben!';
      }  else {
        this.winner = result === localStorage.getItem('ownId') ? 'Du hast gewonnen!' : 'Du hast leider verloren.';
      }
      this.gameOver$.next(!!result);
    }));
  }

  public async joinNewGame() {
    await this.signalRService.joinNewSession();
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
