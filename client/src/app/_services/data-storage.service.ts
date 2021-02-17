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
}
