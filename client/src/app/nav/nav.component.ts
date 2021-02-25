import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { BasketService } from '../_services/basket.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router, public basketService: BasketService) {
  }

  ngOnInit(): void {

  }

  getTotalQty(): number {
    return this.basketService.getNumberOfItems();
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
}
