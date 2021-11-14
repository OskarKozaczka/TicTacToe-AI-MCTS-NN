import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TestComponent } from './test/test.component';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BoardComponent } from './board/board.component';
import { PlaceholderComponent } from './placeholder/placeholder.component';
import { GameComponent } from './game/game.component';
import { StartComponent } from './start/start.component';

@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    BoardComponent,
    PlaceholderComponent,
    GameComponent,
    StartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
