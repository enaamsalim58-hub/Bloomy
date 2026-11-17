export interface Product {
  productId: number;
  name: string;
  description: string;
  price: number;
  stockQuantity: number;
  isAvailable: boolean;
  mainImageUrl: string;
  categoryName: string;
}