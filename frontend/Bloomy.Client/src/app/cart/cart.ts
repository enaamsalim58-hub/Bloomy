import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';

import { CartService } from '../services/cart.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule
  ],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class Cart implements OnInit {

  cartItems: any[] = [];

  isSubmitting = false;

  // بيانات إتمام الطلب
  checkout = {
    receiverName: '',
    receiverPhone: '',
    deliveryAddress: '',
    subtotal: 0,
    deliveryFee: 20,
    discount: 0,
    total: 0
  };

  constructor(
    private cartService: CartService,
    private authService: AuthService,
    private router: Router
  ) {}


  // تحميل المنتجات من خدمة السلة
  ngOnInit(): void {

    this.cartService.items$.subscribe(items => {

      this.cartItems = items;

      this.updateTotals();

    });

  }


  // زيادة الكمية
  increase(item: any): void {

    this.cartService.updateQuantity(
      item.cartItemId,
      item.quantity + 1
    );

  }


  // تقليل الكمية
  decrease(item: any): void {

    if (item.quantity > 1) {

      this.cartService.updateQuantity(
        item.cartItemId,
        item.quantity - 1
      );

    }

  }


  // دوال متوافقة مع الاستخدام السابق
  up(item: any): void {
    this.increase(item);
  }

  down(item: any): void {
    this.decrease(item);
  }


  // حذف منتج من السلة
  removeItem(item: any): void {

    this.cartService.removeItem(
      item.cartItemId
    );

  }


  // حساب الإجمالي
  updateTotals(): void {

    this.checkout.subtotal = this.cartItems.reduce(
      (sum, item) =>
        sum + (item.unitPrice * item.quantity),
      0
    );

    this.checkout.total =
      this.checkout.subtotal +
      this.checkout.deliveryFee -
      this.checkout.discount;

  }


  // ======================================
  // تأكيد الطلب وحفظه
  // ======================================

  confirmOrder(): void {

    // منع تكرار تأكيد الطلب
    if (this.isSubmitting) {
      return;
    }

    // التأكد من تسجيل الدخول بحساب عميل
    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {

      alert('يرجى تسجيل الدخول بحساب عميل أولاً');

      this.router.navigate(['/login']);

      return;

    }

    // التأكد من تعبئة بيانات التوصيل
    if (
      !this.checkout.receiverName.trim() ||
      !this.checkout.receiverPhone.trim() ||
      !this.checkout.deliveryAddress.trim()
    ) {

      alert('من فضلك املئي بيانات التوصيل كاملة');

      return;

    }

    // التأكد من وجود منتجات
    if (this.cartItems.length === 0) {

      alert('السلة فارغة، أضيفي منتجات أولاً');

      return;

    }

    this.isSubmitting = true;

    // حفظ الطلب في CartService
    const savedOrder = this.cartService.createOrder({

      receiverName: this.checkout.receiverName,

      receiverPhone: this.checkout.receiverPhone,

      deliveryAddress: this.checkout.deliveryAddress,

      subtotal: this.checkout.subtotal,

      deliveryFee: this.checkout.deliveryFee,

      discount: this.checkout.discount,

      total: this.checkout.total,

      customerEmail: user.email

    });

    // التأكد من نجاح الحفظ
    if (!savedOrder) {

      this.isSubmitting = false;

      alert('تعذر حفظ الطلب. يرجى المحاولة مرة أخرى.');

      return;

    }

    // رسالة نجاح مع رقم الطلب
    alert(
      'تم تأكيد طلبك وحفظه بنجاح 🌸\n\n' +
      'رقم الطلب: #' + savedOrder.orderId +
      '\nالإجمالي: ' + savedOrder.total + ' ر.س' +
      '\nحالة الطلب: ' + savedOrder.status
    );

    // الانتقال إلى صفحة تتبع الطلبات
    this.router.navigate(['/my-orders']);

  }

}