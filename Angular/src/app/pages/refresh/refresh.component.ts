import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth';

@Component({
  selector: 'app-refresh',
  template:'',
  styles: []
})
export class RefreshComponent {

  constructor(
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    debugger;
    this.authService.refreshCallBack();
  }

}
