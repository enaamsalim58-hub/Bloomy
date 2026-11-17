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
import { Products } from './products/products';
import { Cart } from './cart/cart';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'products', component: Products },
  { path: 'product/:id', component: ProductDetails },
  { path: 'cart', component: Cart },
  { path: 'about', component: About },
  { path: 'contact', component: Contact },
  { path: '**', redirectTo: '' }
];