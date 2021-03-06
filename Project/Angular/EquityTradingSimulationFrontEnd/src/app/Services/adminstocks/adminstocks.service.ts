import { Injectable } from '@angular/core';
import {Http,Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map'; 
import 'rxjs/add/operator/catch'; 
import { GlobalService } from "../global.service";

@Injectable()
export class AdminstocksService {

  /*private _baseUrl: string = "http://localhost:52705/api/Stocks/PutList";
  private _baseUrl1: string = "http://localhost:52705/api/Stocks";*/

  private Url: string = "api/Stocks/PutList";
  private Url1: string = "api/Stocks";

  stocks:any;

  constructor(private _http: Http,private globalService:GlobalService) {
  }

  getStocks()
  {
      this.globalService.GetMethod(this.Url1).subscribe
          (response => this.stocks = response,
          error => console.error(error),
          () => { console.info()}
          //() => { console.info(this.stocks)}
      ); 
      return this.globalService.GetMethod(this.Url1);
  }
  

  AddStocks(r:any)
  {
  /*   alert("Stocks received at my service");
    //console.log(r+"--");
    this.globalService.PostMethod(r,this._baseUrl).subscribe(
        response => response,
        error => console.error(error),
        () => this.getStocks()
    );
    //console.info("AddStocks Service wala: "+r); */
    return this.globalService.PostMethod(r,this.Url);
  }
}
