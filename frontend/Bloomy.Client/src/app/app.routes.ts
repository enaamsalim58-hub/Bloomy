
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
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: Login },
  { path: 'home', component: Home },

  { path: 'products', component: Products },
  { path: 'product/:id', component: ProductDetails },
  { path: 'cart', component: Cart },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },

  { path: 'my-orders', component: MyOrders },

  { path: 'dashboard', component: Dashboard },
  { path: 'admin-profile', component: AdminProfile },
  { path: 'manage-products', component: ManageProducts },

  { path: 'customer-profile', component: CustomerProfile },

  { path: '**', redirectTo: 'login' }
];
