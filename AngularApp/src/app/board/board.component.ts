import { Component, OnInit } from '@angular/core';
import { BackendService } from '../backend.service';
import { Move } from '../Move';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css'],
})
export class BoardComponent implements OnInit {
  constructor(private api: BackendService) {}

  ngOnInit(): void {
    for (var i = 0; i < 100; i += 1) {
      var div = document.createElement('div');
      div.className = 'square';
      div.onclick = this.onsquareclick.bind(this);
      document.body.getElementsByClassName('board')[0].appendChild(div);
    }
  }

  public onsquareclick(e: MouseEvent) {
    var el = e.target as HTMLInputElement;
    el.style.backgroundColor = 'red';
    var move = { x: 2, y: 3 } as Move;
    this.api.GetMoveFromApi('twojstary', move);
  }
}
