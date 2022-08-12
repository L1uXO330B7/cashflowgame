import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { UsersTableComponent } from './pages/users-table/users-table.component';

const routes: Routes = [
  { path: 'dashboard',
    component: DashboardPageComponent,
    children: [
      {
        path:'users',
        component:UsersTableComponent
      }
    ] },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
