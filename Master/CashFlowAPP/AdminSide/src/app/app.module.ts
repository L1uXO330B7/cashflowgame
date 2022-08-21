import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { SharedMaterialModule } from './common/modules/shared/shared-material.module';
import { UsersTableComponent } from './pages/users-table/users-table.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpInterceptorService } from './common/services/http-interceptor.service';
import { FormsModule } from '@angular/forms';
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
import { QustioneTypePipe, StatusPipe } from './common/pipes/base.pipe';

@NgModule({
  declarations: [
    AppComponent,
    DashboardPageComponent,
    LoginPageComponent,
    UsersTableComponent,
    AnswerQuestionsTableComponent,
    AssetCategorysTableComponent,
    AssetsTableComponent,
    CardEffectsTableComponent,
    CardsTableComponent,
    CashFlowCategorysTableComponent,
    CashFlowsTableComponent,
    EffectTablesTableComponent,
    FunctionsTableComponent,
    LogsTableComponent,
    QuestionsTableComponent,
    QustionEffectsTableComponent,
    RoleFunctionsTableComponent,
    RolesTableComponent,
    UserBoardsTableComponent,
    QustioneTypePipe,
    StatusPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedMaterialModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
