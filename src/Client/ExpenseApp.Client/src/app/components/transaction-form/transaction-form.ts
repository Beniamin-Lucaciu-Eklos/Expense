import { CommonModule } from '@angular/common';
import { Component, inject, isWritableSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './transaction-form.html',
  styleUrl: './transaction-form.css',
})
export class TransactionForm implements OnInit {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private transactionService = inject(TransactionService);

  transactionForm: FormGroup;
  incomeCategories = [
    'Salary',
    'Freelance',
    'Investment'
  ];
  expenseCategories = [
    "Food",
    "Transportation",
    "Entertaiment"
  ];
  availableCategories: string[] = [];

  transactionId?: number;
  isEditModel: WritableSignal<boolean> = signal(false);

  constructor() {
    this.transactionForm = this.fb.group({
      type: ['Expense', Validators.required],
      category: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0)]],
      description: ['', [Validators.required, Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    const type = this.transactionForm.get('type')?.value;
    this.updateAvailableCategories(type);

    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.transactionId = +id;
      this.isEditModel.set(true);
      this.loadTransaction(this.transactionId);
    }
  }
  loadTransaction(id: number) {
    this.transactionService.getById(id).subscribe({
      next: transaction => {
        this.transactionForm.patchValue({
          type: transaction.type,
          category: transaction.category,
          amount: transaction.amount,
          description: transaction.description
        })
      },
      error: error => console.log(error)
    })
  }

  onTypeChange() {
    const type = this.transactionForm.get('type')?.value;
    this.updateAvailableCategories(type);
  }

  updateAvailableCategories(type: string) {
    this.availableCategories = type === 'Expense' ? this.expenseCategories : this.incomeCategories;
    this.transactionForm.patchValue({ category: '' });
  }
  onSubmit() {
    if (this.transactionForm.valid) {
      const transaction = this.transactionForm.value;

      if (this.isEditModel() && this.transactionId) {
        this.transactionService.update(this.transactionId, transaction).subscribe({
          next: data => this.navigateToTransactionPage(),
          error: error => console.log(error)
        });
      }
      else {
        this.transactionService.create(transaction).subscribe({
          next: data => this.navigateToTransactionPage(),
          error: error => console.log(error)
        });
      }
    }
  }

  cancel() {
    this.navigateToTransactionPage()
  }

  navigateToTransactionPage(): void {
    this.router.navigate(['/transactions'])
  }
}
