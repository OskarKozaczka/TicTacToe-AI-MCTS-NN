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


  text?: String;
  ngOnInit(): void {
    this.text= "Studio Jajo";
  }
 

  public apicall()
  {
    this.http.get("api/Api/StudioJajo", { responseType: 'text' }).subscribe(text => this.text=text);
  }

}
