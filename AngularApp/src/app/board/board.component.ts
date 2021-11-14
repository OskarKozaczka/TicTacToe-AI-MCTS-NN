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
    for (var y = 0; y < 10; y += 1) {
      for (var x = 0; x < 10; x += 1) {
        var div = document.createElement('div');
        div.className = 'square';
        div.setAttribute('x', x.toString());
        div.setAttribute('y', y.toString());
        div.onclick = this.onsquareclick.bind(this);
        document.body.getElementsByClassName('board')[0].appendChild(div);
      }
    }
  }

  public onsquareclick(e: MouseEvent) {
    var el = e.target as HTMLInputElement;
    el.style.backgroundColor = 'red';
    var MoveX = el.getAttribute('x') ?? '';
    var MoveY = el.getAttribute('y') ?? '';
    var move = { x: MoveX, y: MoveY } as Move;
    // console.log(el.getAttribute('x'));
    this.api.MakeMoveOnBoard(MoveX, MoveY);
    this.api.GetMoveFromApi(move);
  }
}
