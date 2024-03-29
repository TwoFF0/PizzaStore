import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error) => {
        console.log(error);
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const errors = [];
                for (const index in error.error.errors) {
                  if (error.error.errors[index]) {
                    errors.push(error.error.errors[index]);
                  }
                }

                this.toastr.error(error.statusText, error.status);
                throw errors.flat();
              } else {
                this.toastr.error(error.statusText, error.status);
              }
              break;

            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {
                state: { error: error.error },
              };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something went wrong!');
              break;
          }
        }

        return throwError(error);
      })
    );
  }
}
