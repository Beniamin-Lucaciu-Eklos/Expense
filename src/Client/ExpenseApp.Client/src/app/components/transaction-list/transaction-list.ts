import { Component, inject, OnInit, signal } from '@angular/core';
import { Transaction } from '../../models/transaction';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction-list',
  imports: [],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList implements OnInit {

  private transactionService = inject(TransactionService);

  transactions: Transaction[] = [];

  ngOnInit(): void {
    this.initialize();
  }

  initialize() {
    this.getTransactions();
  }

  getTransactions() {
    this.transactionService.getAll().subscribe({
      next: response => {
        this.transactions = response;
      },
      error: error => console.log(error)
    })
  }
}
