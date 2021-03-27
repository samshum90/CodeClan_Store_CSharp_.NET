import { OrderedProducts } from "./orderedProducts";

export interface Order {
    id?: number;
    orderCreated: string;
    orderDate?: string;
    orderedProducts: OrderedProducts[];
}