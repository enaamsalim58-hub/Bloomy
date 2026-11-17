import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

export interface ProductItem {
  productId: number;
  name: string;
  description?: string;
  price: number;
  stockQuantity: number;
  isAvailable: boolean;
  mainImageUrl: string;
  categoryId: number;
  categoryName: string;
}

export interface CategoryItem {
  categoryId: number;
  name: string;
  imageUrl: string;
}

@Component({
  selector: 'app-manage-products',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './manage-products.html',
  styleUrl: './manage-products.css'
})
export class ManageProducts {
  activeTab: 'products' | 'categories' = 'products';

  // نفس مسارات الصور المعتمدة في صفحات زميلاتكِ (f.jpeg, k.jpeg, d.jpeg, h.jpeg)
  categories: CategoryItem[] = [
    { categoryId: 1, name: 'باقات الورود', imageUrl: '/Images/f.jpeg' },
    { categoryId: 2, name: 'شوكولاتة', imageUrl: '/Images/k.jpeg' },
    { categoryId: 3, name: 'دمى', imageUrl: '/Images/d.jpeg' },
    { categoryId: 4, name: 'هدايا', imageUrl: '/Images/h.jpeg' }
  ];

  // المنتجات بنفس الصور المرفقة في المشروع دون أي تغيير على مجلد الصور
  products: ProductItem[] = [
    {
      productId: 1,
      categoryId: 1,
      categoryName: 'باقات الورود',
      name: 'باقة لافندر الملكية',
      description: 'باقة فاخرة من الورود البنفسجية والبيضاء',
      price: 199,
      stockQuantity: 15,
      isAvailable: true,
      mainImageUrl: '/Images/f.jpeg'
    },
    {
      productId: 2,
      categoryId: 1,
      categoryName: 'باقات الورود',
      name: 'باقة ورد أحمر',
      description: 'تنسيق أنيق من الجوري الأحمر الطبيعي',
      price: 149,
      stockQuantity: 20,
      isAvailable: true,
      mainImageUrl: '/Images/j.jpeg'
    },
    {
      productId: 3,
      categoryId: 3,
      categoryName: 'دمى',
      name: 'دمية الحب',
      description: 'دمية ناعمة مثالية للإهداء مع الباقات',
      price: 90,
      stockQuantity: 12,
      isAvailable: true,
      mainImageUrl: '/Images/d.jpeg'
    },
    {
      productId: 4,
      categoryId: 2,
      categoryName: 'شوكولاتة',
      name: 'علبة شوكولاتة فاخرة',
      description: 'تشكيلة شوكولاتة بلجيكية مغلفة بعناية',
      price: 80,
      stockQuantity: 25,
      isAvailable: true,
      mainImageUrl: '/Images/k.jpeg'
    }
  ];

  searchTerm = '';
  categorySearchTerm = '';
  currentPage = 1;

  showProductForm = false;
  isEditingProduct = false;
  editingProductId: number | null = null;

  productForm = {
    name: '',
    description: '',
    price: 100,
    stockQuantity: 10,
    mainImageUrl: '/Images/f.jpeg',
    categoryId: 1
  };

  showCategoryForm = false;
  isEditingCategory = false;
  editingCategoryId: number | null = null;

  categoryForm = {
    name: '',
    imageUrl: '/Images/h.jpeg'
  };

  statusMessage = '';
  errorMessage = '';

  get filteredProducts(): ProductItem[] {
    if (!this.searchTerm.trim()) {
      return this.products;
    }
    const q = this.searchTerm.trim().toLowerCase();
    return this.products.filter(
      (p) =>
        p.name.toLowerCase().includes(q) ||
        p.categoryName.toLowerCase().includes(q)
    );
  }

  get filteredCategories(): CategoryItem[] {
    if (!this.categorySearchTerm.trim()) {
      return this.categories;
    }
    const q = this.categorySearchTerm.trim().toLowerCase();
    return this.categories.filter((c) => c.name.toLowerCase().includes(q));
  }

  getCategoryProductsCount(categoryId: number): number {
    return this.products.filter((p) => p.categoryId === categoryId).length;
  }

  switchTab(tab: 'products' | 'categories'): void {
    this.activeTab = tab;
    this.clearMessages();
    this.showProductForm = false;
    this.showCategoryForm = false;
  }

  selectPage(page: number): void {
    if (page >= 1 && page <= 3) {
      this.currentPage = page;
    }
  }

  openAddProduct(): void {
    this.clearMessages();
    this.isEditingProduct = false;
    this.editingProductId = null;
    this.productForm = {
      name: '',
      description: '',
      price: 100,
      stockQuantity: 10,
      mainImageUrl: '/Images/f.jpeg',
      categoryId: this.categories[0]?.categoryId || 1
    };
    this.showProductForm = true;
  }

  openEditProduct(item: ProductItem): void {
    this.clearMessages();
    this.isEditingProduct = true;
    this.editingProductId = item.productId;
    this.productForm = {
      name: item.name,
      description: item.description || '',
      price: item.price,
      stockQuantity: item.stockQuantity,
      mainImageUrl: item.mainImageUrl,
      categoryId: item.categoryId
    };
    this.showProductForm = true;
  }

  saveProduct(): void {
    this.clearMessages();
    if (!this.productForm.name.trim()) {
      this.errorMessage = 'يرجى إدخال اسم المنتج.';
      return;
    }
    if (Number(this.productForm.price) <= 0) {
      this.errorMessage = 'يجب أن يكون السعر أكبر من صفر.';
      return;
    }

    const catId = Number(this.productForm.categoryId);
    const catName =
      this.categories.find((c) => c.categoryId === catId)?.name || 'باقات الورود';
    const qty = Number(this.productForm.stockQuantity);

    if (this.isEditingProduct && this.editingProductId !== null) {
      const idx = this.products.findIndex((p) => p.productId === this.editingProductId);
      if (idx !== -1) {
        this.products[idx] = {
          productId: this.editingProductId,
          name: this.productForm.name.trim(),
          description: this.productForm.description.trim(),
          price: Number(this.productForm.price),
          stockQuantity: qty,
          isAvailable: qty > 0,
          mainImageUrl: this.productForm.mainImageUrl.trim() || '/Images/f.jpeg',
          categoryId: catId,
          categoryName: catName
        };
      }
      this.statusMessage = 'تم تعديل بيانات المنتج بنجاح.';
    } else {
      const nextId =
        this.products.length > 0
          ? Math.max(...this.products.map((p) => p.productId)) + 1
          : 1;

      this.products.unshift({
        productId: nextId,
        name: this.productForm.name.trim(),
        description: this.productForm.description.trim(),
        price: Number(this.productForm.price),
        stockQuantity: qty,
        isAvailable: qty > 0,
        mainImageUrl: this.productForm.mainImageUrl.trim() || '/Images/f.jpeg',
        categoryId: catId,
        categoryName: catName
      });
      this.statusMessage = 'تمت إضافة المنتج الجديد بنجاح.';
    }

    this.showProductForm = false;
  }

  deleteProduct(item: ProductItem): void {
    this.clearMessages();
    if (!confirm(`هل أنتِ متأكدة من حذف "${item.name}"؟`)) {
      return;
    }
    this.products = this.products.filter((p) => p.productId !== item.productId);
    this.statusMessage = 'تم حذف المنتج بنجاح.';
  }

  openAddCategory(): void {
    this.clearMessages();
    this.isEditingCategory = false;
    this.editingCategoryId = null;
    this.categoryForm = { name: '', imageUrl: '/Images/h.jpeg' };
    this.showCategoryForm = true;
  }

  openEditCategory(cat: CategoryItem): void {
    this.clearMessages();
    this.isEditingCategory = true;
    this.editingCategoryId = cat.categoryId;
    this.categoryForm = { name: cat.name, imageUrl: cat.imageUrl };
    this.showCategoryForm = true;
  }

  saveCategory(): void {
    this.clearMessages();
    if (!this.categoryForm.name.trim()) {
      this.errorMessage = 'يرجى إدخال اسم التصنيف.';
      return;
    }

    if (this.isEditingCategory && this.editingCategoryId !== null) {
      const idx = this.categories.findIndex((c) => c.categoryId === this.editingCategoryId);
      if (idx !== -1) {
        this.categories[idx] = {
          categoryId: this.editingCategoryId,
          name: this.categoryForm.name.trim(),
          imageUrl: this.categoryForm.imageUrl.trim() || '/Images/h.jpeg'
        };
      }
      this.statusMessage = 'تم تعديل التصنيف بنجاح.';
    } else {
      const nextId =
        this.categories.length > 0
          ? Math.max(...this.categories.map((c) => c.categoryId)) + 1
          : 1;

      this.categories.push({
        categoryId: nextId,
        name: this.categoryForm.name.trim(),
        imageUrl: this.categoryForm.imageUrl.trim() || '/Images/h.jpeg'
      });
      this.statusMessage = 'تمت إضافة التصنيف الجديد بنجاح.';
    }

    this.showCategoryForm = false;
  }

  deleteCategory(cat: CategoryItem): void {
    this.clearMessages();
    if (!confirm(`هل أنتِ متأكدة من حذف تصنيف "${cat.name}"؟`)) {
      return;
    }
    this.categories = this.categories.filter((c) => c.categoryId !== cat.categoryId);
    this.statusMessage = 'تم حذف التصنيف بنجاح.';
  }

  private clearMessages(): void {
    this.statusMessage = '';
    this.errorMessage = '';
  }
}