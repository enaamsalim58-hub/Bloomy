import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface CartProduct {
  cartItemId: number;
  productId: number;
  productName: string;
  imageUrl: string;
  sizeName: string;
  colorName: string;
  unitPrice: number;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  private itemsSubject = new BehaviorSubject<CartProduct[]>([
    { cartItemId: 1, productId: 1, productName: 'باقة لافندر الملكية', imageUrl: 'https://images.unsplash.com/photo-1561181286-d3fee7d55364?w=200', sizeName: 'كبير', colorName: 'بنفسجي', unitPrice: 199, quantity: 1 },
    { cartItemId: 2, productId: 2, productName: 'علبة شوكولاتة فاخرة', imageUrl: 'https://images.unsplash.com/photo-1549007994-cb92caebd54b?w=200', sizeName: 'متوسط', colorName: 'أحمر', unitPrice: 90, quantity: 1 }
  ]);
  items$ = this.itemsSubject.asObservable();

  addToCart(product: any) {
    const items = [...this.itemsSubject.value];
    const qty = product.quantity || 1;
    const existing = items.find(x => x.productId === product.productId);
    if (existing) {
      existing.quantity += qty;
    } else {
      items.push({
        cartItemId: Date.now(),
        productId: product.productId,
        productName: product.name || product.productName || 'منتج',
        imageUrl: product.image || product.imageUrl || '/Images/fl.png',
        sizeName: product.sizeName || 'متوسط',
        colorName: product.colorName || 'افتراضي',
        unitPrice: product.price || product.unitPrice || 0,
        quantity: qty
      });
    }
    this.itemsSubject.next(items);
  }

  updateQuantity(cartItemId: number, quantity: number) {
    const items = this.itemsSubject.value.map(x =>
      x.cartItemId === cartItemId ? { ...x, quantity } : x
    );
    this.itemsSubject.next(items);
  }

  removeItem(cartItemId: number) {
    const items = this.itemsSubject.value.filter(x => x.cartItemId !== cartItemId);
    this.itemsSubject.next(items);
  }
}
