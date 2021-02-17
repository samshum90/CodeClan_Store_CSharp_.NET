import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { AngularSvgIconModule } from 'angular-svg-icon';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavComponent } from './nav/nav.component';
import { ProductListComponent } from './products/product-list/product-list.component';
import { ProductCardComponent } from './products/product-card/product-card.component';
import { ProductDetailComponent } from './products/product-detail/product-detail.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { SideBasketListComponent } from './side-basket/side-basket-list/side-basket-list.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    ProductListComponent,
    ProductCardComponent,
    ProductDetailComponent,
    LoginComponent,
    RegisterComponent,
    SideBasketListComponent,
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
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
  ],
  entryComponents: [ProductDetailComponent],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },],
  bootstrap: [AppComponent]
})
export class AppModule { }
