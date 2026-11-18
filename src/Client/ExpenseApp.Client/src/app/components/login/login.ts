import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { getErrorMessage } from '../../shared/utils/error-message';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  loginForm: FormGroup;
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  errorMessage = signal('');

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.loginForm.get(controlName);
    return !!control && (control.touched || control.dirty) && control.hasError(errorName);
  }

  onSubmit(): void {
    this.errorMessage.set('');

    if (this.loginForm.valid) {
      const login = this.loginForm.value;

      this.authService.login(login).subscribe({
        next: () => {
          this.router.navigate(['/transactions']);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage.set(getErrorMessage(err, 'An error occurred during login. Try again.'));
        }
      });
    }
  }
}
