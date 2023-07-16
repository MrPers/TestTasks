import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { URLService } from '../services/url';

@Component({
  selector: 'app-root',
  // template:'<router-outlet></router-outlet>'
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  
  messageGet : string;
  messagePost : string;
  error$ = new Subject<string>();
  
  constructor(private url: URLService) {}
  
  ngOnInit()
  {
    this.url.postSecrets({})
    .subscribe({
      next: (result: any) => this.messagePost = result,
      error: (e:HttpErrorResponse)=> console.log(e)
    }); 
    this.url.getSecrets()
    .subscribe({
      next: (result: any) => this.messageGet = result,
      error: (e:HttpErrorResponse)=> console.log(e)
    });   
  }

  getSecrets(): void {
    this.url.getSecrets()
    .subscribe({
      next: (result: any) => this.messageGet = result,
      error: (e:HttpErrorResponse)=> console.log(e)
    });
    this.url.postSecrets({})
    .subscribe({
      next: (result: any) => this.messagePost = result,
      error: (e:HttpErrorResponse)=> console.log(e)
    });  
  }
}
