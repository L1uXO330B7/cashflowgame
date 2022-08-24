import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.canEnter();
  }

  canEnter():boolean{
    let UserName = localStorage.getItem("StrangerName");
    if(UserName==""){
      return false;
    }
    if(UserName==null){
      return false;
    }
    if(UserName==undefined){
      return false;
    }
    else{
      return true;
    }
  }
}
