import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Footer } from './footer/footer';
import { TransactionList } from './transaction-list/transaction-list';
import { Header } from './header/header';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Footer,
    Header],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('ExpenseApp.Client');
}
