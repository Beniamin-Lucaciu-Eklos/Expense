import { HttpErrorResponse } from '@angular/common/http';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { EMPTY, Subject } from 'rxjs';
import { catchError, finalize, switchMap, tap } from 'rxjs/operators';
import { MonthlySummary, TransactionTotals } from '../../models/monthly-summary';
import { Transaction } from '../../models/transaction';
import { getErrorMessage } from '../../shared/utils/error-message';
import { TransactionService } from '../../services/transaction.service';

const TRANSACTIONS_PER_PAGE = 50;

@Component({
  selector: 'app-transaction-list',
  imports: [NgClass, CurrencyPipe, DatePipe],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList implements OnInit {
  private readonly transactionService = inject(TransactionService);
  private readonly router = inject(Router);
  private readonly pageRequests = new Subject<number>();

  readonly transactions = signal<Transaction[]>([]);
  readonly currentPage = signal(1);
  readonly totalCount = signal(0);
  readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / TRANSACTIONS_PER_PAGE)),
  );
  readonly firstTransactionNumber = computed(() =>
    this.totalCount() === 0 ? 0 : (this.currentPage() - 1) * TRANSACTIONS_PER_PAGE + 1,
  );
  readonly lastTransactionNumber = computed(() =>
    Math.min(this.currentPage() * TRANSACTIONS_PER_PAGE, this.totalCount()),
  );
  readonly isTransactionsLoading = signal(true);
  readonly transactionsError = signal('');
  readonly monthlySummary = signal<MonthlySummary | null>(null);
  readonly selectedMonth = signal(this.getCurrentMonth());
  readonly summaryError = signal('');
  readonly isSummaryLoading = signal(false);
  readonly allTimeSummary = signal<TransactionTotals | null>(null);
  readonly totalsError = signal('');
  readonly isTotalsLoading = signal(true);

  constructor() {
    this.pageRequests
      .pipe(
        switchMap(page => {
          this.currentPage.set(page);
          this.isTransactionsLoading.set(true);
          this.transactionsError.set('');

          return this.transactionService.getTransactionsPage(page, TRANSACTIONS_PER_PAGE).pipe(
            tap(response => {
              this.transactions.set(response.items);
              this.totalCount.set(response.totalCount);
            }),
            catchError((error: HttpErrorResponse) => {
              this.transactionsError.set(getErrorMessage(error));
              return EMPTY;
            }),
            finalize(() => this.isTransactionsLoading.set(false)),
          );
        }),
        takeUntilDestroyed(),
      )
      .subscribe();
  }

  ngOnInit(): void {
    this.loadTransactions();
    this.loadMonthlySummary();
    this.loadAllTimeSummary();
  }

  private getCurrentMonth(): string {
    const now = new Date();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    return `${now.getFullYear()}-${month}`;
  }

  loadTransactions(page = this.currentPage()): void {
    this.pageRequests.next(Math.max(1, page));
  }

  onMonthChange(event: Event): void {
    const monthValue = (event.target as HTMLInputElement).value;
    if (!monthValue) {
      return;
    }

    this.selectedMonth.set(monthValue);
    this.loadMonthlySummary();
  }

  private loadMonthlySummary(): void {
    const [year, month] = this.selectedMonth().split('-').map(Number);
    this.isSummaryLoading.set(true);
    this.summaryError.set('');

    this.transactionService.getMonthlySummary(year, month).subscribe({
      next: summary => {
        this.monthlySummary.set(summary);
        this.isSummaryLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.monthlySummary.set(null);
        this.summaryError.set(getErrorMessage(error));
        this.isSummaryLoading.set(false);
      },
    });
  }

  private loadAllTimeSummary(): void {
    this.isTotalsLoading.set(true);
    this.totalsError.set('');

    this.transactionService.getTransactionTotals().subscribe({
      next: summary => {
        this.allTimeSummary.set(summary);
        this.isTotalsLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.allTimeSummary.set(null);
        this.totalsError.set(getErrorMessage(error));
        this.isTotalsLoading.set(false);
      },
    });
  }

  goToPreviousPage(): void {
    if (this.currentPage() > 1 && !this.isTransactionsLoading()) {
      this.loadTransactions(this.currentPage() - 1);
    }
  }

  goToNextPage(): void {
    if (this.currentPage() < this.totalPages() && !this.isTransactionsLoading()) {
      this.loadTransactions(this.currentPage() + 1);
    }
  }

  editTransaction(transaction: Transaction): void {
    if (transaction.id) {
      this.router.navigate(['transactions/edit/', transaction.id]);
    }
  }

  deleteTransaction(transaction: Transaction, closeButton: HTMLButtonElement): void {
    if (transaction.id) {
      this.transactionService.delete(transaction.id).subscribe({
        next: () => {
          const nextPage =
            this.transactions().length === 1 && this.currentPage() > 1
              ? this.currentPage() - 1
              : this.currentPage();
          this.loadTransactions(nextPage);
          this.loadMonthlySummary();
          this.loadAllTimeSummary();
          closeButton.click();
        },
        error: (error: HttpErrorResponse) => console.error(getErrorMessage(error)),
      });
    }
  }
}

