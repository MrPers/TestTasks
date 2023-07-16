import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth';


@Component({
  selector: 'app-callback',
  template:'',
  styles: []
})
export class CallbackComponent implements OnInit {

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.authService.authenticationCallback();    
  }

}
