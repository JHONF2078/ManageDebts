import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MATERIAL_IMPORTS } from '../../material/material.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  imports: [...MATERIAL_IMPORTS, FormsModule, CommonModule, RouterModule]
})
export class RegisterComponent {
  username = '';
  fullName = '';
  password = '';
  confirmPassword = '';
  errorMessages: string[] = [];

  constructor(private router: Router, private authService: AuthService) { }

  async register() {
    if (this.password !== this.confirmPassword) {
      this.errorMessages = ['Las contraseñas no coinciden'];
      return;
    }
    try {
      const data = await this.authService.register(this.username, this.password, this.fullName);
      this.authService.setToken(data.token);
      this.router.navigate(['/']);
    } catch (err: any) {
      const detail = err?.error?.detail || err.message;
      this.errorMessages = detail.split(' | ');
    }
  }
}
