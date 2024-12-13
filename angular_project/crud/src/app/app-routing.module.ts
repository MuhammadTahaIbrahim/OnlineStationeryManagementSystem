import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UpdataComponent } from './updata/updata.component';

const routes: Routes = [
  {
  component:HomeComponent,
  path:"home"
},
{
  component:AboutComponent,
  path:"about"
},
{
  component: UpdataComponent,
  path:"update/:id"
},
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
