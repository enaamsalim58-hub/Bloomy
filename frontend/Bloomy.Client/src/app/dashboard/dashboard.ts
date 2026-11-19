import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { ManageProducts } from '../manage-products/manage-products';

import {
  RouterModule,
  ActivatedRoute,
  Router
} from '@angular/router';

type DashboardView =
  | 'dashboard'
  | 'products'
  | 'orders'
  | 'customers'
  | 'reports'
  | 'settings';

interface ManagerOrder {
  orderId: number;
  receiverName: string;
  orderDate: string;
  status: string;
  total: number;
}

interface ManagerCustomer {
  customerId: number;
  name: string;
  email: string;
  phone: string;
  totalOrders: number;
  totalSpent: number;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    ManageProducts
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  // العرض الحالي داخل لوحة الإدارة
  activeView: DashboardView = 'dashboard';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  // تحديد العرض عند فتح لوحة الإدارة
  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {

      const view = params.get('view');

      if (
        view === 'products' ||
        view === 'orders' ||
        view === 'customers' ||
        view === 'reports' ||
        view === 'settings'
      ) {
        this.activeView = view;
      } else {
        this.activeView = 'dashboard';
      }

    });
  }

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

  // ألوان حالات الطلبات
  private statusColors: { [key: string]: string } = {
    'قيد التوصيل': '#f59e0b',
    'تم التسليم': '#10b981',
    'قيد التجهيز': '#994b94',
    'جديد': '#3b82f6',
    'ملغي': '#ef4444'
  };

  // ملخص حالات الطلبات
  orderStatusSummary = [
    { label: 'قيد التوصيل', count: 20, color: '#f59e0b' },
    { label: 'تم التسليم', count: 32, color: '#10b981' },
    { label: 'قيد التجهيز', count: 18, color: '#994b94' },
    { label: 'جديد', count: 12, color: '#3b82f6' },
    { label: 'ملغي', count: 4, color: '#ef4444' }
  ];

  // الطلبات التجريبية
  managerOrders: ManagerOrder[] = [
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

  // بيانات العملاء التجريبية
  managerCustomers: ManagerCustomer[] = [
    {
      customerId: 1,
      name: 'سارة أحمد',
      email: 'sara@example.com',
      phone: '0501234567',
      totalOrders: 5,
      totalSpent: 1250
    },
    {
      customerId: 2,
      name: 'منى سالم',
      email: 'mona@example.com',
      phone: '0502345678',
      totalOrders: 3,
      totalSpent: 780
    },
    {
      customerId: 3,
      name: 'أمل خالد',
      email: 'amal@example.com',
      phone: '0503456789',
      totalOrders: 4,
      totalSpent: 960
    },
    {
      customerId: 4,
      name: 'نورة علي',
      email: 'noura@example.com',
      phone: '0504567890',
      totalOrders: 2,
      totalSpent: 420
    }
  ];

  // بيانات الإعدادات التجريبية
  settingsForm = {
    adminName: 'مدير النظام',
    storeName: 'Bloomy',
    storeEmail: 'admin@example.com',
    storePhone: '0500000000',
    orderNotifications: true,
    emailNotifications: true,
    promotionalNotifications: false
  };

  settingsMessage = '';

  // فتح لوحة التحكم
  showDashboard(): void {
    this.router.navigate(['/dashboard']);
  }

  // فتح إدارة المنتجات
  showProducts(): void {
    this.router.navigate(['/dashboard'], {
      queryParams: { view: 'products' }
    });
  }

  // فتح إدارة الطلبات
  showOrders(): void {
    this.router.navigate(['/dashboard'], {
      queryParams: { view: 'orders' }
    });
  }

  // فتح إدارة العملاء
  showCustomers(): void {
    this.router.navigate(['/dashboard'], {
      queryParams: { view: 'customers' }
    });
  }

  // فتح التقارير
  showReports(): void {
    this.router.navigate(['/dashboard'], {
      queryParams: { view: 'reports' }
    });
  }

  // فتح الإعدادات
  showSettings(): void {
    this.router.navigate(['/dashboard'], {
      queryParams: { view: 'settings' }
    });
  }

  // حفظ الإعدادات التجريبية
  saveSettings(): void {
    this.settingsMessage =
      'تم حفظ التغييرات في الواجهة التجريبية بنجاح.';
  }

  // تغيير حالة الطلب
  changeOrderStatus(
    orderId: number,
    newStatus: string
  ): void {

    const order = this.managerOrders.find(
      item => item.orderId === orderId
    );

    if (!order || order.status === newStatus) {
      return;
    }

    const oldItem = this.orderStatusSummary.find(
      item => item.label === order.status
    );

    const newItem = this.orderStatusSummary.find(
      item => item.label === newStatus
    );

    if (!oldItem || !newItem || oldItem.count <= 0) {
      return;
    }

    oldItem.count++;
    oldItem.count--;

    newItem.count++;

    order.status = newStatus;
  }

  // الحصول على لون حالة الطلب
  getStatusColor(status: string): string {
    return this.statusColors[status] || '#994b94';
  }

  // حساب نسبة كل حالة
  getStatusPercentage(count: number): number {

    if (this.stats.totalOrders === 0) {
      return 0;
    }

    return (count / this.stats.totalOrders) * 100;
  }

  // إنشاء المخطط الدائري تلقائيًا
  get donutGradient(): string {

    let currentPercentage = 0;

    const parts = this.orderStatusSummary.map(item => {

      const percentage =
        this.getStatusPercentage(item.count);

      const start = currentPercentage;

      currentPercentage += percentage;

      return `${item.color} ${start}% ${currentPercentage}%`;
    });

    return `conic-gradient(${parts.join(', ')})`;
  }

  // إجمالي مبيعات الطلبات التجريبية المعروضة
  get totalReportedSales(): number {
    return this.managerOrders.reduce(
      (sum, order) => sum + order.total,
      0
    );
  }

  // عدد الطلبات المسلّمة
  get deliveredOrdersCount(): number {
    return this.orderStatusSummary.find(
      item => item.label === 'تم التسليم'
    )?.count || 0;
  }

  // الطلبات الجديدة وقيد التجهيز
  get pendingOrdersCount(): number {
    return this.orderStatusSummary
      .filter(item =>
        item.label === 'جديد' ||
        item.label === 'قيد التجهيز'
      )
      .reduce((sum, item) => sum + item.count, 0);
  }

}