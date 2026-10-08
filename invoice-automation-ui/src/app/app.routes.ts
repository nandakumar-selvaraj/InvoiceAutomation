import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/dashboard/dashboard').then((m) => m.Dashboard)
  },
  {
    path: 'invoices',
    loadComponent: () => import('./components/invoice-details/invoice-details').then((m) => m.InvoiceDetails)
  }
];
