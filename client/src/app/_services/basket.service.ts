import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { OrderedProducts } from '../_models/orderedProducts';
import { Product } from '../_models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  basketChanged = new Subject<Order>();
  baseUrl = environment.apiUrl;
  private basket!: Order;

  constructor(private http: HttpClient) { }

  setBasket(basket: Order) {
    this.basket = basket;
    this.basketChanged.next(basket);
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
    this.basketChanged.next();
  }

  addProduct(orderedProduct: OrderedProducts) {
    return this.http.post(this.baseUrl + "orders", orderedProduct)
      // .pipe(map((res: any) => {
      //   this.basket.orderedProducts.push(res);
      //   this.basketChanged.next(this.basket);
      //   console.log(this.basket)
      // }));
      .subscribe(
        (res: any) => {                           //Next callback
          this.basket.orderedProducts.push(res);
          this.basketChanged.next(this.basket);
        },
        (error) => {
          console.error(error);
        })
  }
}
