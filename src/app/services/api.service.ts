import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions, RequestMethod } from '@angular/http';

import { DatabaseLog } from '../models/databaselog.model'
import { Product } from '../models/product.model'

import { Observable } from "rxjs";



@Injectable()
export class APIService {

  constructor(
    private http: Http
  ) { };


  async GetAllDatabaseLogs(): Promise<DatabaseLog[]> {
    var url = location.origin + "/api/DatabaseLogs";
    return await this.http.get(url)
      .toPromise()
      .then(x => {
        return x.json();
      });
  
  }


  async GetSearchedProductIds(ids): Promise<Product[]> {
    var url = location.origin + "/api/Products/Search?ids=" + ids;
    return await this.http.get(url)
      .toPromise()
      .then(x => {
        return x.json();
      });
  }


}







