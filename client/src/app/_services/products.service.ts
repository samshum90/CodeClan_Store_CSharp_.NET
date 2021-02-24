import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Product } from '../_models/product';
import { OrderedProducts } from '../_models/orderedProducts';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<Product[]>(this.baseUrl + "products");
  }

  getProduct(productname: string) {
    return this.http.get<Product>(this.baseUrl + "products/" + productname);
  }

  addProduct(orderedProduct: OrderedProducts) {
    return this.http.post(this.baseUrl + "orders", orderedProduct)
      .subscribe(
        (res) => {                           //Next callback
          console.log('response received')
          console.log(res);
        },
        (error) => {                              //Error callback
          console.error('error caught in component')
          console.error(error);
        }
      )
  }
}
