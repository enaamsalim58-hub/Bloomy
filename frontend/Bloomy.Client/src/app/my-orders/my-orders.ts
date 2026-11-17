import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Order {
  orderId: number;
  orderDate: string;
  status: string;
  total: number;
  productImage: string;
}

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-orders.html',
  styleUrl: './my-orders.css',
})
export class MyOrders {

  // التبويب النشط في القائمة الجانبية
  activeTab: 'orders' | 'wishlist' | 'addresses' | 'info' = 'orders';

  // فلترة الطلبات
  orderFilter: 'all' | 'processing' | 'delivered' = 'all';

  // بيانات العميل
  user = {
    userId: 101,
    fullName: 'سارة أحمد',
    email: 'sara.ahmed@example.com',
    phone: '777123456'
  };

  // الطلبات التجريبية
  orders: Order[] = [
    {
      orderId: 1054,
      orderDate: '2025-05-20',
      status: 'قيد التوصيل',
      total: 309,
      productImage: '/Images/j.jpeg'
    },
    {
      orderId: 1053,
      orderDate: '2025-05-15',
      status: 'تم التسليم',
      total: 199,
      productImage: '/Images/h.jpeg'
    },
    {
      orderId: 1052,
      orderDate: '2025-05-10',
      status: 'قيد التجهيز',
      total: 250,
      productImage: '/Images/f.jpeg'
    }
  ];

  // الطلب المحدد لتتبع حالته
  selectedOrder: Order = this.orders[0];

  // المنتجات المفضلة
  wishlist = [
    {
      productId: 1,
      productName: 'باقة لافندر الملكية',
      price: 199
    },
    {
      productId: 2,
      productName: 'علبة شوكولاتة فاخرة',
      price: 90
    }
  ];

  // عناوين العميل
  addresses = [
    {
      addressId: 1,
      title: 'المنزل',
      addressLine: 'المكلا - فوه - حي المساكن'
    },
    {
      addressId: 2,
      title: 'العمل',
      addressLine: 'المكلا - الديس - شارع الستين'
    }
  ];

  // فلترة الطلبات حسب الحالة
  get filteredOrders(): Order[] {

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

  // اختيار طلب لعرض تفاصيل تتبعه
  selectOrder(order: Order): void {
    this.selectedOrder = order;
  }

  // حذف منتج من المفضلة
  removeFromWishlist(productId: number): void {
    this.wishlist = this.wishlist.filter(
      item => item.productId !== productId
    );
  }

  // تسجيل الخروج
  logout(): void {
    window.alert('سيتم ربط تسجيل الخروج بنظام المستخدمين لاحقًا.');
  }

}