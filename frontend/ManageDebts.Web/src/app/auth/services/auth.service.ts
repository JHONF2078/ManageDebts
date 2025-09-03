import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Account`;

  constructor(private http: HttpClient) { }

  async login(email: string, password: string): Promise<any> {
    const res$ = this.http.post(`${this.apiUrl}/login`, { email, password });
    return await firstValueFrom(res$);
  }

  async register(email: string, password: string, fullName: string): Promise<any> {
    const res$ = this.http.post(`${this.apiUrl}/register`, { email, password, fullName });
    return await firstValueFrom(res$);
  }

  setToken(token: string, expirationUtc?: string) {
    localStorage.setItem('token', token);
    if (expirationUtc) {
      localStorage.setItem('token_exp', expirationUtc);
    }
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('token_exp');
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      // Si el token tiene campo exp (segundos desde epoch)
      if (payload.exp) {
        const now = Math.floor(Date.now() / 1000);
        const expDate = new Date(payload.exp * 1000);
        const nowDate = new Date(now * 1000);
        console.log('Expiración:', expDate.toISOString(), 'Actual:', nowDate.toISOString());
        return payload.exp > now;
      }
    } catch { }
    // Si no tiene exp, solo verifica existencia
    return true;
  }
}
