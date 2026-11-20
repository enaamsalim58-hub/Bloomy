import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ActivatedRoute,
  Router,
  RouterModule
} from '@angular/router';

import { CartService } from '../services/cart.service';
import { AuthService } from '../auth.service';
import { WishlistService } from '../services/wishlist.service';

interface Product {
  id: number;
  name: string;
  price: number;
  reviews: number;
  description: string;
  design: string;
  ingredients: string;
  image: string;
}

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails {

  productId = 1;

  quantity = 1;

  selectedColor = 'lavender';

  selectedSize = 'متوسط';

  isFavorite = false;

  showLoginPrompt = false;

  // ==================================
  // بيانات المنتجات
  // ==================================

  products: Product[] = [

    // باقات الورود

    {
      id: 1,
      name: 'باقة لافندر الملكية',
      price: 199,
      reviews: 128,
      description: 'باقة فاخرة من الورود البنفسجية والوردية بتنسيق أنيق يناسب أجمل المناسبات.',
      design: 'تغليف بنفسجي فاخر وتنسيق أنيق.',
      ingredients: 'ورود طبيعية وورد جوري وأوراق خضراء.',
      image: '/Images/Flowers/FlowerLafnder.jpg'
    },
    {
      id: 2,
      name: 'باقة الورد الوردي',
      price: 179,
      reviews: 94,
      description: 'باقة جميلة من الورود الوردية بتنسيق رومانسي وتغليف أنيق.',
      design: 'تنسيق وردي ناعم مع تغليف فاخر.',
      ingredients: 'ورود وردية طبيعية وأوراق خضراء.',
      image: '/Images/Flowers/FlowerPink.jpg'
    },
    {
      id: 3,
      name: 'باقة الورود البيضاء',
      price: 189,
      reviews: 87,
      description: 'باقة من الورود البيضاء الجميلة بتنسيق هادئ وأنيق.',
      design: 'تنسيق كلاسيكي بتغليف أبيض راقٍ.',
      ingredients: 'ورود بيضاء طبيعية وأوراق خضراء.',
      image: '/Images/Flowers/FlowerWhite.jpg'
    },
    {
      id: 4,
      name: 'باقة الورد الملكي',
      price: 229,
      reviews: 105,
      description: 'تشكيلة فاخرة من الورود الملونة بتنسيق مميز وجذاب.',
      design: 'تنسيق ملكي متعدد الألوان.',
      ingredients: 'ورود طبيعية متنوعة وأوراق زينة.',
      image: '/Images/Flowers/FlowerQeen.jpg'
    },

    // الهدايا

    {
      id: 5,
      name: 'صندوق هدايا فاخر',
      price: 149,
      reviews: 95,
      description: 'صندوق هدايا أنيق بتغليف بنفسجي فاخر للمناسبات واللحظات المميزة.',
      design: 'صندوق فاخر مزين بشريط بنفسجي.',
      ingredients: 'صندوق هدايا وشريط ساتان وتنسيقات جميلة.',
      image: '/Images/Gifts/GiftFaker.jpg'
    },
    {
      id: 6,
      name: 'صندوق المفاجآت',
      price: 169,
      reviews: 82,
      description: 'صندوق مفاجآت أنيق يحتوي على هدية جميلة بتنسيق مميز.',
      design: 'صندوق هدايا بتغليف احتفالي.',
      ingredients: 'صندوق مزخرف وإضافات هدايا متنوعة.',
      image: '/Images/Gifts/GiftMofajat.jpg'
    },
    {
      id: 7,
      name: 'سلة الهدايا الوردية',
      price: 189,
      reviews: 91,
      description: 'سلة هدايا جميلة بتنسيق وردي ناعم ولمسات أنيقة.',
      design: 'سلة مرتبة بتغليف وردي فاخر.',
      ingredients: 'سلة هدايا وتنسيقات زينة.',
      image: '/Images/Gifts/GiftPink.jpg'
    },
    {
      id: 8,
      name: 'هدية المناسبات',
      price: 139,
      reviews: 76,
      description: 'هدية أنيقة مصممة لتكون اختياراً جميلاً في لحظاتك السعيدة.',
      design: 'تغليف أنيق بتفاصيل جميلة.',
      ingredients: 'مستلزمات هدايا وتغليف مزخرف.',
      image: '/Images/Gifts/GiftMonasabat.jpg'
    },

    // الشوكولاتة

    {
      id: 9,
      name: 'علبة شوكولاتة فاخرة',
      price: 99,
      reviews: 86,
      description: 'تشكيلة شوكولاتة فاخرة بمذاقات متنوعة وتغليف أنيق.',
      design: 'علبة أنيقة بتقسيم داخلي مرتب.',
      ingredients: 'تشكيلة من قطع الشوكولاتة المتنوعة.',
      image: '/Images/Chocolates/ChoFaker.jpg'
    },
    {
      id: 10,
      name: 'شوكولاتة بلجيكية',
      price: 129,
      reviews: 92,
      description: 'مجموعة من الشوكولاتة البلجيكية الفاخرة بتغليف جميل.',
      design: 'علبة فاخرة بتنسيق أنيق.',
      ingredients: 'قطع شوكولاتة بلجيكية متنوعة.',
      image: '/Images/Chocolates/ChoBlkajea.jpg'
    },
    {
      id: 11,
      name: 'صندوق الشوكولاتة المشكل',
      price: 149,
      reviews: 88,
      description: 'صندوق يضم تشكيلة متنوعة من الشوكولاتة بنكهات مختلفة.',
      design: 'صندوق مقسم بتنسيق مرتب.',
      ingredients: 'قطع شوكولاتة متنوعة.',
      image: '/Images/Chocolates/ChoMoshakel.jpg'
    },
    {
      id: 12,
      name: 'علبة شوكولاتة القلوب',
      price: 119,
      reviews: 79,
      description: 'علبة شوكولاتة جميلة بأشكال القلوب لتقديم هدية مميزة.',
      design: 'علبة بتصميم رومانسي أنيق.',
      ingredients: 'شوكولاتة بأشكال ونكهات متنوعة.',
      image: '/Images/Chocolates/ChoGolob.jpg'
    },

    // الدمى

    {
      id: 13,
      name: 'دمية الدب اللطيف',
      price: 119,
      reviews: 74,
      description: 'دمية دب ناعمة ومميزة لمحبي الهدايا اللطيفة.',
      design: 'دمية بتصميم لطيف وتفاصيل جميلة.',
      ingredients: 'خامة ناعمة وحشوة داخلية.',
      image: '/Images/Dolls/DDob.jpg'
    },
    {
      id: 14,
      name: 'دمية الأرنب',
      price: 129,
      reviews: 81,
      description: 'دمية أرنب ناعمة بتصميم جميل وألوان لطيفة.',
      design: 'تصميم أرنب لطيف بتفاصيل ناعمة.',
      ingredients: 'خامة ناعمة وحشوة مريحة.',
      image: '/Images/Dolls/DArnb.jpg'
    },
    {
      id: 15,
      name: 'الدب الوردي',
      price: 139,
      reviews: 89,
      description: 'دمية دب بتنسيق وردي جميل تضيف لمسة من البهجة.',
      design: 'دب وردي بتصميم جذاب.',
      ingredients: 'خامة ناعمة وحشوة داخلية.',
      image: '/Images/Dolls/DPink.jpg'
    },
    {
      id: 16,
      name: 'دمية الهدية',
      price: 159,
      reviews: 77,
      description: 'دمية مميزة بتصميم أنيق لتكون هدية جميلة.',
      design: 'دمية بتنسيق هدايا فاخر.',
      ingredients: 'خامة ناعمة وتفاصيل جميلة.',
      image: '/Images/Dolls/DGift.jpg'
    }

  ];

  product: Product = this.products[0];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService,
    private authService: AuthService,
    private wishlistService: WishlistService
  ) {

    this.route.paramMap.subscribe(params => {

      const id = Number(params.get('id')) || 1;

      this.productId = id;

      this.product =
        this.products.find(p => p.id === id) || this.products[0];

      this.quantity = 1;

      this.selectedColor = 'lavender';

      this.selectedSize = 'متوسط';

      this.updateFavoriteStatus();

    });

  }

  // تحديث حالة القلب عند فتح المنتج
  updateFavoriteStatus(): void {

    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {
      this.isFavorite = false;
      return;
    }

    this.isFavorite = this.wishlistService.isFavorite(
      user.email,
      this.product.id
    );

  }

  // زيادة الكمية
  increaseQuantity(): void {
    this.quantity++;
  }

  // تقليل الكمية
  decreaseQuantity(): void {

    if (this.quantity > 1) {
      this.quantity--;
    }

  }

  // ==================================
  // إضافة أو حذف المنتج من المفضلة
  // ==================================

  toggleFavorite(): void {

    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {

      alert('يرجى تسجيل الدخول بحساب عميل لاستخدام المفضلة ❤️');

      this.router.navigate(['/login']);

      return;

    }

    this.isFavorite = this.wishlistService.toggleFavorite(
      user.email,
      {
        productId: this.product.id,
        productName: this.product.name,
        price: this.product.price,
        image: this.product.image
      }
    );

    alert(
      this.isFavorite
        ? 'تمت إضافة المنتج إلى المفضلة ❤️'
        : 'تم حذف المنتج من المفضلة'
    );

  }

  // ==================================
  // إضافة المنتج إلى السلة
  // ==================================

  addToCart(): void {

    if (!this.authService.isCustomer()) {

      this.showLoginPrompt = true;

      return;

    }

    this.cartService.addToCart({

      productId: this.product.id,

      name: this.product.name,

      price: this.product.price,

      image: this.product.image,

      quantity: this.quantity,

      sizeName: this.selectedSize,

      colorName: this.selectedColor

    });

    alert('تمت إضافة المنتج إلى السلة ❤️');

    this.router.navigate(['/cart']);

  }

  // معالجة الصورة إذا لم يتم العثور عليها
  onImageError(event: Event): void {

    const image = event.target as HTMLImageElement;

    image.style.display = 'none';

    console.error(
      'لم يتم العثور على صورة المنتج:',
      this.product.image
    );

  }

  // الانتقال لتسجيل الدخول
  goToLogin(): void {

    this.showLoginPrompt = false;

    this.router.navigate(['/login']);

  }

  // إغلاق نافذة تسجيل الدخول
  closeLoginPrompt(): void {
    this.showLoginPrompt = false;
  }

}