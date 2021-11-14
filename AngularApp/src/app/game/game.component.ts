import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BackendService } from '../backend.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
})
export class GameComponent implements OnInit {
  constructor(private route: ActivatedRoute, private api: BackendService) {}

  public gameID: String = 'id';

  ngOnInit(): void {
    this.gameID = String(this.route.snapshot.paramMap.get('id'));
  }
}
