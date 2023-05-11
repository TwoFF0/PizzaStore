import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { Product } from '../models/Product';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  baseUrl = environment.apiUrl;
  products: Product[];

  constructor(
    private httpClient: HttpClient,
    private cookieService: CookieService
  ) {}

  async getProducts(): Promise<Product[]> {
    this.products = (await this.httpClient
      .get<Product[]>(this.baseUrl + 'products')
      .toPromise()) as Product[];

    return this.products;
  }
}