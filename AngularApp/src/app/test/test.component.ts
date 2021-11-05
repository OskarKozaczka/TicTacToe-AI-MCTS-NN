import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

const httpOptions = {
  headers: new HttpHeaders({
    responseType: 'text',
  }),
};

const url = 'api/StudioJajo';

@Component({
  selector: 'app-main',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css'],
})
export class TestComponent implements OnInit {
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
