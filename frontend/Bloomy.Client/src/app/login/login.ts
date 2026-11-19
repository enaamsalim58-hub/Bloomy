import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';

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

  // حقن خدمة المصادقة AuthService
  constructor(private authService: AuthService) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  toggleRegisterPasswordVisibility() {
    this.showRegisterPassword = !this.showRegisterPassword;
  }

  onLogin() {
    console.log('بيانات تسجيل الدخول:', this.loginData);

    // تحديد الصلاحية مؤقتاً لحين ربط الـ API (إذا احتوى البريد على admin يدخل كأدمن)
    const isAdmin = this.loginData.email.trim().toLowerCase().includes('admin');

    this.authService.setSession({
      token: 'bloomy-temp-token',
      role: isAdmin ? 'admin' : 'user',
      email: this.loginData.email
    });

    alert('تم تسجيل الدخول بنجاح!');

    // التوجيه تلقائياً حسب الدور (admin -> /manage-products ، user -> /products)
    this.authService.redirectByRole();
  }

  onRegister() {
    console.log('بيانات إنشاء حساب:', this.registerData);

    // الحسابات الجديدة تكون بصلاحية مستخدم عادي (user)
    this.authService.setSession({
      token: 'bloomy-temp-token',
      role: 'user',
      email: this.registerData.email
    });

    alert('تم إنشاء الحساب بنجاح!');
    this.authService.redirectByRole();
  }
}