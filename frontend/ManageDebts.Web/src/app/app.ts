import { Component, signal } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from './auth/services/auth.service';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected readonly title = signal('ManageDebts.Web');

  constructor(private router: Router, private snackBar: MatSnackBar, private authService: AuthService) {
    const token = localStorage.getItem('token');
    const exp = localStorage.getItem('token_exp');
    const currentUrl = window.location.pathname;
    // Si no hay token y no está en login/register, redirigir a login
    if (!token && !currentUrl.includes('login') && !currentUrl.includes('register')) {
      this.router.navigate(['/login']);
    } else if (token && exp && !this.authService.isAuthenticated()) {
      // Solo eliminar el token si realmente está expirado y existe fecha de expiración
      this.snackBar.open('Tu sesión ha expirado. Por favor inicia sesión nuevamente.', 'Cerrar', {
        duration: 5000,
        verticalPosition: 'top',
        horizontalPosition: 'center'
      });
      this.authService.logout();
      this.router.navigate(['/login']);
    }
  }
}
