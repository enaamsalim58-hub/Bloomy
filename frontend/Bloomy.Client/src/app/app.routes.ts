
import { Routes } from '@angular/router';

import { Home } from './home/home';
import { Contact } from './contact/contact';
import { About } from './about/about';
import { ProductDetails } from './product-details/product-details';
import { Cart } from './cart/cart';
import { Products } from './products/products';
import { ManageProducts } from './manage-products/manage-products';
import { Login } from './login/login';
import { MyOrders } from './my-orders/my-orders';
import { Dashboard } from './dashboard/dashboard';
import { AdminProfile } from './admin-profile/admin-profile';
import { CustomerProfile } from './customer-profile/customer-profile';

export const routes: Routes = [

  // الصفحة الرئيسية هي أول صفحة تظهر للزائر
  { path: '', redirectTo: 'home', pathMatch: 'full' },

  // الصفحات الأساسية
  { path: 'home', component: Home },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },

  // تسجيل الدخول وإنشاء الحساب
  { path: 'login', component: Login },

  // صفحات المنتجات
  { path: 'products', component: Products },
  { path: 'product/:id', component: ProductDetails },

  // سلة المشتريات
  { path: 'cart', component: Cart },

  // صفحات العميل
  { path: 'my-orders', component: MyOrders },
  { path: 'customer-profile', component: CustomerProfile },

  // صفحات المدير
  { path: 'dashboard', component: Dashboard },
  { path: 'admin-profile', component: AdminProfile },
  { path: 'manage-products', component: ManageProducts },

  // أي رابط غير معروف يرجع للرئيسية
  { path: '**', redirectTo: 'home' }

];
