import { Component, HostBinding, OnInit } from '@angular/core';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'sr-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent implements OnInit {
  public turn = 'X';
  public gameOver = false;
  public cells: string[] = [];
  public winner = null;


  @HostBinding('class.disabled')
  public waitForOther: boolean;

  constructor(private readonly signalRService: SignalrService) {
  }

  ngOnInit() {
    this.init();
    this.signalRService.userPlayed$.subscribe(data => {
        this.otherPlay(data);
    });

    this.signalRService.resetGame$.subscribe(data => {
      this.init();
    });
  }

  async resetGame() {
    await this.signalRService.resetRound();
    this.init();
  }

  init() {
    for (let i = 0; i < 9; i++) {
      this.cells[i] = null;
    }
    this.gameOver = false;
    this.winner = null;
  }

  otherPlay(idx: number) {
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

  async clickHandler(idx: number) {
    if (!this.gameOver && !this.waitForOther) {
      if (this.cells[idx] === null) {
        this.cells[idx] = this.turn;
        await this.signalRService.sendPlayRound(idx);
        this.checkWinner();
        if (!this.gameOver) {
          this.waitForOther = true;
        }
      }
    }
  }

  checkWinner() {
    // winning options
    const lines = [
      [0, 1, 2],
      [3, 4, 5],
      [6, 7, 8],
      [0, 3, 6],
      [1, 4, 7],
      [2, 5, 8],
      [0, 4, 8],
      [2, 4, 6]
    ];
    for (const line of lines) {
      if (this.cells[line[0]] === this.cells[line[1]] && this.cells[line[1]] === this.cells[line[2]] && this.cells[line[0]] !== null) {
        this.gameOver = true;
        const result = this.cells[line[0]];
        this.winner = result === this.turn ? 'You Win' : 'You loose';
        return;
      }
    }

    let occupy = 0;
    this.cells.forEach((e) => { occupy += (e !== null ? 1 : 0) });
    if (occupy === 9) {
      this.gameOver = true;
      this.winner = 'tie';
    }
  }
}
