import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products {
  products: any[] = [
    { productId: 1, name: 'باقة ورود مميزة', description: 'باقة أنيقة من أجمل الورود', price: 199, image: '/Images/f.jpeg', categoryName: 'باقات الورود' },
    { productId: 2, name: 'صندوق هدايا', description: 'هدية جميلة للمناسبات الخاصة', price: 149, image: '/Images/h.jpeg', categoryName: 'الهدايا' },
    { productId: 3, name: 'علبة شوكولاتة', description: 'شوكولاتة فاخرة بتغليف أنيق', price: 99, image: '/Images/k.jpeg', categoryName: 'الشوكولاتة' },
    { productId: 4, name: 'دمية لطيفة', description: 'هدية مميزة لمن تحبين', price: 119, image: '/Images/d.jpeg', categoryName: 'الدمى' }
  ];

  constructor(private cartService: CartService) {}

  addToCart(product: any) {
    
    console.log('ضغطت إضافة', this.products);
    this.cartService.addToCart(product);
    alert('تمت الإضافة للسلة 🌸');
  }
}