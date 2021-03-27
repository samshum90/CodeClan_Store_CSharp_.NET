import { OrderedProducts } from "./orderedProducts";

export interface Order {
    id?: number;
    orderCreated: Date;
    lastUpdate: Date;
    orderDate?: Date;
    orderedProducts: OrderedProducts[];
}