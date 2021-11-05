import { Component, OnInit } from '@angular/core';
import { BackendService } from '../backend.service';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.css']
})


export class BoardComponent implements OnInit {

  constructor(private api: BackendService) { }

  ngOnInit(): void {
    for(var i = 0; i < 100; i += 1) {
      var div = document.createElement("div");
      div.className = "square";
      div.onclick = this.onsquareclick;
      document.body.getElementsByClassName("board")[0].appendChild(div);
    }
  }

  public onsquareclick(e:MouseEvent)
  {
      var el = e.target as HTMLInputElement;
      el.style.backgroundColor = "red";
      this.api.GetMoveFromApi("twój stary");
  }
}
