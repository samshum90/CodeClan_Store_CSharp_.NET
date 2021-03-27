import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { OrderedProducts } from 'src/app/_models/orderedProducts';
import { Product } from 'src/app/_models/product';
import { BasketService } from 'src/app/_services/basket.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  quantity: number = 1;
  orderedProduct!: OrderedProducts;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { product: Product },
    private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addQty(): void {
    this.quantity += 1
  }
  minusQty(): void {
    if (this.quantity > 0)
      this.quantity -= 1
  }

  addProduct(): void {
    this.orderedProduct =
    {
      product: this.data.product,
      quantity: this.quantity
    }
    this.basketService.addProduct(this.orderedProduct)
  }
}
