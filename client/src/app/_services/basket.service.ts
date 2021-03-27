import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { OrderedProducts } from '../_models/orderedProducts';
import { Product } from '../_models/product';
import { Quantity } from '../_models/quantity';
import { User } from '../_models/user';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  basketChanged = new Subject<Order>();
  baseUrl = environment.apiUrl;
  private basket!: Order;
  numOfItems: BehaviorSubject<number> = new BehaviorSubject<number>(0);
  user!: User;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  setBasket(basket: Order) {
    this.basket = basket;
    this.basketChanged.next(basket);
    this.numOfItems.next(basket.orderedProducts.map(op => op.quantity).reduce((acc, value) => acc + value, 0));
  }

  getBasket() {
    return this.basket;
  }

  deleteBasket() {
    this.basketChanged.next();
    localStorage.removeItem('basket');
  }

  addProduct(orderedProduct: OrderedProducts) {
    if (!!this.user) {
      this.http.post(this.baseUrl + "orders", orderedProduct)
        // .pipe(map((res: any) => {
        //   this.setBasket(res)
        // }));
        .subscribe(
          (res: any) => {
            if (!!this.basket) {
              this.basket.orderedProducts.push(res);
            }
            this.setBasket(res);
            localStorage.setItem('basket', JSON.stringify(res));
          })
    } else {
      this.basket.orderedProducts.push(orderedProduct);
      this.setBasket(this.basket);
      localStorage.setItem('basket', JSON.stringify(this.basket));
    }

  }

  updateProduct(qty: number, od: OrderedProducts) {
    const index = this.basket.orderedProducts.indexOf(od);
    if (!!this.user) {
      if (qty === 0) {
        return this.http.delete(this.baseUrl + "orders/delete-item/" + od.product.id)
          .pipe(map(() => {
            this.basket.orderedProducts.splice(index, 1);
            this.setBasket(this.basket);
            localStorage.setItem('basket', JSON.stringify(this.basket));
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
            this.setBasket(this.basket);
            localStorage.setItem('basket', JSON.stringify(this.basket));
          }));
      }
    } else {
      if (qty === 0) {
        this.basket.orderedProducts.splice(index, 1);
      } else {
        this.basket.orderedProducts[index].quantity = qty;
      }
      this.setBasket(this.basket);
      localStorage.setItem('basket', JSON.stringify(this.basket));
      return of();
    }
  }
}
