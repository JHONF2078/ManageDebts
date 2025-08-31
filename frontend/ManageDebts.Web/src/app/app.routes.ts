import { Routes } from '@angular/router';
// El guard solo se aplicará en navigation.routes.ts

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: '',
    loadChildren: () =>
      import('./navigation/navigation.routes') // standalone route file
        .then(m => m.NAVIGATION_ROUTES)
  }
];
