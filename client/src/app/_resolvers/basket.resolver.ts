import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { Order } from '../_models/order';
import { BasketService } from '../_services/basket.service';
import { DataStorageService } from '../_services/data-storage.service';

@Injectable({
  providedIn: 'root'
})
export class BasketResolver implements Resolve<Order> {
  constructor(private dataStorageService: DataStorageService, private basketService: BasketService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const basket = this.basketService.getBasket();
    if (!basket) {
      return this.dataStorageService.fetchBasket();
    } else {
      return basket;
    }
  }
}
