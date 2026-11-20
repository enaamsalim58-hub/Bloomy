import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-customer-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './customer-profile.html',
  styleUrl: './customer-profile.css'
})
export class CustomerProfile {

  customerData = {
    fullName: '',
    email: '',
    phone: ''
  };

  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {

    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {
      this.router.navigate(['/login']);
      return;
    }

    this.customerData = {
      fullName: user.fullName,
      email: user.email,
      phone: user.phone || ''
    };
  }

  saveProfile(): void {

    const success = this.authService.updateCustomerProfile(
      this.customerData.fullName,
      this.customerData.email,
      this.customerData.phone
    );

    if (!success) {
      alert('تعذر تحديث البيانات. تأكدي من البريد الإلكتروني.');
      return;
    }

    alert('تم تحديث بياناتك بنجاح 🌸');
  }

  changePassword(): void {

    if (
      !this.currentPassword ||
      !this.newPassword ||
      !this.confirmPassword
    ) {
      alert('يرجى تعبئة جميع حقول كلمة المرور');
      return;
    }

    if (this.newPassword.length < 3) {
      alert('كلمة المرور الجديدة قصيرة جداً');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      alert('كلمتا المرور الجديدتان غير متطابقتين');
      return;
    }

    const success = this.authService.changeCustomerPassword(
      this.currentPassword,
      this.newPassword
    );

    if (!success) {
      alert('كلمة المرور الحالية غير صحيحة');
      return;
    }

    alert('تم تغيير كلمة المرور بنجاح 🌸');

    this.currentPassword = '';
    this.newPassword = '';
    this.confirmPassword = '';
  }

  logout(): void {
    this.authService.logout();

    // العودة إلى واجهة الزائر
    this.router.navigate(['/home']);
  }

}