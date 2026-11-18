import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { User } from '../models/user';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AuthResponse } from '../models/authresponse';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5136/api/Auth';
  private currentUserSubject = new BehaviorSubject<string | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  private http = inject(HttpClient);
  private router = inject(Router);

  constructor() {
    const token = localStorage.getItem('token');
    if (token) {
      this.currentUserSubject.next('user');
    }
  }

  login(credentials: User): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.apiUrl + '/login', credentials)
      .pipe(
        tap(
          (response) => {
            localStorage.setItem('token', response.token);
            this.currentUserSubject.next('user');
          })
      );
  }

  register(credentials: User): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.apiUrl + '/register', credentials)
      .pipe(
        tap(
          (response) => {
            localStorage.setItem('token', response.token);
          })
      );;
  }

  logout(): void {
    this.currentUserSubject.next(null);
    localStorage.removeItem("token");
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  get isLoginVisible(): boolean {
    return !this.isAuthenticated() && !this.router.url.includes('/login');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
