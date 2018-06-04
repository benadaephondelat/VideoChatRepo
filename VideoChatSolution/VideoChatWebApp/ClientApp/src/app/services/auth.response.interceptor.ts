import { Injectable, Injector } from "@angular/core";
import { Router } from "@angular/router";
import {
    HttpClient,
    HttpHandler, HttpEvent, HttpInterceptor,
    HttpRequest, HttpResponse, HttpErrorResponse
} from "@angular/common/http";
import { AuthService } from "./auth.service";
import { Observable } from "rxjs";

@Injectable()
export class AuthResponseInterceptor implements HttpInterceptor {

    currentRequest: HttpRequest<any>;
    auth: AuthService;

    constructor(private injector: Injector, private router: Router) {

    }

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {

        this.auth = this.injector.get(AuthService);
        var token = (this.auth.isLoggedIn()) ? this.auth.getAuth()!.token : null;

        if (token) {
            this.currentRequest = request;

          return next.handle(request).do((event: HttpEvent<any>) => {
            if (event instanceof HttpResponse) {

            }
          }, error => this.handleError(error));
        }
        else {
            return next.handle(request);
        }
    }

    handleError(err: any) {
        if (err instanceof HttpErrorResponse) {
            if (err.status === 401) {
                console.log("Token expired. Attempting refresh...");
                this.auth.refreshToken().subscribe(res => {
                    if (res) {
                      console.log("refresh token successful");
                      this.router.navigate([window.location.pathname]);

                      return true;
                    }
                    else {
                        console.log("refresh token failed");
                        this.auth.logout();
                        this.router.navigate(["login"]);
                    }
                }, error => console.log(error));
            }
      }

      return Observable.throwError('Unauthorized');
    }
}
