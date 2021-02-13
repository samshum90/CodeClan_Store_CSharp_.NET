import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { SideBasketComponent } from './side-basket/side-basket.component';
import { ProductListComponent } from './products/product-list/product-list.component';
import { ProductCardComponent } from './products/product-card/product-card.component';
import { ProductDetailComponent } from './products/product-detail/product-detail.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    SideBasketComponent,
    ProductListComponent,
    ProductCardComponent,
    ProductDetailComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    MatIconModule,
    AngularSvgIconModule.forRoot(),
    MatCardModule,
    MatDialogModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDividerModule,
  ],
  entryComponents: [ProductDetailComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
