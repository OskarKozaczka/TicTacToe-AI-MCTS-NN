import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
})
export class GameComponent implements OnInit {
  constructor(private route: ActivatedRoute) {}

  gameID: String = 'id';

  ngOnInit(): void {
    this.gameID = String(this.route.snapshot.paramMap.get('id'));
  }
}
