// import { Routes } from '@angular/router';
// import { Home } from './home/home';
// import { Contact } from './contact/contact';
// import { About } from './about/about';
// import { ProductDetails } from './product-details/product-details';
// import { Products } from './products/products';

// export const routes: Routes = [
//   { path: '', component: Home },
//   { path: 'products', component: Products },
//   { path: 'product/:id', component: ProductDetails },
//   { path: 'about', component: About },
//   { path: 'contact', component: Contact },
//   { path: '**', redirectTo: '' }
// ];

import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Contact } from './contact/contact';
import { About } from './about/about';
import { ProductDetails } from './product-details/product-details';
import { Cart } from './cart/cart';
import { Products } from './products/products';
import { ManageProducts } from './manage-products/manage-products';
import { Login } from './login/login';
import { adminGuard } from './services/admin.guard';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'products', component: Products },
  { path: 'product/:id', component: ProductDetails },
  { path: 'cart', component: Cart },
  { path: 'manage-products', component: ManageProducts },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: 'login', component: Login },
  { path: 'manage-products', component: ManageProducts, canActivate: [adminGuard] },
  { path: '**', redirectTo: '' },
  
];