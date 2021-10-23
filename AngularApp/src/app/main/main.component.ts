import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  constructor(private http: HttpClient,) { 
    
  }

  ngOnInit(): void {
  }


  public apicall()
  {
      return this.http.get<string>("https://localhost:44340/api/Api/StudioJajo").subscribe(value => value);
  }

}
