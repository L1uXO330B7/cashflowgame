import { Router } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { SharedService } from 'src/app/service/shared.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.components.scss']
})
export class NavbarComponent implements OnInit {

  constructor(public _SharedService:SharedService,public _Router:Router,private modalService: NgbModal,){ }

  ngOnInit(): void {
    this.LoginToUserInfo();
  }
  IsLogin:boolean=false;
  UserData:any;
  LoginToUserInfo(){
    let UserId = localStorage.getItem('UserId');
    console.log(UserId);
    this._SharedService.SharedData.subscribe((Res)=>{
      this.UserData = Res;
      console.log(this.UserData,"a");
    })
    if (UserId!=""&&UserId!=null){

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
