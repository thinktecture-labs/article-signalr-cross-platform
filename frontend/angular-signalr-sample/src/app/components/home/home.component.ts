import { Component, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  public gameRunning$ = new BehaviorSubject<boolean>(false);
  public gameOver$ = new BehaviorSubject<boolean>(false);
  public winner: string;
  public opponent: string;

  constructor(private readonly signalRService: SignalRService) {
  }

  public async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.gameRunning$ = this.signalRService.gameRunning$;
    this.subscription.add(this.signalRService.gameOver$.subscribe(result => {
      if (result === 'Tie') {
        this.winner = 'Unentschieden';
      } else if (result === 'Lost') {
        this.winner = 'Der Gegner hat die Verbindung verloren!';
      } else {
        this.winner = result === this.signalRService.connectionId ? 'Du hast gewonnen!' : 'Du hast leider verloren.';
      }
      this.gameOver$.next(!!result);
    }));
    this.subscription.add(this.signalRService.activeSession$.subscribe(session => {
      if (session != null) {
        this.opponent = session.userOne.connectionId === this.signalRService.connectionId
          ? session.userTwo.name
          : session.userOne.name;
      } else {
        this.opponent = null;
      }
    }));
  }

  public async joinNewGame() {
    // await this.signalRService.joinNewSession();
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
