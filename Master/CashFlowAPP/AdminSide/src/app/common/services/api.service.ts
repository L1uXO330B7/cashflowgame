import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../models/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    public _HttpClient: HttpClient
  ) {

  }

  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  _ApiUrl = `${environment.ApiRoot}`;

  UserLogin(Req: any): Observable<ApiResponse> {
    let Url = `${this._ApiUrl}/ClientSide/UserLogin`;
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }

  AddCards(): Observable<ApiResponse> {
    let Url = `${this._ApiUrl}/Adjustment/SupportCardDev`;
    return this._HttpClient.post<ApiResponse>(Url, this._HttpOptions);
  }








  UsersRead(Req: any) {
    let Url = `${this._ApiUrl}/Users/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UsersDelete(Req: any) {
    let Url = `${this._ApiUrl}/Users/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UsersCreate(Req: any) {
    let Url = `${this._ApiUrl}/Users/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UsersUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Users/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }



  AnswerQuestionsRead(Req: any) {
    let Url = `${this._ApiUrl}/AnswerQuestions/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AnswerQuestionsDelete(Req: any) {
    let Url = `${this._ApiUrl}/AnswerQuestions/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AnswerQuestionsCreate(Req: any) {
    let Url = `${this._ApiUrl}/AnswerQuestions/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AnswerQuestionsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/AnswerQuestions/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  AssetsRead(Req: any) {
    let Url = `${this._ApiUrl}/Assets/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetsDelete(Req: any) {
    let Url = `${this._ApiUrl}/Assets/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetsCreate(Req: any) {
    let Url = `${this._ApiUrl}/Assets/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Assets/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  AssetCategorysRead(Req: any) {
    let Url = `${this._ApiUrl}/AssetCategorys/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetCategorysDelete(Req: any) {
    let Url = `${this._ApiUrl}/AssetCategorys/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetCategorysCreate(Req: any) {
    let Url = `${this._ApiUrl}/AssetCategorys/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  AssetCategorysUpdate(Req: any) {
    let Url = `${this._ApiUrl}/AssetCategorys/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  CardsRead(Req: any) {
    let Url = `${this._ApiUrl}/Cards/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardsDelete(Req: any) {
    let Url = `${this._ApiUrl}/Cards/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardsCreate(Req: any) {
    let Url = `${this._ApiUrl}/Cards/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Cards/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  CardEffectsRead(Req: any) {
    let Url = `${this._ApiUrl}/CardEffects/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardEffectsDelete(Req: any) {
    let Url = `${this._ApiUrl}/CardEffects/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardEffectsCreate(Req: any) {
    let Url = `${this._ApiUrl}/CardEffects/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CardEffectsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/CardEffects/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  CashFlowsRead(Req: any) {
    let Url = `${this._ApiUrl}/CashFlows/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowsDelete(Req: any) {
    let Url = `${this._ApiUrl}/CashFlows/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowsCreate(Req: any) {
    let Url = `${this._ApiUrl}/CashFlows/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/CashFlows/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  CashFlowCategorysRead(Req: any) {
    let Url = `${this._ApiUrl}/CashFlowCategorys/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowCategorysDelete(Req: any) {
    let Url = `${this._ApiUrl}/CashFlowCategorys/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowCategorysCreate(Req: any) {
    let Url = `${this._ApiUrl}/CashFlowCategorys/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  CashFlowCategorysUpdate(Req: any) {
    let Url = `${this._ApiUrl}/CashFlowCategorys/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  EffectTablesRead(Req: any) {
    let Url = `${this._ApiUrl}/EffectTables/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  EffectTablesDelete(Req: any) {
    let Url = `${this._ApiUrl}/EffectTables/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  EffectTablesCreate(Req: any) {
    let Url = `${this._ApiUrl}/EffectTables/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  EffectTablesUpdate(Req: any) {
    let Url = `${this._ApiUrl}/EffectTables/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  FunctionsRead(Req: any) {
    let Url = `${this._ApiUrl}/Functions/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  FunctionsDelete(Req: any) {
    let Url = `${this._ApiUrl}/Functions/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  FunctionsCreate(Req: any) {
    let Url = `${this._ApiUrl}/Functions/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  FunctionsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Functions/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  LogsRead(Req: any) {
    let Url = `${this._ApiUrl}/Logs/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  LogsDelete(Req: any) {
    let Url = `${this._ApiUrl}/Logs/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  LogsCreate(Req: any) {
    let Url = `${this._ApiUrl}/Logs/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  LogsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Logs/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  QuestionsRead(Req: any) {
    let Url = `${this._ApiUrl}/Questions/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QuestionsDelete(Req: any) {
    let Url = `${this._ApiUrl}/Questions/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QuestionsCreate(Req: any) {
    let Url = `${this._ApiUrl}/Questions/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QuestionsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Questions/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  QustionEffectsRead(Req: any) {
    let Url = `${this._ApiUrl}/QustionEffects/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QustionEffectsDelete(Req: any) {
    let Url = `${this._ApiUrl}/QustionEffects/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QustionEffectsCreate(Req: any) {
    let Url = `${this._ApiUrl}/QustionEffects/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  QustionEffectsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/QustionEffects/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  RolesRead(Req: any) {
    let Url = `${this._ApiUrl}/Roles/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RolesDelete(Req: any) {
    let Url = `${this._ApiUrl}/Roles/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RolesCreate(Req: any) {
    let Url = `${this._ApiUrl}/Roles/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RolesUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Roles/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }




  RoleFunctionsRead(Req: any) {
    let Url = `${this._ApiUrl}/RoleFunctions/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RoleFunctionsDelete(Req: any) {
    let Url = `${this._ApiUrl}/RoleFunctions/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RoleFunctionsCreate(Req: any) {
    let Url = `${this._ApiUrl}/RoleFunctions/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  RoleFunctionsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/RoleFunctions/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }





  UserBoardsRead(Req: any) {
    let Url = `${this._ApiUrl}/UserBoards/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserBoardsDelete(Req: any) {
    let Url = `${this._ApiUrl}/UserBoards/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserBoardsCreate(Req: any) {
    let Url = `${this._ApiUrl}/UserBoards/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserBoardsUpdate(Req: any) {
    let Url = `${this._ApiUrl}/UserBoards/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }



}
