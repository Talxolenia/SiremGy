import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { debug } from 'util';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  intercept(
    req: import('@angular/common/http').HttpRequest<any>,
    next: import('@angular/common/http').HttpHandler): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        debugger;
        if (error.status == 401) {

          return throwError(error.error);
        }
        if (error instanceof HttpErrorResponse) {
          const applicationError = error.headers.get('Application-Error');
          if (applicationError) {
            return throwError(applicationError);
          }
        }

        const serverError = error.error;
        let modelStateErrors = '';
        if (serverError.errors && Object.keys(serverError.errors).length > 0) {
          for (const key in serverError.errors) {
            if (serverError.errors[key]) {
              modelStateErrors += '\n' + serverError.errors[key];
            }
          }
        }

        return throwError(modelStateErrors || serverError || 'Server Error');
      })
    );
  }

}

export const ErroInterceptorProviders = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true
};
