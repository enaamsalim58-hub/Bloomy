// import { Service } from '@angular/core';

// @Service()
// export class Cart {}


import { Injectable } from '@angular/core';

export interface CartProduct {
  productId: number;
  name: string;
  description: string;
  price: number;
  image: string;
  categoryName: string;
}

export interface CartItem {
  product: CartProduct;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {

  items: CartItem[] = [];

  addToCart(product: CartProduct): void {
    const existingItem = this.items.find(
      item => item.product.productId === product.productId
    );

    if (existingItem) {
      existingItem.quantity++;
    } else {
      this.items.push({
        product: product,
        quantity: 1
      });
    }
  }

  increase(item: CartItem): void {
    item.quantity++;
  }

  decrease(item: CartItem): void {
    if (item.quantity > 1) {
      item.quantity--;
    }
  }

  removeItem(productId: number): void {
    this.items = this.items.filter(
      item => item.product.productId !== productId
    );
  }

  clearCart(): void {
    this.items = [];
  }

  get itemsCount(): number {
    return this.items.reduce(
      (total, item) => total + item.quantity, 0
    );
  }

  get subtotal(): number {
    return this.items.reduce(
      (total, item) => total + item.product.price * item.quantity, 0
    );
  }

  get deliveryFee(): number {
    return this.items.length > 0 ? 500 : 0;
  }

  get total(): number {
    return this.subtotal + this.deliveryFee;
  }
}
