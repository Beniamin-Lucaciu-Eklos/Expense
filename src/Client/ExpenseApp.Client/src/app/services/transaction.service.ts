import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {

  private apiUrl = 'http://localhost:5136/api';
  private http = inject(HttpClient);

  getAll(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.apiUrl + '/transactions');
  }

  getById(id: number): Observable<Transaction> {
    return this.http.get<Transaction>(this.apiUrl + '/transactions/' + id);
  }

  create(transaction: Transaction): Observable<Transaction> {
    return this.http.post<Transaction>(this.apiUrl + '/transactions', transaction);
  }

  update(id: number, transaction: Transaction): Observable<Transaction> {
    return this.http.put<Transaction>(this.apiUrl + '/transactions/' + id, transaction);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(this.apiUrl + '/transactions/' + id);
  }
}
