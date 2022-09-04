import { ApiService } from 'src/app/service/api.service';
import { Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedService } from 'src/app/service/shared.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ApiRequest } from 'src/app/models/ApiRequest';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.components.scss']
})
export class NavbarComponent implements OnInit {

  constructor(public _SharedService:SharedService,public _Router:Router,private modalService: NgbModal,private _ApiService:ApiService){ }

  ngOnInit(): void {
    this.LoginToUserInfo();
  }
  IsLogin:boolean=false;


  UserId = localStorage.getItem("UserId");
  UserData:any;
  UserDataName:string|any;
  LoginToUserInfo(){

    this._SharedService.SharedData.subscribe((Res)=>{
      this.UserData = Res;
    })
    if (this.UserId!=""&&this.UserId!=null){

      this.IsLogin=true;
    }
  }


  UserLogOut(){
    localStorage.removeItem("UserId");
    localStorage.removeItem("Token");
    this._Router.navigate(['/', 'login']);
  }

ResetInfo:boolean=true;
ResetUserInfo(){
  this.ResetInfo=false;
}
// modal
  @ViewChild('UserModal', { static: true }) modalDOM: any;
  closeResult = '';
  open(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }


  private getDismissReason(reason: any): string {
    this.ResetInfo=true;
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
