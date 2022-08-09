import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {

  constructor() { }

  NewUserName:string="新使用者";

  SetRegisterName(Name:string){
    this.NewUserName = Name;
  }
  GetRegisterName(){
    return this.NewUserName;
  }

}
