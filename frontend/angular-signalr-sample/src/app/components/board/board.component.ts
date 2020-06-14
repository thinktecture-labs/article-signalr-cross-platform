import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'sr-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss'],
})
export class BoardComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  public turn = 'X';
  public opponent: string;
  public gameOver = false;
  public cells?: string[] = [];


  @HostBinding('class.disabled')
  public waitForOther: boolean;

  constructor(private readonly signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.init();

    this.subscription.add(
      this.signalRService.userPlayed$.subscribe(data => {
        this.otherPlay(data);
      }),
    );

    this.subscription.add(
      this.signalRService.activeSession$.subscribe(session => {
        if (session !== null) {
          this.waitForOther = session.activeUser !== localStorage.getItem('ownId');
        }
      }),
    );
  }

  public async onUserClick(idx: number) {
    if (!this.gameOver && !this.waitForOther) {
      if (this.cells[idx] === null) {
        this.cells[idx] = this.turn;
        await this.signalRService.sendPlayRound(idx);
        this.waitForOther = true;
      }
    }
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private init(): void {
    for (let i = 0; i < 9; i++) {
      this.cells[i] = null;
    }
    this.gameOver = false;
  }

  private otherPlay(idx: number): void {
    if (!this.gameOver) {
      if (this.cells[idx] === null) {
        this.cells[idx] = 'O';
        this.waitForOther = false;
      }
    }
  }
}
