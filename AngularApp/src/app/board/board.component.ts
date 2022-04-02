import { Component, OnInit } from '@angular/core';
import { BackendService } from '../backend.service';
import { Move } from '../Move';
import { MoveResponse } from '../MoveResponse';
import { ActivatedRoute } from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
import {Inject} from '@angular/core';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
})
export class BoardComponent implements OnInit {
  constructor(private api: BackendService, private route: ActivatedRoute,public dialog: MatDialog) {}

  Board: Number[][] = [];
  PlayerTurn = true;

  ngOnInit(): void {
    this.api.GetBoard(String(this.route.snapshot.paramMap.get('id'))).then(
      (res) => {
        this.Board = res;
        this.createBoard();
      },
      (error) => console.log('Error Getting the Table From Server:' + error)
    );
  }

  public createBoard() {
    var i = 0;
    for (var y = 0; y < 5; y += 1) {
      for (var x = 0; x < 5; x += 1) {
        var div = document.createElement('div');
        div.className = 'square';
        div.setAttribute('id', i.toString());
        i++;
        div.setAttribute('x', x.toString());
        div.setAttribute('y', y.toString());
        if (this.Board[y][x] === 1) {
          div.style.backgroundColor = 'red';
        } else if (this.Board[y][x] === -1) {
          div.style.backgroundColor = 'green';
        }
        div.onclick = this.onsquareclick.bind(this);
        document.body.getElementsByClassName('board')[0].appendChild(div);
      }
    }
  }

  public async onsquareclick(e: MouseEvent) {
    var el = e.target as HTMLInputElement;
    if (el.style.backgroundColor == "" && this.PlayerTurn == true)
    {
      this.PlayerTurn = false
      el.style.backgroundColor = 'red';
      var MoveX = el.getAttribute('x') ?? '';
      var MoveY = el.getAttribute('y') ?? '';
      var move = { x: MoveX, y: MoveY } as Move;
      var MoveResponse = await this.api.GetMoveFromApi(String(this.route.snapshot.paramMap.get('id')), move);
      console.log(MoveResponse);
      var id = MoveResponse.moveID;
      var gameStateMessage = MoveResponse.gameStateMessage;
      if (id >= 0) document.getElementById(id.toString())!.style.backgroundColor = 'green'
      if (gameStateMessage != "") this.dialog.open(DialogContentExampleDialog, {data: {message: gameStateMessage}})
      this.PlayerTurn = true;
    }

  }


}

@Component({
  selector: 'dialog-content-example-dialog',
  templateUrl: 'dialog-content-example-dialog.html',
})
export class DialogContentExampleDialog {
  constructor(private api: BackendService,  @Inject(MAT_DIALOG_DATA) public data: {message: string}) {}

  async CreateNewGame() {
    this.api.CreateNewGame();
    await this.delay(800);
    window.location.reload();
  }

    delay(ms: number) {
      return new Promise( resolve => setTimeout(resolve, ms) );
  }
}
