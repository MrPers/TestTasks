import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { CallbackComponent } from './pages/auth-callback/callback.component';
import { RefreshComponent } from './pages/refresh/refresh.component';
// import { ChartCoinComponent } from './modules/coin/pages/chart-coin/chart.component';

const routes: Routes = [
  {
    path: 'callback',
    component: CallbackComponent
  },
  {
    path: 'refresh',
    component: RefreshComponent,
  },
  {
    path: '',
    component: AppComponent,
  },
  
  // {
  //   // path: 'call-api',
  //   // component: CallApiComponent
  // },
];

export const AppRoutes = RouterModule.forChild(routes);