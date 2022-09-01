import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth.guard';
import { GamePageComponent } from './pages/game-page/game-page.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginComponent } from './pages/login-page/login.component';
import { SurveyPageComponent } from './pages/survey-page/survey-page.component';

const routes: Routes = [{
  path: "home",
  component: HomePageComponent,
},
{
  path: "",
  redirectTo: "/home",
  pathMatch: "full", // 當路徑是空的時候轉址到 home
},
{
  path: "login",
  component: LoginComponent,
},
{
  path: "game",
  component: GamePageComponent,
},
{
  path: "survey",
  component: SurveyPageComponent,
  canActivate:[AuthGuard],
},
{
  path: "**",
  component: HomePageComponent, // 萬用路徑，路由沒有比對到，永遠會執行
},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
