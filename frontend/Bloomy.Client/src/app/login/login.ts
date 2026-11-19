import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {

  isLoginMode = true;
  isProfileMode = false;

  showPassword = false;
  showRegisterPassword = false;

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

  profileData = {
    fullName: '',
    email: '',
    phone: '',
    currentPassword: '',
    newPassword: ''
  };

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(['/home']);
    }
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleRegisterPasswordVisibility(): void {
    this.showRegisterPassword = !this.showRegisterPassword;
  }

  showRegisterForm(): void {
    this.isLoginMode = false;
  }

  showLoginForm(): void {
    this.isLoginMode = true;
  }

  onLogin(): void {

    if (!this.loginData.email.trim() || !this.loginData.password) {
      alert('يرجى إدخال البريد الإلكتروني وكلمة المرور');
      return;
    }

    const user = this.authService.login(
      this.loginData.email.trim(),
      this.loginData.password
    );

    if (!user) {
      alert('البريد الإلكتروني أو كلمة المرور غير صحيحة');
      return;
    }

    alert(
      user.role === 'admin'
        ? 'مرحباً بك أيها المدير في Bloomy 🌸'
        : 'مرحباً بك في Bloomy 🌸'
    );

    this.router.navigate(['/home']);
  }

  onRegister(): void {

    if (
      !this.registerData.fullName.trim() ||
      !this.registerData.email.trim() ||
      !this.registerData.phone.trim() ||
      !this.registerData.password
    ) {
      alert('يرجى تعبئة جميع البيانات');
      return;
    }

    const success = this.authService.register(this.registerData);

    if (!success) {
      alert('هذا البريد الإلكتروني مسجل مسبقاً');
      return;
    }

    const user = this.authService.login(
      this.registerData.email,
      this.registerData.password
    );

    if (!user) {
      alert('تم إنشاء الحساب، يرجى تسجيل الدخول');
      this.isLoginMode = true;
      return;
    }

    alert('تم إنشاء حسابك بنجاح، أهلاً بك في Bloomy 🌸');

    this.router.navigate(['/home']);
  }

  saveProfile(): void {

    const success = this.authService.updateCustomerProfile(
      this.profileData.fullName,
      this.profileData.email,
      this.profileData.phone
    );

    if (!success) {
      alert('تعذر حفظ البيانات. تأكدي من البريد الإلكتروني.');
      return;
    }

    if (this.profileData.newPassword) {

      const passwordChanged =
        this.authService.changeCustomerPassword(
          this.profileData.currentPassword,
          this.profileData.newPassword
        );

      if (!passwordChanged) {
        alert(
          'تم حفظ المعلومات، لكن كلمة المرور الحالية غير صحيحة أو كلمة المرور الجديدة قصيرة.'
        );
        return;
      }
    }

    alert('تم حفظ التعديلات بنجاح 🌸');

    this.profileData.currentPassword = '';
    this.profileData.newPassword = '';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}