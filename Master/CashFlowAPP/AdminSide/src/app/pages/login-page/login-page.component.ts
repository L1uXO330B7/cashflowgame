import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiRequest } from 'src/app/common/models/ApiRequest';
import { ApiService } from 'src/app/common/services/api.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {

  constructor(
    public _ApiService: ApiService,
    private _Router: Router,
  ) {

  }

  ngOnInit(): void {
    this.RandomColor = this.getRandomColor();
  }

  RandomColor:string = '';
  // 隨機取 rgb 字串
  getRandomColor() {
    var rgb = 'rgba(' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ',' + Math.floor(Math.random() * 255) + ',0.3)';
    return rgb;
  }

  _ClientUserLogin: ClientUserLogin = new ClientUserLogin();
  UserLogin() {
    let Req = new ApiRequest<ClientUserLogin>();
    Req.Args = this._ClientUserLogin;
    this._ApiService.UserLogin(Req).subscribe((Res) => {
      console.log(Res);
      if (Res.Success) {
        localStorage.setItem('Token', Res.Data);
        this._Router.navigateByUrl('/dashboard');
      }
    });
  }
}

export class ClientUserLogin {
  Email: string = "";
  Password: string = "";
}
