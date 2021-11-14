import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Move } from './Move';
import { Router } from '@angular/router';

const GetMoveUrl = 'api/GetMove/';
const CreateNewGameUrl = 'api/CreateNewGame';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private http: HttpClient, private router: Router) {}

  Board: Number[][] = [];
  GameID: String = '';

  GetMoveFromApi(move: Move) {
    var url = GetMoveUrl + this.GameID;
    this.http.post(url, move).subscribe();
  }

  CreateNewGame() {
    var url = CreateNewGameUrl;
    this.http.post(url, null, { responseType: 'text' }).subscribe(
      (text) => (
        (this.GameID = text), this.router.navigate(['/game/' + this.GameID])
      )
      //(error) => console.log(error)
    );
  }

  RedirectToGame() {}

  ResetBoard() {
    for (let y = 0; y <= 100; y++) {
      for (let x = 0; x <= 100; x++) {
        this.Board[y][x] = 0;
      }
    }
  }

  MakeMoveOnBoard(x: string, y: string) {}

  FetchBoard() {}
}
