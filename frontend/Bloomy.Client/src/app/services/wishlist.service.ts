import { Injectable } from '@angular/core';

export interface WishlistItem {
  productId: number;
  productName: string;
  price: number;
  image: string;
}

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  // مفتاح مستقل للمفضلة الخاصة بكل عميل
  private getStorageKey(email: string): string {
    return `bloomy_wishlist_${email.trim().toLowerCase()}`;
  }

  // استرجاع المفضلة
  getWishlist(email: string): WishlistItem[] {
    try {
      const data = localStorage.getItem(this.getStorageKey(email));
      return data ? JSON.parse(data) : [];
    } catch (error) {
      console.error('تعذر استرجاع المفضلة:', error);
      return [];
    }
  }

  // التحقق من وجود المنتج في المفضلة
  isFavorite(email: string, productId: number): boolean {
    return this.getWishlist(email).some(
      item => item.productId === productId
    );
  }

  // إضافة المنتج أو حذفه من المفضلة
  toggleFavorite(email: string, product: WishlistItem): boolean {

    const items = this.getWishlist(email);

    const index = items.findIndex(
      item => item.productId === product.productId
    );

    if (index !== -1) {
      items.splice(index, 1);
      this.saveWishlist(email, items);
      return false;
    }

    items.push(product);
    this.saveWishlist(email, items);

    return true;
  }

  // حذف منتج من المفضلة
  removeFavorite(email: string, productId: number): void {

    const items = this.getWishlist(email).filter(
      item => item.productId !== productId
    );

    this.saveWishlist(email, items);
  }

  // حفظ المفضلة
  private saveWishlist(email: string, items: WishlistItem[]): void {

    localStorage.setItem(
      this.getStorageKey(email),
      JSON.stringify(items)
    );

  }
}