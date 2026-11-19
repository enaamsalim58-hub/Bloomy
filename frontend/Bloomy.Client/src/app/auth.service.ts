
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface User {
  fullName: string;
  email: string;
  phone?: string;
  role: 'admin' | 'customer';
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private admin = {
    fullName: 'مدير Bloomy',
    email: 'admin@gmail.com',
    password: '123'
  };

  private customers: {
    fullName: string;
    email: string;
    phone: string;
    password: string;
  }[] = [];

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    // استرجاع حسابات العملاء
    const savedCustomers = localStorage.getItem('bloomy_customers');

    if (savedCustomers) {
      this.customers = JSON.parse(savedCustomers);
    }

    // استرجاع بيانات المدير
    const savedAdmin = localStorage.getItem('bloomy_admin');

    if (savedAdmin) {
      this.admin = JSON.parse(savedAdmin);
    }

    // استرجاع المستخدم المسجل دخوله
    const savedUser = localStorage.getItem('bloomy_current_user');

    if (savedUser) {
      this.currentUserSubject.next(JSON.parse(savedUser));
    }
  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  private saveCustomers(): void {
    localStorage.setItem(
      'bloomy_customers',
      JSON.stringify(this.customers)
    );
  }

  private setCurrentUser(user: User): void {
    this.currentUserSubject.next(user);

    localStorage.setItem(
      'bloomy_current_user',
      JSON.stringify(user)
    );
  }

  login(email: string, password: string): User | null {
    const normalizedEmail = email.trim().toLowerCase();

    if (
      normalizedEmail === this.admin.email.toLowerCase() &&
      password === this.admin.password
    ) {
      const user: User = {
        fullName: this.admin.fullName,
        email: this.admin.email,
        role: 'admin'
      };

      this.setCurrentUser(user);
      return user;
    }

    const customer = this.customers.find(
      c =>
        c.email.toLowerCase() === normalizedEmail &&
        c.password === password
    );

    if (customer) {
      const user: User = {
        fullName: customer.fullName,
        email: customer.email,
        phone: customer.phone,
        role: 'customer'
      };

      this.setCurrentUser(user);
      return user;
    }

    return null;
  }

  register(data: {
    fullName: string;
    email: string;
    phone: string;
    password: string;
  }): boolean {

    const email = data.email.trim().toLowerCase();

    const emailExists =
      email === this.admin.email.toLowerCase() ||
      this.customers.some(c => c.email.toLowerCase() === email);

    if (emailExists) {
      return false;
    }

    this.customers.push({
      fullName: data.fullName.trim(),
      email,
      phone: data.phone.trim(),
      password: data.password
    });

    this.saveCustomers();

    return true;
  }

  updateCustomerProfile(
    fullName: string,
    email: string,
    phone: string
  ): boolean {

    const user = this.currentUser;

    if (!user || user.role !== 'customer') return false;

    const newEmail = email.trim().toLowerCase();

    if (!fullName.trim() || !newEmail) return false;

    const emailExists =
      newEmail === this.admin.email.toLowerCase() ||
      this.customers.some(
        c =>
          c.email.toLowerCase() === newEmail &&
          c.email.toLowerCase() !== user.email.toLowerCase()
      );

    if (emailExists) return false;

    const customer = this.customers.find(
      c => c.email.toLowerCase() === user.email.toLowerCase()
    );

    if (!customer) return false;

    customer.fullName = fullName.trim();
    customer.email = newEmail;
    customer.phone = phone.trim();

    this.saveCustomers();

    this.setCurrentUser({
      fullName: customer.fullName,
      email: customer.email,
      phone: customer.phone,
      role: 'customer'
    });

    return true;
  }

  changeCustomerPassword(
    currentPassword: string,
    newPassword: string
  ): boolean {

    const user = this.currentUser;

    if (!user || user.role !== 'customer' || newPassword.length < 3) {
      return false;
    }

    const customer = this.customers.find(
      c => c.email.toLowerCase() === user.email.toLowerCase()
    );

    if (!customer || customer.password !== currentPassword) {
      return false;
    }

    customer.password = newPassword;
    this.saveCustomers();

    return true;
  }

  updateAdminProfile(fullName: string, email: string): boolean {
    const newEmail = email.trim().toLowerCase();

    if (!fullName.trim() || !newEmail) return false;

    const emailExists = this.customers.some(
      c => c.email.toLowerCase() === newEmail
    );

    if (emailExists) return false;

    this.admin.fullName = fullName.trim();
    this.admin.email = newEmail;

    localStorage.setItem('bloomy_admin', JSON.stringify(this.admin));

    this.setCurrentUser({
      fullName: this.admin.fullName,
      email: this.admin.email,
      role: 'admin'
    });

    return true;
  }

  changeAdminPassword(
    currentPassword: string,
    newPassword: string
  ): boolean {

    if (
      this.admin.password !== currentPassword ||
      newPassword.length < 3
    ) {
      return false;
    }

    this.admin.password = newPassword;

    localStorage.setItem('bloomy_admin', JSON.stringify(this.admin));

    return true;
  }

  logout(): void {
    this.currentUserSubject.next(null);
    localStorage.removeItem('bloomy_current_user');
  }

  isLoggedIn(): boolean {
    return this.currentUser !== null;
  }

  isAdmin(): boolean {
    return this.currentUser?.role === 'admin';
  }

  isCustomer(): boolean {
    return this.currentUser?.role === 'customer';
  }
}
