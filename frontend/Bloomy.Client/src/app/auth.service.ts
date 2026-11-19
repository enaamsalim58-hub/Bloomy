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

  private currentUserSubject =
    new BehaviorSubject<User | null>(null);

  currentUser$ = this.currentUserSubject.asObservable();

  constructor() {

    // استرجاع حسابات العملاء المحفوظة
    const savedCustomers = localStorage.getItem('bloomy_customers');

    if (savedCustomers) {
      try {
        this.customers = JSON.parse(savedCustomers);
      } catch {
        this.customers = [];
      }
    }

    // استرجاع بيانات المدير المحفوظة
    const savedAdmin = localStorage.getItem('bloomy_admin');

    if (savedAdmin) {
      try {
        this.admin = JSON.parse(savedAdmin);
      } catch {
        // الاحتفاظ ببيانات المدير الافتراضية
      }
    }

    // حذف جلسة تسجيل الدخول السابقة
    // حتى يبدأ المشروع دائماً بواجهة الزائر
    localStorage.removeItem('bloomy_current_user');

  }

  get currentUser(): User | null {
    return this.currentUserSubject.value;
  }

  // حفظ حسابات العملاء
  private saveCustomers(): void {
    localStorage.setItem(
      'bloomy_customers',
      JSON.stringify(this.customers)
    );
  }

  // تحديد المستخدم الحالي خلال الجلسة
  private setCurrentUser(user: User): void {
    this.currentUserSubject.next(user);
  }

  // تسجيل الدخول
  login(email: string, password: string): User | null {

    const normalizedEmail = email.trim().toLowerCase();

    // التحقق من حساب المدير
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

    // التحقق من حساب العميل
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

  // إنشاء حساب عميل جديد
  register(data: {
    fullName: string;
    email: string;
    phone: string;
    password: string;
  }): boolean {

    const email = data.email.trim().toLowerCase();

    const emailExists =
      email === this.admin.email.toLowerCase() ||
      this.customers.some(
        c => c.email.toLowerCase() === email
      );

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

  // تحديث بيانات العميل
  updateCustomerProfile(
    fullName: string,
    email: string,
    phone: string
  ): boolean {

    const user = this.currentUser;

    if (!user || user.role !== 'customer') {
      return false;
    }

    const newEmail = email.trim().toLowerCase();

    if (!fullName.trim() || !newEmail) {
      return false;
    }

    const emailExists =
      newEmail === this.admin.email.toLowerCase() ||
      this.customers.some(
        c =>
          c.email.toLowerCase() === newEmail &&
          c.email.toLowerCase() !== user.email.toLowerCase()
      );

    if (emailExists) {
      return false;
    }

    const customer = this.customers.find(
      c => c.email.toLowerCase() === user.email.toLowerCase()
    );

    if (!customer) {
      return false;
    }

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

  // تغيير كلمة مرور العميل
  changeCustomerPassword(
    currentPassword: string,
    newPassword: string
  ): boolean {

    const user = this.currentUser;

    if (
      !user ||
      user.role !== 'customer' ||
      newPassword.length < 3
    ) {
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

  // تحديث بيانات المدير
  updateAdminProfile(
    fullName: string,
    email: string
  ): boolean {

    const newEmail = email.trim().toLowerCase();

    if (!fullName.trim() || !newEmail) {
      return false;
    }

    const emailExists = this.customers.some(
      c => c.email.toLowerCase() === newEmail
    );

    if (emailExists) {
      return false;
    }

    this.admin.fullName = fullName.trim();
    this.admin.email = newEmail;

    localStorage.setItem(
      'bloomy_admin',
      JSON.stringify(this.admin)
    );

    this.setCurrentUser({
      fullName: this.admin.fullName,
      email: this.admin.email,
      role: 'admin'
    });

    return true;
  }

  // تغيير كلمة مرور المدير
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

    localStorage.setItem(
      'bloomy_admin',
      JSON.stringify(this.admin)
    );

    return true;
  }

  // تسجيل الخروج
  logout(): void {

    this.currentUserSubject.next(null);

    localStorage.removeItem('bloomy_current_user');

  }

  // هل المستخدم مسجل الدخول؟
  isLoggedIn(): boolean {
    return this.currentUser !== null;
  }

  // هل المستخدم مدير؟
  isAdmin(): boolean {
    return this.currentUser?.role === 'admin';
  }

  // هل المستخدم عميل؟
  isCustomer(): boolean {
    return this.currentUser?.role === 'customer';
  }

}