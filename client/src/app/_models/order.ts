import { OrderedProducts } from "./orderedProducts";

export interface Order {
    id?: number;
    orderCreated: Date;
    orderDate?: Date;
    lastUpdate: Date;
    orderedProducts: OrderedProducts[];
}