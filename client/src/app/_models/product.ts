import { ProductPhoto } from "./productPhoto";

export interface Product {
    id: number;
    name: string;
    photoUrl: string;
    productPrice: string;
    salePrice: string;
    description: string;
    category: string;
    stock: number;
    highlight: boolean;
    photos: ProductPhoto[];
}