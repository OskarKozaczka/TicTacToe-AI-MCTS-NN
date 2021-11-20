import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Move } from './Move';
import { Router } from '@angular/router';

const GetMoveUrl = 'api/GetMove/';
const CreateNewGameUrl = 'api/CreateNewGame';
const GetBoardUrl = 'api/GetBoard/';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private http: HttpClient, private router: Router) {}

  Board: Number[][] = [];
  GameID: String = '';

  GetMoveFromApi(id: string, move: Move): number {
    var url = GetMoveUrl + id;
    var moveid: number = 0;
    this.http.post<number>(url, move).subscribe((data) => (moveid = data));
    return moveid;
  }

  CreateNewGame() {
    var url = CreateNewGameUrl;
    this.http.post(url, null, { responseType: 'text' }).subscribe(
      (text) => this.router.navigate(['/game/' + text]),
      (error) => console.log('Error Creating a Game:' + error)
    );
  }

  MakeMoveOnBoard(x: string, y: string) {}

  async GetBoard(id: string) {
    var url = GetBoardUrl + id;
    return this.http.get<Number[][]>(url).toPromise();
  }
}
