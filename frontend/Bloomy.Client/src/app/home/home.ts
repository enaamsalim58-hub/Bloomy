import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  Router,
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { AuthService } from '../auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  categories = [
    { name: 'الهدايا', image: '/Images/h.jpeg' },
    { name: 'الشوكولاتة', image: '/Images/k.jpeg' },
    { name: 'الدمى', image: '/Images/d.jpeg' },
    { name: 'باقات الورود', image: '/Images/f.jpeg' }
  ];

  // التحقق من تسجيل الدخول
  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  // التحقق من المدير
  get isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  // التحقق من العميل
  get isCustomer(): boolean {
    return this.authService.isCustomer();
  }

  // تسجيل الخروج والعودة إلى واجهة الزائر
  logout(): void {
    this.authService.logout();

    this.router.navigateByUrl('/home');
  }

}