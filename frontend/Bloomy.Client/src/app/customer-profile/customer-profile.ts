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

  saveProfile() {
    const success = this.authService.updateCustomerProfile(
      this.customerData.fullName,
      this.customerData.email,
      this.customerData.phone
    );

    alert(success ? 'تم تحديث بياناتك بنجاح 🌸' : 'تعذر تحديث البيانات. تأكدي من البريد الإلكتروني.');
  }

  changePassword() {
    if (!this.currentPassword || !this.newPassword) {
      alert('يرجى تعبئة حقول كلمة المرور');
      return;
    }

    const success = this.authService.changeCustomerPassword(
      this.currentPassword,
      this.newPassword
    );

    if (success) {
      alert('تم تغيير كلمة المرور بنجاح 🌸');
      this.currentPassword = '';
      this.newPassword = '';
    } else {
      alert('كلمة المرور الحالية غير صحيحة أو الجديدة قصيرة');
    }
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}