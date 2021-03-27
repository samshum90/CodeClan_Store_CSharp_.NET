import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { take } from 'rxjs/operators';
import { Order } from '../_models/order';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { BasketService } from '../_services/basket.service';
import { DataStorageService } from '../_services/data-storage.service';

@Injectable({
  providedIn: 'root'
})
export class BasketResolver implements Resolve<Order> {
  user!: User;
  constructor(private dataStorageService: DataStorageService, private basketService: BasketService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (!!this.user) {
      const basket = this.basketService.getBasket();
      if (!basket) {
        return this.dataStorageService.fetchBasket();
      } else {
        return basket;
      }
    } else {
      const localBasket: Order = JSON.parse(localStorage.getItem("basket"));
      if (localBasket === null || localBasket.orderedProducts.length === 0) {
        return this.dataStorageService.setNewLocalStorageBasket();
      } else {
        return this.dataStorageService.setLocalStorageBasket();
      }
    }
  }
}
