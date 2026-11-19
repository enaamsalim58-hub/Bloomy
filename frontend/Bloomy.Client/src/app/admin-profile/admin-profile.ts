import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-admin-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './admin-profile.html',
  styleUrl: './admin-profile.css'
})
export class AdminProfile {

  adminData = {
    fullName: '',
    email: ''
  };

  passwordData = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
    const user = this.authService.currentUser;

    if (user?.role !== 'admin') {
      this.router.navigate(['/login']);
      return;
    }

    this.adminData.fullName = user.fullName;
    this.adminData.email = user.email;
  }

  saveProfile() {
    const success = this.authService.updateAdminProfile(
      this.adminData.fullName,
      this.adminData.email
    );

    if (!success) {
      alert('البريد الإلكتروني مستخدم بالفعل');
      return;
    }

    alert('تم تحديث بيانات المدير بنجاح 🌸');
  }

  changePassword() {
    if (
      !this.passwordData.currentPassword ||
      !this.passwordData.newPassword ||
      !this.passwordData.confirmPassword
    ) {
      alert('يرجى تعبئة جميع حقول كلمة المرور');
      return;
    }

    if (
      this.passwordData.newPassword !==
      this.passwordData.confirmPassword
    ) {
      alert('كلمتا المرور الجديدتان غير متطابقتين');
      return;
    }

    const success = this.authService.changeAdminPassword(
      this.passwordData.currentPassword,
      this.passwordData.newPassword
    );

    if (!success) {
      alert('كلمة المرور الحالية غير صحيحة');
      return;
    }

    alert('تم تغيير كلمة المرور بنجاح 🌸');

    this.passwordData = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}