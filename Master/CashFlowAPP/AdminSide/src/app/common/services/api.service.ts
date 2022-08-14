import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(public _HttpClient: HttpClient) { }
  _HttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
    _ApiUrl = `${environment.ApiRoot}`;

}
