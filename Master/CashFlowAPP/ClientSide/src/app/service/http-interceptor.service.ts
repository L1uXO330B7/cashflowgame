import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {

  constructor(
  ) {

  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    let NewRequest;
    let Token = localStorage.getItem('Token');

    if (Token != 'null') {
      NewRequest = req.clone({
        setHeaders: {
          Authorization: 'Bearer ' + Token,
          AccessControlAllowOrigin: '*',
          AccessControlAllowMethods: 'GET, PUT, POST, DELETE, OPTIONS'
        }
      });
    } else {
      NewRequest = req;
    }

    return next.handle(NewRequest)
    .pipe(
      map((event: HttpEvent<any>) => {

        // https://www.tpisoftware.com/tpu/articleDetails/1084

        console.log('HttpInterceptorService event', event);

        return event;
      }),
      catchError((error: HttpErrorResponse) => {

        console.log('HttpInterceptorService error', error);

        // https://stackoverflow.com/questions/68655492/throwerrorerror-is-now-deprecated-but-there-is-no-new-errorhttperrorresponse
        return throwError(() => error);

      }),
    );
  }
}

