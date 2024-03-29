import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestComponent } from './test/test.component';
import { PlaceholderComponent } from './placeholder/placeholder.component';
import { GameComponent } from './game/game.component';
import { StartComponent } from './start/start.component';

const routes: Routes = [
  { path: 'game/:id', component: GameComponent },
  { path: 'test', component: TestComponent },
  { path: '', component: PlaceholderComponent },
  { path: 'start', component: StartComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
