import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { Order } from 'src/app/_models/order';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { BasketService } from 'src/app/_services/basket.service';
import { DataStorageService } from 'src/app/_services/data-storage.service';

@Component({
  selector: 'app-side-basket-list',
  templateUrl: './side-basket-list.component.html',
  styleUrls: ['./side-basket-list.component.scss']
})
export class SideBasketListComponent implements OnInit {
  basket!: Order;
  subscription!: Subscription;
  user!: User;
  displayedColumns: string[] = ['photo', 'name', 'quantity', 'price'];
  dataSource!: MatTableDataSource<any>;

  constructor(public basketService: BasketService, public dataStorageService: DataStorageService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadBasket();
  }

  loadBasket() {

    this.subscription = this.basketService.basket
      .subscribe(
        (basket: Order) => {
          this.basket = basket;
        }
      );
    if (!!this.user) {
      this.dataStorageService.fetchBasket().subscribe(products => {
        this.basket = products;
        this.dataSource = new MatTableDataSource(products.orderedProducts);
      })
    }
  }
}
