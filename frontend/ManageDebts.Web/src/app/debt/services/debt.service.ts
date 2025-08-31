
export interface CreateDebtDto {
  amount: number;
  description: string;
  creditorId: string;
}
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, of, tap } from 'rxjs';
import { Debt } from '../interface/debt.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class DebtService {
  getDebtById(id: string): Observable<Debt> {
    const token = localStorage.getItem('token');
    return this.http.get<Debt>(`${this.apiUrl}/${id}`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });
  }

  private apiUrl = `${environment.apiUrl}/Debts`;

  constructor(private http: HttpClient) {
    this.loadDebts();
  }

  private booksSubject = new BehaviorSubject<Debt[]>([]);

  ListSalesDatePredictions$ = this.booksSubject.asObservable();

  createDebt(debt: CreateDebtDto): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.post<any>(this.apiUrl, debt, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    }).pipe(
      tap(() => this.loadDebts())
    );
  }

  updateDebt(id: string, debt: CreateDebtDto): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.put<any>(`${this.apiUrl}/${id}`, debt, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    }).pipe(
      tap(() => this.loadDebts())
    );
  }

  payDebt(id: string): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.post<any>(`${this.apiUrl}/${id}/pay`, {}, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });
  }

  deleteDebt(id: string): Observable<any> {
    const token = localStorage.getItem('token');
    return this.http.delete<any>(`${this.apiUrl}/${id}`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    }).pipe(
      tap(() => this.loadDebts())
    );
  }


  private loadDebts(): void {
    const token = localStorage.getItem('token');
    this.http.get<Debt[]>(this.apiUrl, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    }).subscribe(debts => {
      this.booksSubject.next(debts);
    });
  }

  /**
   * Buscar lista de deudas
   * @param customerName Nombre del cliente.
   * @returns Observable con las lista de deudas.
   */
  searchDebts(fullName: string): Observable<Debt[]> {
    const url = `${this.apiUrl}?fullName=${encodeURIComponent(fullName)}`;
    const token = localStorage.getItem('token');
    return this.http.get<Debt[]>(url, {
      headers: token ? { Authorization: `Bearer ${token}` } : {}
    });
  }

  getDebts(): Observable<Debt[]> {
    return this.ListSalesDatePredictions$; // Se retorna directamente sin redundancias
  }
}
