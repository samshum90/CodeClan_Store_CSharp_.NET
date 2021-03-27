import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Order } from '../_models/order';
import { Product } from '../_models/product';
import { BasketService } from './basket.service';

@Injectable({
  providedIn: 'root'
})
export class DataStorageService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private basketService: BasketService) { }

  fetchBasket() {
    return this.http
      .get<Order>(this.baseUrl + 'orders/basket')
      .pipe(map(res => {
        const localBasket: Order = JSON.parse(localStorage.getItem("basket"));
        if (localBasket === null || localBasket.orderedProducts.length === 0) {
          this.basketService.setBasket(res);
        }
        if (new Date(res.lastUpdate) > new Date(localBasket.lastUpdate)) {
          this.basketService.setBasket(res);
        } else {
          this.basketService.updateBasket(localBasket);
        }

        return res;
      }))
  }

  setNewLocalStorageBasket() {
    const basket: Order = {
      orderCreated: new Date(),
      lastUpdate: new Date(),
      orderedProducts: []
    };
    localStorage.setItem('basket', JSON.stringify(basket));
    this.basketService.setBasket(basket);
    return basket;
  }

  setLocalStorageBasket() {
    const basket = JSON.parse(localStorage.getItem("basket"));
    this.basketService.setBasket(basket);
    return basket;
  }
}
