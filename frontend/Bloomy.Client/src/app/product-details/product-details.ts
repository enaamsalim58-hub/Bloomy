import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { CartService } from '../services/cart.service';
import { AuthService } from '../auth.service';

interface Product {
  id: number;
  name: string;
  price: number;
  reviews: number;
  description: string;
  design: string;
  ingredients: string;
  occasion: string;
  image: string;
}

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
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
  // بيانات المنتجات الستة عشر
  // كل منتج له تفاصيله وصورته الخاصة
  // ==================================

  products: Product[] = [

    // باقات الورود

    {
      id: 1,
      name: 'باقة لافندر الملكية',
      price: 199,
      reviews: 128,
      description: 'باقة فاخرة تجمع بين جمال الورود البنفسجية والوردية، بتنسيق أنيق وتغليف راقٍ يناسب أجمل المناسبات.',
      design: 'تنسيق أنيق بلمسة عصرية وتغليف بنفسجي فاخر.',
      ingredients: 'ورود طبيعية، ورد جوري، وأوراق خضراء.',
      occasion: 'أعياد الميلاد، المناسبات الخاصة، والتعبير عن الحب.',
      image: '/Images/flowers/flower-1.jpeg'
    },
    {
      id: 2,
      name: 'باقة الورد الوردي',
      price: 179,
      reviews: 94,
      description: 'باقة جميلة من الورود الوردية بتنسيق رومانسي وتغليف أنيق.',
      design: 'تنسيق وردي ناعم مع تغليف فاخر.',
      ingredients: 'ورود وردية طبيعية وأوراق خضراء.',
      occasion: 'أعياد الميلاد، التهنئة، والتعبير عن المحبة.',
      image: '/Images/flowers/flower-2.jpeg'
    },
    {
      id: 3,
      name: 'باقة الورود البيضاء',
      price: 189,
      reviews: 87,
      description: 'باقة من الورود البيضاء الجميلة بتنسيق هادئ وأنيق.',
      design: 'تنسيق كلاسيكي بتغليف أبيض راقٍ.',
      ingredients: 'ورود بيضاء طبيعية وأوراق خضراء.',
      occasion: 'التهنئة، المناسبات الخاصة، والتقدير.',
      image: '/Images/flowers/flower-3.jpeg'
    },
    {
      id: 4,
      name: 'باقة الورد الملكي',
      price: 229,
      reviews: 105,
      description: 'تشكيلة فاخرة من الورود الملونة بتنسيق مميز وجذاب.',
      design: 'تنسيق ملكي متعدد الألوان مع تغليف فاخر.',
      ingredients: 'ورود طبيعية متنوعة وأوراق زينة.',
      occasion: 'الاحتفالات، أعياد الميلاد، والمناسبات المميزة.',
      image: '/Images/flowers/flower-4.jpeg'
    },

    // الهدايا

    {
      id: 5,
      name: 'صندوق هدايا فاخر',
      price: 149,
      reviews: 95,
      description: 'صندوق هدايا أنيق بتغليف بنفسجي فاخر، ليكون اختياراً جميلاً للمناسبات واللحظات المميزة.',
      design: 'صندوق فاخر مزين بشريط بنفسجي أنيق.',
      ingredients: 'صندوق هدايا، شريط ساتان، وتنسيقات جميلة.',
      occasion: 'أعياد الميلاد، التخرج، والمناسبات الخاصة.',
      image: '/Images/gifts/gift-1.jpeg'
    },
    {
      id: 6,
      name: 'صندوق المفاجآت',
      price: 169,
      reviews: 82,
      description: 'صندوق مفاجآت أنيق يحتوي على هدية جميلة بتنسيق مميز.',
      design: 'صندوق هدايا بتغليف احتفالي.',
      ingredients: 'صندوق مزخرف وإضافات هدايا متنوعة.',
      occasion: 'أعياد الميلاد والمفاجآت.',
      image: '/Images/gifts/gift-2.jpeg'
    },
    {
      id: 7,
      name: 'سلة الهدايا الوردية',
      price: 189,
      reviews: 91,
      description: 'سلة هدايا جميلة بتنسيق وردي ناعم ولمسات أنيقة.',
      design: 'سلة مرتبة بتغليف وردي فاخر.',
      ingredients: 'سلة هدايا وتنسيقات زينة.',
      occasion: 'الزيارات والمناسبات الخاصة.',
      image: '/Images/gifts/gift-3.jpeg'
    },
    {
      id: 8,
      name: 'هدية المناسبات',
      price: 139,
      reviews: 76,
      description: 'هدية أنيقة مصممة لتكون اختياراً جميلاً في لحظاتك السعيدة.',
      design: 'تغليف أنيق بتفاصيل جميلة.',
      ingredients: 'مستلزمات هدايا وتغليف مزخرف.',
      occasion: 'التخرج والنجاح وأعياد الميلاد.',
      image: '/Images/gifts/gift-4.jpeg'
    },

    // الشوكولاتة

    {
      id: 9,
      name: 'علبة شوكولاتة فاخرة',
      price: 99,
      reviews: 86,
      description: 'تشكيلة شوكولاتة فاخرة بمذاقات متنوعة وتغليف أنيق، مناسبة لمشاركة أجمل اللحظات.',
      design: 'علبة أنيقة بتقسيم داخلي مرتب.',
      ingredients: 'تشكيلة من قطع الشوكولاتة المتنوعة.',
      occasion: 'الزيارات، الأعياد، والمناسبات السعيدة.',
      image: '/Images/chocolates/chocolate-1.jpeg'
    },
    {
      id: 10,
      name: 'شوكولاتة بلجيكية',
      price: 129,
      reviews: 92,
      description: 'مجموعة من الشوكولاتة البلجيكية الفاخرة بتغليف جميل.',
      design: 'علبة فاخرة بتنسيق أنيق.',
      ingredients: 'قطع شوكولاتة بلجيكية متنوعة.',
      occasion: 'الهدايا والاحتفالات.',
      image: '/Images/chocolates/chocolate-2.jpeg'
    },
    {
      id: 11,
      name: 'صندوق الشوكولاتة المشكل',
      price: 149,
      reviews: 88,
      description: 'صندوق يضم تشكيلة متنوعة من الشوكولاتة بنكهات مختلفة.',
      design: 'صندوق مقسم بتنسيق مرتب.',
      ingredients: 'قطع شوكولاتة متنوعة.',
      occasion: 'الزيارات والمناسبات الخاصة.',
      image: '/Images/chocolates/chocolate-3.jpeg'
    },
    {
      id: 12,
      name: 'علبة شوكولاتة القلوب',
      price: 119,
      reviews: 79,
      description: 'علبة شوكولاتة جميلة بأشكال القلوب لتقديم هدية مميزة.',
      design: 'علبة بتصميم رومانسي أنيق.',
      ingredients: 'شوكولاتة بأشكال ونكهات متنوعة.',
      occasion: 'التعبير عن الحب والمناسبات السعيدة.',
      image: '/Images/chocolates/chocolate-4.jpeg'
    },

    // الدمى

    {
      id: 13,
      name: 'دمية الدب اللطيف',
      price: 119,
      reviews: 74,
      description: 'دمية دب ناعمة ومميزة لمحبي الهدايا اللطيفة.',
      design: 'دمية بتصميم لطيف وتفاصيل جميلة.',
      ingredients: 'خامة ناعمة مناسبة كهدية.',
      occasion: 'أعياد الميلاد والمفاجآت.',
      image: '/Images/dolls/doll-1.jpeg'
    },
    {
      id: 14,
      name: 'دمية الأرنب',
      price: 129,
      reviews: 81,
      description: 'دمية أرنب ناعمة بتصميم جميل وألوان لطيفة.',
      design: 'تصميم أرنب لطيف بتفاصيل ناعمة.',
      ingredients: 'خامة ناعمة وحشوة مريحة.',
      occasion: 'الهدايا وأعياد الميلاد.',
      image: '/Images/dolls/doll-2.jpeg'
    },
    {
      id: 15,
      name: 'الدب الوردي',
      price: 139,
      reviews: 89,
      description: 'دمية دب بتنسيق وردي جميل تضيف لمسة من البهجة.',
      design: 'دب وردي بتصميم جذاب.',
      ingredients: 'خامة ناعمة وحشوة داخلية.',
      occasion: 'المفاجآت والمناسبات الخاصة.',
      image: '/Images/dolls/doll-3.jpeg'
    },
    {
      id: 16,
      name: 'دمية الهدية',
      price: 159,
      reviews: 77,
      description: 'دمية مميزة بتصميم أنيق لتكون هدية جميلة لمن تحبين.',
      design: 'دمية بتنسيق هدايا فاخر.',
      ingredients: 'خامة ناعمة وتفاصيل جميلة.',
      occasion: 'أعياد الميلاد والتخرج والهدايا.',
      image: '/Images/dolls/doll-4.jpeg'
    }

  ];

  product: Product = this.products[0];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService,
    private authService: AuthService
  ) {

    this.route.paramMap.subscribe(params => {

      const id = Number(params.get('id')) || 1;

      this.productId = id;

      this.product =
        this.products.find(p => p.id === id) || this.products[0];

      this.quantity = 1;
      this.selectedColor = 'lavender';
      this.selectedSize = 'متوسط';
      this.isFavorite = false;
    });

  }

  increaseQuantity(): void {
    this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  toggleFavorite(): void {
    this.isFavorite = !this.isFavorite;
  }

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

  goToLogin(): void {
    this.showLoginPrompt = false;
    this.router.navigate(['/login']);
  }

  closeLoginPrompt(): void {
    this.showLoginPrompt = false;
  }

}