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
    const exp = localStorage.getItem('token_exp');
    if (!token || !exp) return false;
    // expirationUtc viene en formato ISO UTC
    const nowUtc = new Date(new Date().toISOString());
    const expiration = new Date(exp);
    return expiration.getTime() > nowUtc.getTime();
  }
}
