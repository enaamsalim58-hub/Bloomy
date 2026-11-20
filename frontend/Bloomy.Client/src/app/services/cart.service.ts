import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


// ======================================
// بيانات المنتج الموجود في السلة
// ======================================

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


// ======================================
// بيانات تأكيد الطلب
// ======================================

export interface CheckoutData {
  receiverName: string;
  receiverPhone: string;
  deliveryAddress: string;
  subtotal?: number;
  deliveryFee?: number;
  discount?: number;
  total?: number;
  customerEmail?: string;
}


// ======================================
// بيانات الطلب المحفوظ
// ======================================

export interface StoredOrder {
  orderId: number;
  orderDate: string;
  status: string;

  receiverName: string;
  receiverPhone: string;
  deliveryAddress: string;
  customerEmail: string;

  subtotal: number;
  deliveryFee: number;
  discount: number;
  total: number;

  productImage: string;

  items: CartProduct[];
}


// ======================================
// Cart Service
// ======================================

@Injectable({
  providedIn: 'root'
})
export class CartService {

  // أسماء التخزين في المتصفح
  private readonly CART_STORAGE_KEY = 'bloomy_cart_items';
  private readonly ORDERS_STORAGE_KEY = 'bloomy_orders';


  // ======================================
  // استرجاع البيانات المحفوظة
  // ======================================

  private loadCart(): CartProduct[] {

    try {

      const savedCart = localStorage.getItem(
        this.CART_STORAGE_KEY
      );

      return savedCart ? JSON.parse(savedCart) : [];

    } catch (error) {

      console.error('تعذر استرجاع السلة:', error);

      return [];

    }

  }


  private loadOrders(): StoredOrder[] {

    try {

      const savedOrders = localStorage.getItem(
        this.ORDERS_STORAGE_KEY
      );

      return savedOrders ? JSON.parse(savedOrders) : [];

    } catch (error) {

      console.error('تعذر استرجاع الطلبات:', error);

      return [];

    }

  }


  // ======================================
  // قائمة المنتجات في السلة
  // ======================================

  private itemsSubject = new BehaviorSubject<CartProduct[]>(
    this.loadCart()
  );

  items$ = this.itemsSubject.asObservable();


  // ======================================
  // قائمة الطلبات المؤكدة
  // ======================================

  private ordersSubject = new BehaviorSubject<StoredOrder[]>(
    this.loadOrders()
  );

  orders$ = this.ordersSubject.asObservable();


  // ======================================
  // حفظ السلة
  // ======================================

  private saveCart(): void {

    try {

      localStorage.setItem(
        this.CART_STORAGE_KEY,
        JSON.stringify(this.itemsSubject.value)
      );

    } catch (error) {

      console.error('تعذر حفظ السلة:', error);

    }

  }


  // ======================================
  // حفظ الطلبات
  // ======================================

  private saveOrders(): void {

    try {

      localStorage.setItem(
        this.ORDERS_STORAGE_KEY,
        JSON.stringify(this.ordersSubject.value)
      );

    } catch (error) {

      console.error('تعذر حفظ الطلبات:', error);

    }

  }


  // ======================================
  // إضافة منتج إلى السلة
  // ======================================

  addToCart(product: any): void {

    const items = [...this.itemsSubject.value];

    const quantity = Math.max(1, Number(product.quantity) || 1);

    const existing = items.find(
      item => item.productId === product.productId
    );

    if (existing) {

      existing.quantity += quantity;

    } else {

      items.push({

        cartItemId: Date.now(),

        productId: product.productId,

        productName:
          product.name ||
          product.productName ||
          'منتج',

        imageUrl:
          product.image ||
          product.imageUrl ||
          '/Images/fl.png',

        sizeName: product.sizeName || 'متوسط',

        colorName: product.colorName || 'افتراضي',

        unitPrice:
          Number(product.price || product.unitPrice) || 0,

        quantity: quantity

      });

    }

    this.itemsSubject.next(items);

    this.saveCart();

  }


  // ======================================
  // تعديل كمية المنتج
  // ======================================

  updateQuantity(cartItemId: number, quantity: number): void {

    if (quantity < 1) return;

    const items = this.itemsSubject.value.map(item =>

      item.cartItemId === cartItemId
        ? { ...item, quantity }
        : item

    );

    this.itemsSubject.next(items);

    this.saveCart();

  }


  // ======================================
  // حذف منتج من السلة
  // ======================================

  removeItem(cartItemId: number): void {

    const items = this.itemsSubject.value.filter(
      item => item.cartItemId !== cartItemId
    );

    this.itemsSubject.next(items);

    this.saveCart();

  }


  // ======================================
  // تفريغ السلة بعد تأكيد الطلب
  // ======================================

  clearCart(): void {

    this.itemsSubject.next([]);

    this.saveCart();

  }


  // ======================================
  // حساب مجموع المنتجات
  // ======================================

  getSubtotal(): number {

    return this.itemsSubject.value.reduce(

      (sum, item) =>
        sum + item.unitPrice * item.quantity,

      0

    );

  }


  // ======================================
  // إنشاء طلب جديد وحفظه
  // ======================================

  createOrder(data: CheckoutData): StoredOrder | null {

    const items = this.itemsSubject.value;

    // التأكد من وجود منتجات
    if (items.length === 0) {

      return null;

    }

    // التأكد من بيانات المستلم
    if (
      !data.receiverName.trim() ||
      !data.receiverPhone.trim() ||
      !data.deliveryAddress.trim()
    ) {

      return null;

    }

    // حساب الأسعار
    const subtotal = this.getSubtotal();

    const deliveryFee = Number(data.deliveryFee ?? 20);

    const discount = Number(data.discount ?? 0);

    const total = subtotal + deliveryFee - discount;


    // إنشاء الطلب
    const newOrder: StoredOrder = {

      orderId: Date.now(),

      orderDate: new Date().toISOString().slice(0, 10),

      status: 'قيد التجهيز',

      receiverName: data.receiverName.trim(),

      receiverPhone: data.receiverPhone.trim(),

      deliveryAddress: data.deliveryAddress.trim(),

      customerEmail: (data.customerEmail || '').trim().toLowerCase(),

      subtotal: subtotal,

      deliveryFee: deliveryFee,

      discount: discount,

      total: total,

      productImage: items[0].imageUrl,

      // حفظ جميع المنتجات والكميات داخل الطلب
      items: items.map(item => ({ ...item }))

    };


    // إضافة الطلب إلى قائمة الطلبات
    const orders = [
      newOrder,
      ...this.ordersSubject.value
    ];

    this.ordersSubject.next(orders);

    // حفظ الطلبات في المتصفح
    this.saveOrders();

    // تفريغ السلة بعد نجاح الحفظ
    this.clearCart();

    return newOrder;

  }


  // ======================================
  // استرجاع جميع الطلبات
  // ======================================

  getOrders(): StoredOrder[] {

    return this.ordersSubject.value;

  }


  // ======================================
  // استرجاع طلبات عميل محدد
  // ======================================

  getOrdersForCustomer(email: string): StoredOrder[] {

    const normalizedEmail = email.trim().toLowerCase();

    return this.ordersSubject.value.filter(

      order =>
        order.customerEmail === normalizedEmail

    );

  }


  // ======================================
  // تغيير حالة الطلب
  // ======================================

  updateOrderStatus(
    orderId: number,
    newStatus: string
  ): void {

    const orders = this.ordersSubject.value.map(order =>

      order.orderId === orderId
        ? { ...order, status: newStatus }
        : order

    );

    this.ordersSubject.next(orders);

    this.saveOrders();

  }


  // ======================================
  // حذف طلب
  // ======================================

  removeOrder(orderId: number): void {

    const orders = this.ordersSubject.value.filter(

      order => order.orderId !== orderId

    );

    this.ordersSubject.next(orders);

    this.saveOrders();

  }

}