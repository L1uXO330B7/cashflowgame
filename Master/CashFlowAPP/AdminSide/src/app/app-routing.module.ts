import { LoginPageComponent } from './pages/login-page/login-page.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { UsersTableComponent } from './pages/users-table/users-table.component';
import { AnswerQuestionsTableComponent } from './pages/answerquestions-table/answerquestion-table.component';
import { AssetCategorysTableComponent } from './pages/assetcategorys-table/assetcategory-table.component';
import { AssetsTableComponent } from './pages/assets-table/asset-table.component';
import { CardEffectsTableComponent } from './pages/cardeffects-table/cardeffect-table.component';
import { CardsTableComponent } from './pages/cards-table/card-table.component';
import { CashFlowCategorysTableComponent } from './pages/cashflowcategorys-table/cashflowcategory-table.component';
import { CashFlowsTableComponent } from './pages/cashflows-table/cashflow-table.component';
import { EffectTablesTableComponent } from './pages/effecttables-table/effecttable-table.component';
import { FunctionsTableComponent } from './pages/functions-table/function-table.component';
import { LogsTableComponent } from './pages/logs-table/log-table.component';
import { QuestionsTableComponent } from './pages/questions-table/question-table.component';
import { QustionEffectsTableComponent } from './pages/qustioneffects-table/qustioneffect-table.component';
import { RoleFunctionsTableComponent } from './pages/rolefunctions-table/rolefunction-table.component';
import { RolesTableComponent } from './pages/roles-table/role-table.component';
import { UserBoardsTableComponent } from './pages/userboards-table/userboard-table.component';
import { AuthGuard } from './common/services/auth.guard';

const routes: Routes = [
  { path: 'dashboard',
    component: DashboardPageComponent,
    canActivate: [AuthGuard],
    children: [
      {path:'users',component:UsersTableComponent},
      {path:'answer-questions',component:AnswerQuestionsTableComponent},
      {path:'asset-categorys',component:AssetCategorysTableComponent},
      {path:'assets',component:AssetsTableComponent},
      {path:'card-effects',component:CardEffectsTableComponent},
      {path:'cards',component:CardsTableComponent},
      {path:'cash-flow-categorys',component:CashFlowCategorysTableComponent},
      {path:'cash-flows',component:CashFlowsTableComponent},
      {path:'effect',component:EffectTablesTableComponent},
      {path:'functions',component:FunctionsTableComponent},
      {path:'logs',component:LogsTableComponent},
      {path:'questions',component:QuestionsTableComponent},
      {path:'qustion-effects',component:QustionEffectsTableComponent},
      {path:'role-functions',component:RoleFunctionsTableComponent},
      {path:'roles',component:RolesTableComponent},
      {path:'user-boards',component:UserBoardsTableComponent},
    ]
  },
  {
    path:'login',
    component:LoginPageComponent
  },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: '/dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
