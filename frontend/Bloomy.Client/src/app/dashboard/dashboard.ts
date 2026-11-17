import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  // الإحصائيات الرئيسية
  stats = {
    totalSales: 12580,
    totalOrders: 86,
    totalCustomers: 248,
    totalProducts: 56
  };

  // المبيعات الشهرية
  monthlySales = [
    { month: 'يناير', amount: 2200, percent: 30 },
    { month: 'فبراير', amount: 3100, percent: 42 },
    { month: 'مارس', amount: 2800, percent: 38 },
    { month: 'أبريل', amount: 4900, percent: 65 },
    { month: 'مايو', amount: 6800, percent: 90 }
  ];

  // حالات الطلبات
  orderStatusSummary = [
    { label: 'قيد التوصيل', count: 20, color: '#f59e0b' },
    { label: 'تم التسليم', count: 32, color: '#10b981' },
    { label: 'قيد التجهيز', count: 18, color: '#994b94' },
    { label: 'جديد', count: 12, color: '#3b82f6' },
    { label: 'ملغي', count: 4, color: '#ef4444' }
  ];

  // أحدث الطلبات
  recentOrders = [
    {
      orderId: 1054,
      receiverName: 'سارة أحمد',
      orderDate: '2025-05-20',
      status: 'قيد التوصيل',
      total: 309
    },
    {
      orderId: 1053,
      receiverName: 'منى سالم',
      orderDate: '2025-05-19',
      status: 'تم التسليم',
      total: 199
    },
    {
      orderId: 1052,
      receiverName: 'أمل خالد',
      orderDate: '2025-05-18',
      status: 'قيد التجهيز',
      total: 250
    },
    {
      orderId: 1051,
      receiverName: 'نورة علي',
      orderDate: '2025-05-17',
      status: 'جديد',
      total: 145
    }
  ];

}