import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BoardComponent } from './board/board.component';
import { MainComponent } from './main/main.component';
import { PlaceholderComponent } from './placeholder/placeholder.component';

const routes: Routes = [
  { path: 'game', component: BoardComponent },
  { path: 'test', component: MainComponent },
  { path: '', component: PlaceholderComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
