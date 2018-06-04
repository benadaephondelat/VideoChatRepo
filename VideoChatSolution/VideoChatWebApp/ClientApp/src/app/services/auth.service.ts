import { EventEmitter, Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from '@angular/common';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/observable/throw';
import 'rxjs/add/observable/of';

import 'rxjs/Rx';

@Injectable()
export class AuthService {
    authKey: string = "auth";
    clientId: string = "VideoChatWebApp";

    constructor(private http: HttpClient,
        @Inject(PLATFORM_ID) private platformId: any) {
    }

    login(username: string, password: string): Observable<boolean> {
      var url = "api/token/auth";
      var data = {
        username: username,
        password: password,
        client_id: this.clientId,
        grant_type: "password",
        scope: "offline_access profile email"
      };

      return this.getAuthFromServer(url, data);
    }

    refreshToken(): Observable<boolean> {
      var url = "api/token/auth";

      var data = {
        client_id: this.clientId,
        grant_type: "refresh_token",
        refresh_token: this.getAuth()!.refresh_token,
        scope: "offline_access profile email"
      };

      return this.getAuthFromServer(url, data);
  }

  getAuthFromServer(url: string, data: any): Observable<boolean> {
    return this.http.post<TokenResponse>(url, data).map((res) => {
        let token = res && res.token;
        if (token) {
          this.setAuth(res);

          return true;
        }
      }).catch(error => {
        return new Observable<any>(error);
      });
  }

    logout(): boolean {
      this.setAuth(null);
      return true;
    }

    setAuth(auth: TokenResponse | null): boolean {
      if (auth) {
          localStorage.setItem(
              this.authKey,
              JSON.stringify(auth));
      }
      else {
          localStorage.removeItem(this.authKey);
      }
      return true;
    }

    getAuth(): TokenResponse | null {
        var i = localStorage.getItem(this.authKey);
        if (i) {
            return JSON.parse(i);
        }
        else {
            return null;
        }
    }

    isLoggedIn(): boolean {
        return localStorage.getItem(this.authKey) != null;
    }
} 
