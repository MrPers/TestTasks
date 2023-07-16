import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { CallbackComponent } from './pages/auth-callback/callback.component';
import { RefreshComponent } from './pages/refresh/refresh.component';

@NgModule({
  declarations: [
    AppComponent,
    RefreshComponent,
    CallbackComponent,
  ],
  imports: [
    // HttpClientModule, // need child module
    BrowserModule,
    HttpClientModule,
    AppRoutes
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
