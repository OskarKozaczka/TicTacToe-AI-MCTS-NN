import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    responseType: 'text',
  }),
};

const url = 'api/Api/StudioJajo';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
})
export class MainComponent implements OnInit {
  constructor(private http: HttpClient) {}

  text: any;
  ngOnInit(): void {
    this.text = 'Studio Jajo';
  }

  public apicall() {
    this.http
      .get(url, { responseType: 'text' })
      .subscribe((text) => (this.text = text));
  }
}
