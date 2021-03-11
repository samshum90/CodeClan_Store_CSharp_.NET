import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { OrderedProducts } from '../_models/orderedProducts';
import { Product } from '../_models/product';
import { Quantity } from '../_models/quantity';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  basketChanged = new Subject<Order>();
  baseUrl = environment.apiUrl;
  private basket!: Order;
  numOfItems: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  constructor(private http: HttpClient) { }

  setBasket(basket: Order) {
    this.basket = basket;
    this.basketChanged.next(basket);
    this.numOfItems.next(basket.orderedProducts.map(op => op.quantity).reduce((acc, value) => acc + value, 0));
  }

  getBasket() {
    return this.basket;
  }

  // getNumberOfItems() {
  //   if (!!this.basket) {
  //     this.numOfItems.next(this.basket.orderedProducts.map(op => op.quantity).reduce((acc, value) => acc + value, 0));
  //   }
  // if (!!this.basket) {
  //   return this.basket.orderedProducts.map(op => op.quantity).reduce((acc, value) => acc + value, 0);
  // } else {
  //   return 0;
  // }
  // }

  deleteBasket() {
    this.basketChanged.next();
  }

  addProduct(orderedProduct: OrderedProducts) {
    return this.http.post(this.baseUrl + "orders", orderedProduct)
      .pipe(map((res: any) => {
        this.setBasket(res)
        // this.basketChanged.next(res);
      }));
    // .subscribe(
    //   (res: any) => {
    //     if (!!this.basket) {
    //       this.basket.orderedProducts.push(res);
    //     }
    //     this.basketChanged.next(this.basket);
    //   },
    //   (error) => {
    //     console.error(error);
    //   })
  }

  updateProduct(qty: number, od: OrderedProducts) {
    const index = this.basket.orderedProducts.indexOf(od);
    if (qty === 0) {
      return this.http.delete(this.baseUrl + "orders/delete-item/" + od.product.id)
        .pipe(map(() => {
          this.basket.orderedProducts.splice(index, 1);
          this.setBasket(this.basket)
        }))
    } else {
      const quantity: Quantity = {
        quantity: qty,
      }
      return this.http.put(this.baseUrl + "orders/edit-item/" + od.product.id, quantity)
        .pipe(map(() => {
          // const index = this.basket.orderedProducts.findIndex(od => od.product.id === product.id)
          this.basket.orderedProducts[index].quantity = qty;
          // this.basketChanged.next(this.basket)
          this.setBasket(this.basket)
        }));
    }
  }
}
