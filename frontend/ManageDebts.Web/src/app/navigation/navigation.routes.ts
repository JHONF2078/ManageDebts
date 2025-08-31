import { Routes } from '@angular/router';

export const NAVIGATION_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/navigation-page.component')
        .then(m => m.NavigationPageComponent),
    children: [
      {
        path: '', // Redirigir a 'home' por defecto ss
        redirectTo: 'orders',
        pathMatch: 'full',
      },
      {
        path: 'home', // Asegurar que el path sea accesible desde /
        loadComponent: () =>
          import('../home/pages/home/home.component')
            .then(m => m.HomeComponent),
      },
      {
        path: 'orders', // Asegurar que el path sea accesible desde /
        loadComponent: () =>
          import('../debt/pages/page-debt.component')
            .then(m => m.PageDebtComponent),
      },
      // {
      //   path: 'customer-orders/:custId', // Asegurar que el path sea accesible desde /
      //   loadComponent: () =>
      //     import('../debt/components/debt-view/debt-view.component')
      //       .then(m => m.DebtViewComponent),
      // }
      // Las rutas de login y register están en app.routes.ts
    ]
  }
];
