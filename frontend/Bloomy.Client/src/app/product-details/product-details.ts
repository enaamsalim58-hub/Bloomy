import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule],
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

  constructor(private route: ActivatedRoute) {
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
    alert('تمت إضافة المنتج إلى السلة ❤️');
  }
}