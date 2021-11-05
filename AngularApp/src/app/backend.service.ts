import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const urlbase = 'api/GetMove/'

@Injectable({
  providedIn: 'root'
})

export class BackendService {

  constructor(private http: HttpClient) { }

  GetMoveFromApi(id:String)
  {
    var url = urlbase + String(this.route.snapshot.paramMap.get('id'));;

    this.http.post(url, { responseType: 'text'});
  }



}
