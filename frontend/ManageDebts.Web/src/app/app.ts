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
    // La protección de rutas y expiración de sesión ahora se maneja por el guard.
  }
}
