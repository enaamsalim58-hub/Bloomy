import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  Router,
  RouterModule
} from '@angular/router';

import { AuthService } from '../auth.service';
import { WishlistService } from '../services/wishlist.service';

interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  image: string;
  categoryName: string;
}

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products {

  selectedCategory = 'الكل';

  categories = [
    { name: 'الكل', icon: '▦' },
    { name: 'باقات الورود', icon: '🌸' },
    { name: 'الهدايا', icon: '🎁' },
    { name: 'الشوكولاتة', icon: '🍫' },
    { name: 'الدمى', icon: '🧸' }
  ];

  // ==================================
  // جميع المنتجات
  // ==================================

  products: Product[] = [

    // الورود

    {
      productId: 1,
      name: 'باقة لافندر الملكية',
      description: 'باقة فاخرة من الورود البنفسجية والوردية',
      price: 199,
      image: '/Images/Flowers/FlowerLafnder.jpg',
      categoryName: 'باقات الورود'
    },
    {
      productId: 2,
      name: 'باقة الورد الوردي',
      description: 'باقة أنيقة من أجمل الورود الوردية',
      price: 179,
      image: '/Images/Flowers/FlowerPink.jpg',
      categoryName: 'باقات الورود'
    },
    {
      productId: 3,
      name: 'باقة الورود البيضاء',
      description: 'ورود بيضاء بتنسيق ناعم وأنيق',
      price: 189,
      image: '/Images/Flowers/FlowerWhite.jpg',
      categoryName: 'باقات الورود'
    },
    {
      productId: 4,
      name: 'باقة الورد الملكي',
      description: 'تشكيلة فاخرة من أجمل الورود الملونة',
      price: 229,
      image: '/Images/Flowers/FlowerQeen.jpg',
      categoryName: 'باقات الورود'
    },

    // الهدايا

    {
      productId: 5,
      name: 'صندوق هدايا فاخر',
      description: 'صندوق هدايا أنيق بتغليف بنفسجي فاخر',
      price: 149,
      image: '/Images/Gifts/GiftFaker.jpg',
      categoryName: 'الهدايا'
    },
    {
      productId: 6,
      name: 'صندوق المفاجآت',
      description: 'هدية جميلة لمفاجأة الأشخاص المميزين',
      price: 169,
      image: '/Images/Gifts/GiftMofajat.jpg',
      categoryName: 'الهدايا'
    },
    {
      productId: 7,
      name: 'سلة الهدايا الوردية',
      description: 'سلة هدايا مرتبة بلمسات وردية جميلة',
      price: 189,
      image: '/Images/Gifts/GiftPink.jpg',
      categoryName: 'الهدايا'
    },
    {
      productId: 8,
      name: 'هدية المناسبات',
      description: 'هدية أنيقة للمناسبات واللحظات السعيدة',
      price: 139,
      image: '/Images/Gifts/GiftMonasabat.jpg',
      categoryName: 'الهدايا'
    },

    // الشوكولاتة

    {
      productId: 9,
      name: 'علبة شوكولاتة فاخرة',
      description: 'تشكيلة شوكولاتة فاخرة بتغليف أنيق',
      price: 99,
      image: '/Images/Chocolates/ChoFaker.jpg',
      categoryName: 'الشوكولاتة'
    },
    {
      productId: 10,
      name: 'شوكولاتة بلجيكية',
      description: 'قطع شوكولاتة بلجيكية بمذاق غني',
      price: 129,
      image: '/Images/Chocolates/ChoBlkajea.jpg',
      categoryName: 'الشوكولاتة'
    },
    {
      productId: 11,
      name: 'صندوق الشوكولاتة المشكل',
      description: 'تشكيلة متنوعة من الشوكولاتة الفاخرة',
      price: 149,
      image: '/Images/Chocolates/ChoMoshakel.jpg',
      categoryName: 'الشوكولاتة'
    },
    {
      productId: 12,
      name: 'علبة شوكولاتة القلوب',
      description: 'شوكولاتة بأشكال القلوب لتقديم هدية مميزة',
      price: 119,
      image: '/Images/Chocolates/ChoGolob.jpg',
      categoryName: 'الشوكولاتة'
    },

    // الدمى

    {
      productId: 13,
      name: 'دمية الدب اللطيف',
      description: 'دمية دب ناعمة ومميزة لمحبي الهدايا اللطيفة',
      price: 119,
      image: '/Images/Dolls/DDob.jpg',
      categoryName: 'الدمى'
    },
    {
      productId: 14,
      name: 'دمية الأرنب',
      description: 'دمية أرنب ناعمة بتصميم جميل',
      price: 129,
      image: '/Images/Dolls/DArnb.jpg',
      categoryName: 'الدمى'
    },
    {
      productId: 15,
      name: 'الدب الوردي',
      description: 'دمية دب بتنسيق وردي لطيف',
      price: 139,
      image: '/Images/Dolls/DPink.jpg',
      categoryName: 'الدمى'
    },
    {
      productId: 16,
      name: 'دمية الهدية',
      description: 'دمية جميلة مناسبة لتقديمها كهدية',
      price: 159,
      image: '/Images/Dolls/DGift.jpg',
      categoryName: 'الدمى'
    }

  ];

  constructor(
    private wishlistService: WishlistService,
    private authService: AuthService,
    private router: Router
  ) {}

  // ==================================
  // عرض المنتجات حسب التصنيف
  // ==================================

  get filteredProducts(): Product[] {

    // عند اختيار الكل: بطاقة واحدة من كل تصنيف
    if (this.selectedCategory === 'الكل') {

      const featuredProductIds = [1, 5, 9, 13];

      return this.products.filter(
        product => featuredProductIds.includes(product.productId)
      );

    }

    // عند اختيار تصنيف: عرض بطاقاته الأربع
    return this.products.filter(
      product => product.categoryName === this.selectedCategory
    );

  }

  // تغيير التصنيف
  selectCategory(category: string): void {
    this.selectedCategory = category;
  }

  // ==================================
  // إضافة أو حذف المنتج من المفضلة
  // ==================================

  toggleFavorite(productId: number): void {

    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {

      alert('يرجى تسجيل الدخول بحساب عميل لاستخدام المفضلة ❤️');

      this.router.navigate(['/login']);

      return;

    }

    const product = this.products.find(
      p => p.productId === productId
    );

    if (!product) return;

    const added = this.wishlistService.toggleFavorite(
      user.email,
      {
        productId: product.productId,
        productName: product.name,
        price: product.price,
        image: product.image
      }
    );

    alert(
      added
        ? 'تمت إضافة المنتج إلى المفضلة ❤️'
        : 'تم حذف المنتج من المفضلة'
    );

  }

  // التحقق من المفضلة
  isFavorite(productId: number): boolean {

    const user = this.authService.currentUser;

    if (!user || user.role !== 'customer') {
      return false;
    }

    return this.wishlistService.isFavorite(
      user.email,
      productId
    );

  }

}