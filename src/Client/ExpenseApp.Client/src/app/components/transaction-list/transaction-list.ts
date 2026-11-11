import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { Transaction } from '../../models/transaction';
import { TransactionService } from '../../services/transaction.service';
import { NgClass, CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-transaction-list',
  imports: [NgClass, CurrencyPipe, DatePipe],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList implements OnInit {

  private transactionService = inject(TransactionService);

  transactions: WritableSignal<Transaction[]> = signal([]);

  ngOnInit(): void {
    this.initialize();
  }

  initialize() {
    this.getTransactions();
  }

  getTransactions() {
    this.transactionService.getAll().subscribe({
      next: response => this.transactions.set(response),
      error: error => console.log(error)
    })
  }

  getTransactionsTotalByType(type: string): number {
    return this.transactions().filter(t => t.type === type).reduce((sum, t) => sum + t.amount, 0);
  }

  getTotalIncome(): number {
    return this.getTransactionsTotalByType('Income');
  }

  getTotalExpense(): number {
    return this.getTransactionsTotalByType('Expense');
  }

  getNetBalance(): number {
    return this.getTotalIncome() - this.getTotalExpense();
  }
}

