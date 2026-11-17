import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; // 1. استيراد أداة التوجيه

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  isLoginMode: boolean = true;
  showPassword: boolean = false;
  showRegisterPassword: boolean = false;

  loginData = {
    email: '',
    password: ''
  };

  registerData = {
    fullName: '',
    email: '',
    phone: '',
    password: ''
  };

  // 2. حقن الـ Router في البناء (constructor)
  constructor(private router: Router) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleRegisterPasswordVisibility() {
    this.showRegisterPassword = !this.showRegisterPassword;
  }

  onLogin() {
    console.log('بيانات تسجيل الدخول:', this.loginData);
    alert('تم تسجيل الدخول بنجاح!');
    
    // 3. التوجيه تلقائياً إلى الصفحة الرئيسية للموقع بعد تسجيل الدخول
    this.router.navigate(['/']);
  }

  onRegister() {
    console.log('بيانات إنشاء حساب:', this.registerData);
    alert('تم إنشاء الحساب بنجاح!');
    this.router.navigate(['/']);
  }
}