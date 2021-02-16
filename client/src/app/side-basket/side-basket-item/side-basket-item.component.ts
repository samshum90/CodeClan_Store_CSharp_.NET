import { Component, Input, OnInit } from '@angular/core';
import { OrderedProducts } from 'src/app/_models/orderedProducts';

@Component({
  selector: 'app-side-basket-item',
  templateUrl: './side-basket-item.component.html',
  styleUrls: ['./side-basket-item.component.scss']
})
export class SideBasketItemComponent implements OnInit {
  @Input() orderedProduct!: OrderedProducts;
  constructor() { }

  ngOnInit(): void {
  }

}
