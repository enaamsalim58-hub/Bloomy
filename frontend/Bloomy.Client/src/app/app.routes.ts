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

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'products', component: Products },
  { path: 'product/:id', component: ProductDetails },
  { path: 'cart', component: Cart },
  { path: 'manage-products', component: ManageProducts },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  { path: 'my-orders', component: MyOrders },
  { path: 'dashboard', component: Dashboard },
  { path: '**', redirectTo: '' },
];