// // import { Component } from '@angular/core';

// // @Component({
// //   imports: [],
// //   selector: 'app-cart',
// //   styleUrl: './cart.css',
// //   templateUrl: './cart.html',
// // })
// // export class Cart {}

// import { Component } from '@angular/core';
// import { CommonModule } from '@angular/common';

// interface CartItem {
//   id: number;
//   name: string;
//   description: string;
//   category: string;
//   price: number;
//   quantity: number;
//   image: string;
// }

// @Component({
//   selector: 'app-cart',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './cart.html',
//   styleUrl: './cart.css'
// })
// export class Cart {

//   cartItems: CartItem[] = [
//     {
//       id: 1,
//       name: 'Pink Rose Bouquet',
//       description: 'Beautiful pink roses bouquet',
//       category: 'Flowers',
//       price: 25,
//       quantity: 1,
//       image: 'https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=400'
//     },
//     {
//       id: 2,
//       name: 'Chocolate Gift Box',
//       description: 'Elegant chocolate gift box',
//       category: 'Chocolates',
//       price: 18,
//       quantity: 2,
//       image: 'https://images.unsplash.com/photo-1548907040-4d42bfc2f2c5?w=400'
//     },
//     {
//       id: 3,
//       name: 'Cute Teddy Bear',
//       description: 'Soft and lovely teddy bear',
//       category: 'Dolls',
//       price: 15,
//       quantity: 1,
//       image: 'https://images.unsplash.com/photo-1559454403-b8fb88521f11?w=400'
//     }
//   ];

//   deliveryFee = 5;

//   get subtotal(): number {
//     return this.cartItems.reduce(
//       (total, item) => total + item.price * item.quantity, 0
//     );
//   }

//   get total(): number {
//     return this.subtotal + (this.cartItems.length > 0 ? this.deliveryFee : 0);
//   }

//   increase(item: CartItem): void {
//     item.quantity++;
//   }

//   decrease(item: CartItem): void {
//     if (item.quantity > 1) {
//       item.quantity--;
//     }
//   }

//   removeItem(id: number): void {
//     this.cartItems = this.cartItems.filter(item => item.id !== id);
//   }

//   clearCart(): void {
//     this.cartItems = [];
//   }

//   checkout(): void {
//     alert('Thank you for shopping with Bloomy!');
//   }
// }


// // import { Component } from '@angular/core';
// // import { RouterLink } from '@angular/router';
// import { Component } from '@angular/core';
// import { RouterLink } from '@angular/router';
// import { DecimalPipe } from '@angular/common';
// @Component({
//   selector: 'app-cart',
//   standalone: true,
//   // imports: [RouterLink],
//   imports: [RouterLink, DecimalPipe],
//   templateUrl: './cart.html',
//   styleUrl: './cart.css'
// })
// export class Cart {

//   cartItems = [
//     {
//       id: 1,
//       name: 'باقة الورد الوردي',
//       description: 'باقة ورد طبيعية بتغليف أنيق',
//       category: 'ورود',
//       price: 2500,
//       quantity: 1,
//       image: '/Images/B16.jpeg'
//     },
//     {
//       id: 2,
//       name: 'علبة شوكولاتة فاخرة',
//       description: 'شوكولاتة مميزة للمناسبات',
//       category: 'شوكولاتة',
//       price: 1800,
//       quantity: 2,
//       image: '/Images/B17.jpeg'
//     },
//     {
//       id: 3,
//       name: 'هدية دمية لطيفة',
//       description: 'دمية ناعمة بتصميم جميل',
//       category: 'دمى',
//       price: 3000,
//       quantity: 1,
//       image: '/Images/B18.jpeg'
//     }
//   ];

//   deliveryFee = 500;

//   get subtotal(): number {
//     return this.cartItems.reduce(
//       (total, item) => total + item.price * item.quantity,
//       0
//     );
//   }

//   get total(): number {
//     return this.subtotal + (this.cartItems.length > 0 ? this.deliveryFee : 0);
//   }

//   get itemsCount(): number {
//     return this.cartItems.reduce(
//       (total, item) => total + item.quantity,
//       0
//     );
//   }

//   increase(item: typeof this.cartItems[number]): void {
//     item.quantity++;
//   }

//   decrease(item: typeof this.cartItems[number]): void {
//     if (item.quantity > 1) {
//       item.quantity--;
//     }
//   }

//   removeItem(id: number): void {
//     this.cartItems = this.cartItems.filter(item => item.id !== id);
//   }

//   clearCart(): void {
//     this.cartItems = [];
//   }
// }


// import { Component } from '@angular/core';
// import { RouterLink } from '@angular/router';
// import { DecimalPipe } from '@angular/common';

// @Component({
//   selector: 'app-cart',
//   standalone: true,
//   imports: [RouterLink, DecimalPipe],
//   templateUrl: './cart.html',
//   styleUrl: './cart.css'
// })
// export class Cart {
//   cartItems = [
//     {
//       id: 1,
//       name: 'باقة الورد الوردي',
//       description: 'باقة ورد طبيعية بتغليف أنيق',
//       category: 'ورود',
//       price: 2500,
//       quantity: 1,
//       image: '/Images/B16.jpeg'
//     },
//     {
//       id: 2,
//       name: 'علبة شوكولاتة فاخرة',
//       description: 'شوكولاتة مميزة للمناسبات',
//       category: 'شوكولاتة',
//       price: 1800,
//       quantity: 2,
//       image: '/Images/B17.jpeg'
//     },
//     {
//       id: 3,
//       name: 'هدية دمية لطيفة',
//       description: 'هدية جميلة للمناسبات',
//       category: 'دمى',
//       price: 3000,
//       quantity: 1,
//       image: '/Images/B18.jpeg'
//     }
//   ];

//   deliveryFee = 500;

//   get subtotal(): number {
//     return this.cartItems.reduce(
//       (sum, item) => sum + item.price * item.quantity, 0
//     );
//   }

//   get total(): number {
//     return this.subtotal + (this.cartItems.length ? this.deliveryFee : 0);
//   }

//   get itemsCount(): number {
//     return this.cartItems.reduce(
//       (sum, item) => sum + item.quantity, 0
//     );
//   }

//   increase(item: typeof this.cartItems[number]) {
//     item.quantity++;
//   }

//   decrease(item: typeof this.cartItems[number]) {
//     if (item.quantity > 1) item.quantity--;
//   }

//   removeItem(id: number) {
//     this.cartItems = this.cartItems.filter(item => item.id !== id);
//   }

//   clearCart() {
//     this.cartItems = [];
//   }
// }



import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { CartService, CartItem } from '../services/cart';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class Cart {

  constructor(public cartService: CartService) {}

  get cartItems(): CartItem[] {
    return this.cartService.items;
  }

  get itemsCount(): number {
    return this.cartService.itemsCount;
  }

  get subtotal(): number {
    return this.cartService.subtotal;
  }

  get deliveryFee(): number {
    return this.cartService.deliveryFee;
  }

  get total(): number {
    return this.cartService.total;
  }

  increase(item: CartItem): void {
    this.cartService.increase(item);
  }

  decrease(item: CartItem): void {
    this.cartService.decrease(item);
  }

  removeItem(productId: number): void {
    this.cartService.removeItem(productId);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }
}
