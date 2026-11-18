import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../models/user';
import { Observable, tap } from 'rxjs';
import { AuthResponse } from '../models/authresponse';
import { Router } from '@angular/router';
import { CurrentUser } from '../models/CurrentUser';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5136/api/Auth';
  private http = inject(HttpClient);
  private router = inject(Router);

  private _currentUser = signal<CurrentUser | null>(this.loadUser());
  currentUser = this._currentUser.asReadonly();
  isLoggedIn = computed(() => !!this._currentUser());

  login(credentials: User): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(res => this.setSession(res))
    );
  }

  register(credentials: User): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, credentials).pipe(
      tap(res => this.setSession(res))   // optional: log in right after register
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }

  private setSession(res: AuthResponse): void {
    const user: CurrentUser = { email: res.email };
    localStorage.setItem('token', res.token);
    localStorage.setItem('user', JSON.stringify(user));
    this._currentUser.set(user);
  }

  private loadUser(): CurrentUser | null {
    try {
      const raw = localStorage.getItem('user');
      return localStorage.getItem('token') && raw ? JSON.parse(raw) : null;
    } catch {
      return null;
    }
  }

  isAuthenticated(): boolean { return !!localStorage.getItem('token'); }
  getToken(): string | null { return localStorage.getItem('token'); }
  get isLoginVisible(): boolean {
    return !this.isLoggedIn() && !this.router.url.includes('/login');
  }
}
