import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Move } from './Move';

const urlbase = 'api/GetMove/';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor(private http: HttpClient) {}

  GetMoveFromApi(id: String, move: Move) {
    var url = urlbase + id;
    console.log(url);
    this.http.post(url, move).subscribe();
  }
}
