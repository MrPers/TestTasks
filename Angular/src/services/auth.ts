import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import * as Oidc from 'oidc-client';
import { HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root' 
})
export class AuthService {

  private user: User;
  private manager = new UserManager({
      userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }), //to store the session localStore
      authority: "https://localhost:10001",
      client_id: "client_angular_id",
      response_type: "code",
      scope: "openid profile OrdersAPI",
      redirect_uri: "http://localhost:4200/callback",
      silent_redirect_uri: "http://localhost:4200/refresh",
      post_logout_redirect_uri: "http://localhost:4200",
      // automaticSilentRenew: true, //указывающий, должна ли быть автоматическая попытка обновить токен доступа до истечения срока его действия
      // scope: "openid profile Order",
      // loadUserInfo: true,// загрузкой дополнительных идентификационных данных, чтобы заполнить пользователя profile
  });

  constructor( private router: Router) {
    this.manager.getUser().then(user => {
      this.user = user!;
    });
  }

  login(): Promise<void> {
    return this.manager.signinRedirect();
  }
  
  logout(): Promise<void> {
    return this.manager.signoutRedirect();
  }

  isLogein(): boolean {
    return this.user != null && !this.user.expired;
  }

  authenticationCallback(): Promise<void> {
    return this.manager.signinRedirectCallback()
      .then(user => {
        this.user = user;
        this.router.navigate(['/']);
      });
  }

  getAuthorizationHeaderValue(): HttpHeaders {
    let headers = new HttpHeaders();
    if(this.isLogein()){
      headers = new HttpHeaders({ 'Authorization': `${this.user.token_type} ${this.user.access_token}`});
    }
    return headers;
  }

  canActivate(): boolean {
    if (this.isLogein()) {
      return true;
    }

    this.login();
    return false;
  }

  // ------------------------------------------------------------------



  public subscribeevents(): void {
    this.manager.events.addSilentRenewError(() => {
        // console.log("error SilentRenew");
    });

    this.manager.events.addAccessTokenExpiring(() => {
        // console.log("access token expiring");
    });

    this.manager.events.addAccessTokenExpired(() => {
        // console.log("access token expired");
    });
}

  getClaims(): any {
    return this.user.profile;
  }

  public refreshCallBack(): void {
    this.manager.signinSilentCallback();
  }

}

//   private _userManager: UserManager;
//   private _user: any;
//   private _loginChangedSubject = new Subject<boolean>();
//   public loginChanged = this._loginChangedSubject.asObservable();
//   private get idpSettings() : UserManagerSettings {
//     return {
//       userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }), //чтоб хронилась сесия localStore
//       authority: "http://localhost:1001",
//       silent_redirect_uri: "http://localhost:4200/refresh",
//       redirect_uri: "http://localhost:4200/auth-callback",
//       post_logout_redirect_uri: "http://localhost:4200/",
//       response_type: "code",
//       automaticSilentRenew: true, //указывающий, должна ли быть автоматическая попытка обновить токен доступа до истечения срока его действия
//       // scope: "openid profile Order",
//       scope: "openid profile",
//       client_id: "client_angular_id",
//       loadUserInfo: true,// загрузкой дополнительных идентификационных данных, чтобы заполнить пользователя profile

//     }
//   }
//   constructor() { 
//     this._userManager = new UserManager(this.idpSettings);
//   }

//   public login = () => {
//     return this._userManager.signinRedirect();
//   }

//   public isAuthenticated = (): Promise<boolean> => {
//     return this._userManager.getUser()
//     .then(user => {
//       if(this._user !== user){
//         this._loginChangedSubject.next(this.checkUser(this._user));
//       }
//       this._user = user;
        
//       return this.checkUser(this._user);
//     })
//   }

//   private checkUser = (user : User): boolean => {
//     return !!user && !user.expired;
//   }
// }
