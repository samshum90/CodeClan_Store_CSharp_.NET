import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { Product } from '../_models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  basket = new Subject<Order>();

  constructor() { }

  setBasket(basket: Order) {
    this.basket.next(basket);
  }

  getBasket() {
    return this.basket;
  }

  // getNumberOfItems() {
  //   this.basket.orderedProducts.
  // }
  // addBasket(basket: Product) {
  //   this.basket.push(basket);
  //   this.basketChanged.next(this.basket.slice())
  // }

  // updateBasket(index: number, newProduct: Product) {
  //   this.basket[index] = newProduct
  //   this.basketChanged.next(this.basket.slice())
  // }

  deleteBasket() {
    this.basket.next();
  }

}
