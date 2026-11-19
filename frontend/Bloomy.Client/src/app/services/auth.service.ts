import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';

export type UserRole = 'admin' | 'user';

export interface AuthSession {
  token: string;
  role: UserRole;
  email?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private router = inject(Router);
  private readonly storageKey = 'bloomy_auth_session';

  setSession(session: AuthSession): void {
    localStorage.setItem(this.storageKey, JSON.stringify(session));
  }

  getSession(): AuthSession | null {
    const raw = localStorage.getItem(this.storageKey);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      return null;
    }
  }

  getRole(): UserRole | null {
    return this.getSession()?.role ?? null;
  }

  isLoggedIn(): boolean {
    return Boolean(this.getSession()?.token);
  }

  isAdmin(): boolean {
    return this.getRole() === 'admin';
  }

  redirectByRole(): void {
    const role = this.getRole();
    if (role === 'admin') {
      this.router.navigate(['/manage-products']);
    } else {
      this.router.navigate(['/products']);
    }
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.router.navigate(['/login']);
  }
}