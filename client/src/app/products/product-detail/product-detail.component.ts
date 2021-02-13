import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Product } from 'src/app/_models/product';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  quantity: number = 1;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { product: Product }) { }

  ngOnInit(): void {
  }

  addQty(): void {
    this.quantity += 1
  }
  minusQty(): void {
    if (this.quantity > 0)
      this.quantity -= 1
  }

}
