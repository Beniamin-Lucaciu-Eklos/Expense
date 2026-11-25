import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MonthlySummary, TransactionTotals } from '../models/monthly-summary';
import { PagedTransactions } from '../models/paged-transactions';
import { Transaction } from '../models/transaction';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {

  private readonly apiUrl = 'http://localhost:5136/api';
  private http = inject(HttpClient);

  getTransactions(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(this.apiUrl + '/transactions');
  }

  getTransactionsPage(page: number, pageSize: number): Observable<PagedTransactions> {
    return this.http.get<PagedTransactions>(this.apiUrl + '/transactions/paged', {
      params: { page, pageSize },
    });
  }

  getMonthlySummary(year: number, month: number): Observable<MonthlySummary> {
    return this.http.get<MonthlySummary>(this.apiUrl + '/transactions/summary', {
      params: { year, month },
    });
  }

  getTransactionTotals(): Observable<TransactionTotals> {
    return this.http.get<TransactionTotals>(this.apiUrl + '/transactions/summary/all-time');
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
