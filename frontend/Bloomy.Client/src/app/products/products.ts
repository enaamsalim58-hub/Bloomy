// import { Component } from '@angular/core';

// @Component({
//   imports: [],
//   selector: 'app-products',
//   styleUrl: './products.css',
//   templateUrl: './products.html',
// })
// export class Products {}

import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
// import { CartService, CartProduct } from '../services/cart.service';
import { CartService, CartProduct } from '../services/cart';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [RouterLink],
  styleUrl: './products.css',
  templateUrl: './products.html',
})
export class Products {

  constructor(public cartService: CartService) {}

  products: CartProduct[] = [
    {
      productId: 1,
      name: 'باقة ورود مميزة',
      description: 'باقة أنيقة من أجمل الورود',
      price: 199,
      image: '/Images/f.jpeg',
      categoryName: 'باقات الورود'
    },
    {
      productId: 2,
      name: 'صندوق هدايا',
      description: 'هدية جميلة للمناسبات الخاصة',
      price: 149,
      image: '/Images/h.jpeg',
      categoryName: 'الهدايا'
    },
    {
      productId: 3,
      name: 'علبة شوكولاتة',
      description: 'شوكولاتة فاخرة بتغليف أنيق',
      price: 99,
      image: '/Images/k.jpeg',
      categoryName: 'الشوكولاتة'
    },
    {
      productId: 4,
      name: 'دمية لطيفة',
      description: 'هدية مميزة لمن تحبين',
      price: 119,
      image: '/Images/d.jpeg',
      categoryName: 'الدمى'
    }
  ];

  addToCart(product: CartProduct): void {
    this.cartService.addToCart(product);
    alert(`تمت إضافة ${product.name} إلى السلة بنجاح 🌸`);
  }
}