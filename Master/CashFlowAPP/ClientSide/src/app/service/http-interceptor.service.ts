import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpInterceptorService implements HttpInterceptor {
  constructor(
  ) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let newRequest;
    let token = localStorage.getItem('token');

    
    if (token != 'null') {
      newRequest = req.clone({
        setHeaders: {
          Authorization: 'Bearer ' + token,
          AccessControlAllowOrigin: '*',
          AccessControlAllowMethods: 'GET, PUT, POST, DELETE, OPTIONS'
        }
      });
    } else {
      newRequest = req;
    }
    return next.handle(newRequest).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {


          if (event.body.StatusCode > 0) {
            console.log(event);
          }
        }
        return event;
      })
      ,
      catchError((error: HttpErrorResponse) => {
        console.log(error);

        return throwError(error);
      }),
    );
  }
  }

