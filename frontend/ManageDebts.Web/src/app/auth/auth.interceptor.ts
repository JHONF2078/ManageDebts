import { HttpInterceptorFn } from '@angular/common/http';
import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const snackBar = inject(MatSnackBar);
  // Excepciones: no agregar token en login, register y endpoints públicos
  const publicEndpoints = ['/login', '/register', '/Account/login', '/Account/register'];
  const isPublic = publicEndpoints.some(url => req.url.includes(url));

  const token = sessionStorage.getItem('token');
  let authReq = req;
  if (token && !isPublic) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // Manejo global: token expirado (401)
      if (error.status === 401) {
        const detail = error?.error?.detail || error?.error?.message || '';
        if (detail.toLowerCase().includes('expired') || detail.toLowerCase().includes('expirado')) {
          sessionStorage.removeItem('token');
          sessionStorage.removeItem('token_exp');
          snackBar.open('Tu sesión ha expirado. Por favor inicia sesión nuevamente.', 'Cerrar', {
            duration: 5000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
          window.location.href = '/login';
        } else {
          snackBar.open('No autorizado: ' + (detail || 'No tienes permisos para esta acción.'), 'Cerrar', {
            duration: 5000,
            verticalPosition: 'top',
            horizontalPosition: 'center'
          });
        }
      } else if (error.status === 400) {
        const detail = error?.error?.detail || error?.error?.message || '';
        snackBar.open('Error de datos enviados: ' + (detail || 'Verifica los campos e intenta nuevamente.'), 'Cerrar', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      } else if (error.status === 403) {
        snackBar.open('Acceso denegado: No tienes permisos para esta acción.', 'Cerrar', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      } else if (error.status === 404) {
        snackBar.open('No encontrado: El recurso solicitado no existe.', 'Cerrar', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      } else if (error.status === 500) {
        snackBar.open('Error interno del servidor. Intenta más tarde.', 'Cerrar', {
          duration: 5000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
        });
      }
      return throwError(() => error);
    })
  );
};
