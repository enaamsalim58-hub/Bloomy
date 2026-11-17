import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
})
export class ProductDetails {

  productId: string | null = null;

  quantity = 1;
  selectedSize = 'متوسط';
  selectedColor = 'lavender';
  isFavorite = false;

  images = [
    '/Images/fl.png',
    '/Images/fl.png',
    '/Images/fl.png'
  ];

  selectedImage = this.images[0];

  // بيانات المنتج - عشان نرسلها للسلة
  productNames: any = {
    '1': { name: 'باقة ورود مميزة', price: 199 },
    '2': { name: 'صندوق هدايا', price: 149 },
    '3': { name: 'علبة شوكولاتة', price: 99 },
    '4': { name: 'دمية لطيفة', price: 119 }
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService
  ) {
    this.productId = this.route.snapshot.paramMap.get('id');
  }

  selectImage(image: string) {
    this.selectedImage = image;
  }

  selectSize(size: string) {
    this.selectedSize = size;
  }

  selectColor(color: string) {
    this.selectedColor = color;
  }

  increaseQuantity() {
    this.quantity++;
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  toggleFavorite() {
    this.isFavorite = !this.isFavorite;
  }

  addToCart() {
    const id = Number(this.productId) || 1;
    const info = this.productNames[String(id)] || { name: 'باقة ورود مميزة', price: 199 };

    // هنا نرسل المنتج للخدمة فعلاً، مو بس alert
    this.cartService.addToCart({
      productId: id,
      name: info.name,
      price: info.price,
      image: this.selectedImage,
      quantity: this.quantity,
      sizeName: this.selectedSize,
      colorName: this.selectedColor
    });

    alert('تمت إضافة المنتج إلى السلة ❤️');
    this.router.navigate(['/cart']);
  }
}