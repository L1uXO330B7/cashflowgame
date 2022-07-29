import { ApiResponse } from './../models/ApiResponse';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  _ApiUrl = `${environment.ApiRoot}`;
  constructor(public _HttpClient: HttpClient) { }
  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  UserLogin(Req:any):Observable<ApiResponse>{
    this._ApiUrl += `/ClientSide/UserLogin`
   return this._HttpClient.post<ApiResponse>(this._ApiUrl, Req, this._HttpOptions);
  }
  GetVerificationCode(){

  }
  UserSignUp(){}
}
