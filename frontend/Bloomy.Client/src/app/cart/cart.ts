import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class Cart implements OnInit {
  cartItems: any[] = [];

  checkout = {
    receiverName: '',
    receiverPhone: '',
    deliveryAddress: '',
    subtotal: 0,
    deliveryFee: 20,
    discount: 0,
    total: 0
  };

  constructor(private cartService: CartService) {}

  ngOnInit() {
    this.cartService.items$.subscribe(items => {
      this.cartItems = items;
      this.updateTotals();
    });
  }

  increase(item: any) {
    this.cartService.updateQuantity(item.cartItemId, item.quantity + 1);
  }

  decrease(item: any) {
    if (item.quantity > 1) {
      this.cartService.updateQuantity(item.cartItemId, item.quantity - 1);
    }
  }

  up(item: any) { this.increase(item); }
  down(item: any) { this.decrease(item); }

  removeItem(item: any) {
    this.cartService.removeItem(item.cartItemId);
  }

  updateTotals() {
    this.checkout.subtotal = this.cartItems.reduce((sum, x) => sum + (x.unitPrice * x.quantity), 0);
    this.checkout.total = this.checkout.subtotal + this.checkout.deliveryFee - this.checkout.discount;
  }

  confirmOrder() {
    if (!this.checkout.receiverName || !this.checkout.receiverPhone || !this.checkout.deliveryAddress) {
      alert('من فضلك املئي بيانات التوصيل كاملة');
      return;
    }
    alert('تم تأكيد طلبك بنجاح! الإجمالي: ' + this.checkout.total + ' ر.س');
  }
}