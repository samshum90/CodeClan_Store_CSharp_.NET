import { OrderedProducts } from "./orderedProducts";

export interface nonAuthOrder {
    id?: number;
    orderCreated?: string;
    orderDate?: string;
    orderedProducts: OrderedProducts[];
}