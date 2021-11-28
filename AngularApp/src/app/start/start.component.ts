import { Component, OnInit } from '@angular/core';
import { BackendService } from '../backend.service';


@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css'],
})
export class StartComponent implements OnInit {
  constructor(private api: BackendService) {}

  ngOnInit(): void {}

  CreateNewGame() {
    this.api.CreateNewGame();
  }
}
