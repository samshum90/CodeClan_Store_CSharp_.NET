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
        this.basketService.setBasket(res);
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
    const basket = localStorage.getItem("basket");
    this.basketService.setBasket(JSON.parse(basket));
    return JSON.parse(basket);
  }
}
