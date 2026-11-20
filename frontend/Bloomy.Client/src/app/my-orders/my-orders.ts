import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../auth.service';
import {
  CartService,
  StoredOrder
} from '../services/cart.service';

import {
  WishlistService,
  WishlistItem
} from '../services/wishlist.service';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './my-orders.html',
  styleUrl: './my-orders.css',
})
export class MyOrders implements OnInit {

  // التبويب النشط
  activeTab: 'orders' | 'wishlist' | 'info' = 'orders';

  // فلترة الطلبات
  orderFilter: 'all' | 'processing' | 'delivered' = 'all';

  // بيانات العميل
  user = {
    userId: 101,
    fullName: '',
    email: '',
    phone: ''
  };

  // الطلبات المحفوظة
  orders: StoredOrder[] = [];

  // الطلب المحدد
  selectedOrder: StoredOrder | null = null;

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private wishlistService: WishlistService,
    private router: Router
  ) {

    const currentUser = this.authService.currentUser;

    // التحقق من تسجيل الدخول كعميل
    if (!currentUser || currentUser.role !== 'customer') {

      this.router.navigateByUrl('/home');

      return;

    }

    // بيانات العميل الحالي
    this.user = {
      userId: 101,
      fullName: currentUser.fullName,
      email: currentUser.email,
      phone: currentUser.phone || ''
    };

  }

  // ==================================
  // تحميل البيانات
  // ==================================

  ngOnInit(): void {

    const customerEmail = this.user.email
      .trim()
      .toLowerCase();

    // استرجاع طلبات العميل
    this.orders = this.cartService.getOrdersForCustomer(
      customerEmail
    );

    // تحديد أول طلب
    this.selectedOrder = this.orders.length > 0
      ? this.orders[0]
      : null;

  }

  // ==================================
  // المفضلة الحقيقية
  // ==================================

  get wishlist(): WishlistItem[] {

    return this.wishlistService.getWishlist(
      this.user.email
    );

  }

  // ==================================
  // فلترة الطلبات
  // ==================================

  get filteredOrders(): StoredOrder[] {

    if (this.orderFilter === 'processing') {

      return this.orders.filter(order =>
        order.status === 'قيد التجهيز' ||
        order.status === 'قيد التوصيل'
      );

    }

    if (this.orderFilter === 'delivered') {

      return this.orders.filter(order =>
        order.status === 'تم التسليم'
      );

    }

    return this.orders;

  }

  // ==================================
  // اختيار طلب للتتبع
  // ==================================

  selectOrder(order: StoredOrder): void {
    this.selectedOrder = order;
  }

  // ==================================
  // حذف منتج من المفضلة
  // ==================================

  removeFromWishlist(productId: number): void {

    this.wishlistService.removeFavorite(
      this.user.email,
      productId
    );

  }

  // ==================================
  // تسجيل الخروج
  // ==================================

  logout(): void {

    this.authService.logout();

    this.router.navigateByUrl('/home');

  }

}