import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../models/ApiResponse';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(public _HttpClient: HttpClient) { }
  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  _ApiUrl = `${environment.ApiRoot}`;

  UsersRead(Req: any) {
    let Url = `${this._ApiUrl}/Users/Read`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserDelete(Req: any) {
    let Url = `${this._ApiUrl}/Users/Delete`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserCreate(Req: any) {
    let Url = `${this._ApiUrl}/Users/Create`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
  UserUpdate(Req: any) {
    let Url = `${this._ApiUrl}/Users/Update`
    return this._HttpClient.post<ApiResponse>(Url, Req, this._HttpOptions);
  }
}
