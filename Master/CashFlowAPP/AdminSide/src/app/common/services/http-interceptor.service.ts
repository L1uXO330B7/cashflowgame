import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, Observable, throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {

  NewRequest: any;

  constructor(
    private route: Router
  ) {

  }
  ShowToast(Msg: string, CssClass: string, Header: string) {

  }
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let Token = localStorage.getItem('Token');
    if (Token != 'null') {
      this.NewRequest = req.clone({
        setHeaders: {
          Authorization: 'Bearer ' + Token,
          AccessControlAllowOrigin: '*',
          AccessControlAllowMethods: 'GET, PUT, POST, DELETE, OPTIONS'
        }
      });
    } else {
      this.NewRequest = req;
    }

    return next.handle(this.NewRequest)
      .pipe(
        map((event: any) => {
          // https://www.tpisoftware.com/tpu/articleDetails/1084
          if (event.body !== undefined || null) {
            if (event.body.Success) {
              if (this.NewRequest.url.indexOf('Read') == -1) {
                alert(event.body.Message + '\n成功通知 From 錢董')
              }
            }
            else {
              alert(event.body.Message + '\n失敗通知 From 錢董')
            }
          }
          return event;
        }),
        catchError((error: HttpErrorResponse) => {
          this.ShowToast('伺服器維修中，請稍後再試', 'bg-warning  text-dark', '失敗通知 From 錢董')
          // https://stackoverflow.com/questions/68655492/throwerrorerror-is-now-deprecated-but-there-is-no-new-errorhttperrorresponse
          return throwError(() => error);
        }),
      );
  }
}

