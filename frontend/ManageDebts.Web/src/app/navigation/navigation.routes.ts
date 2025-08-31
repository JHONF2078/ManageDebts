import { Routes } from '@angular/router';
import { authGuard } from '../auth/auth.guard';

export const NAVIGATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/navigation-page.component')
        .then(m => m.NavigationPageComponent),
    children: [
      {
        path: '',
        redirectTo: 'debts',
        pathMatch: 'full',
      },
      {
        path: 'debts', // Asegurar que el path sea accesible desde /
        canActivate: [authGuard],
        loadComponent: () =>
          import('../debt/pages/page-debt.component')
            .then(m => m.PageDebtComponent),
      },
    ]
  }
];
