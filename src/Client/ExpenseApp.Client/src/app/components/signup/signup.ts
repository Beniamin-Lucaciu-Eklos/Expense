import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { getErrorMessage } from '../../shared/utils/error-message';

@Component({
  selector: 'app-signup',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './signup.html',
  styleUrl: './signup.css',
})
export class Signup {
  signupForm: FormGroup;
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  errorMessage = signal('');

  constructor() {
    this.signupForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmpassword: ['', [Validators.required, Validators.minLength(6)]]
    },
      {
        validator: [this.passwordMatchValidator]
      });

    this.signupForm.markAllAsTouched();
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.signupForm.get(controlName);
    return !!control && (control.touched || control.dirty) && control.hasError(errorName);
  }

  passwordMatchValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmpassword')?.value;
    return password === confirm ? null : { passwordMismatch: true };
  };

  get showMismatch(): boolean {
    const confirm = this.signupForm.get('confirmpassword');
    return this.signupForm.hasError('passwordMismatch')
      && !!(confirm?.touched || confirm?.dirty);
  }

  onSubmit(): void {

    this.errorMessage.set('');

    if (this.signupForm.valid) {
      const signup = this.signupForm.value;

      this.authService.register(signup).subscribe({
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
