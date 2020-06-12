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
  public gameOver = false;

  // REVIEW: Hier wird später den items auch null zugewiesen, was prinzipiell ja geht, solange strictNullCheck ausgeschaltet sind.
  // Ggf. strictNullCheck einschalten und die Typings besser gestalten. CHECK
  public cells?: string[] = [];

  // REVIEW: Was für ein Typing hat winner? "Null" ist es ja quasi von sich aus. CHECK
  public winner?: string;


  @HostBinding('class.disabled')
  public waitForOther: boolean;

  constructor(private readonly signalRService: SignalRService) {
  }

  public ngOnInit(): void {
    this.init();

    // REVIEW: Hier wird zwar subscribed, aber nie wieder unsubscribed. CHECK
    this.subscription.add(
      this.signalRService.userPlayed$.subscribe(data => {
        this.otherPlay(data);
      }),
    );

    // REVIEW: Hier wird zwar subscribed, aber nie wieder unsubscribed. CHECK
    // Zudem nicht benutzte Parameter löschen und ggf. ein Body im Lambda, wenn's nicht gebraucht wird. CHECK
    this.subscription.add(
      this.signalRService.resetGame$.subscribe(_ => {
        this.init();
      }),
    );
  }

  public async resetGame(): Promise<void> {
    // REVIEW: Ist das hier von der Logik korrekt? Du willst ja die Runde resetten
    // und dann eigentlich warten bis der Server die bestätigung schickt? (Was er nicht kann, weil er keine Game Logik kennt)
    // Aktuell resettest Du einfach, wenn das Command in SignalR abgesetzt wurde.
    await this.signalRService.resetRound();
    this.init();
  }

  public async onUserClick(idx: number) {
    if (!this.gameOver && !this.waitForOther) {
      if (this.cells[idx] === null) {
        this.cells[idx] = this.turn;
        // REVIEW: Hier ist ja das gleiche Problem, wie mit der resetRound oben.
        await this.signalRService.sendPlayRound(idx);
        this.checkWinner();
        if (!this.gameOver) {
          this.waitForOther = true;
        }
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
    this.winner = null;
  }

  private otherPlay(idx: number): void {
    if (!this.gameOver) {
      if (this.cells[idx] === null) {
        this.cells[idx] = 'O';
        this.checkWinner();
        if (!this.gameOver) {
          this.waitForOther = false;
        }
      }
    }
  }

  private checkWinner() {
    // winning options
    const lines = [
      [0, 1, 2],
      [3, 4, 5],
      [6, 7, 8],
      [0, 3, 6],
      [1, 4, 7],
      [2, 5, 8],
      [0, 4, 8],
      [2, 4, 6],
    ];
    for (const line of lines) {
      if (this.cells[line[0]] === this.cells[line[1]] && this.cells[line[1]] === this.cells[line[2]] && this.cells[line[0]] !== null) {
        this.gameOver = true;
        const result = this.cells[line[0]];
        this.winner = result === this.turn ? 'Gewonnen' : 'Verloren';
        return;
      }
    }

    let occupy = 0;
    this.cells.forEach((e) => {
      occupy += (e !== null ? 1 : 0);
    });
    if (occupy === 9) {
      this.gameOver = true;
      this.winner = 'Unentschieden';
    }
  }
}
