import { Injectable } from '@angular/core';
import { Stocks } from "../../Models/stocks";
import { CurrentPosition } from "../../Models/current-position";
import { GlobalService } from "../../Services/global.service";

@Injectable()
export class BuySellService {

  /*private _baseUrl: string = "http://localhost:52705/api/Trader/Orders";
  private _baseUrl1: string = "http://localhost:52705/api/PM/Orders";*/

  private urlTrader: string = "api/Trader/Orders";
  private urlPM: string = "api/PM/Orders";

  constructor(private globalService:GlobalService) { }

  buyorder:Stocks;
  sellorder:CurrentPosition;

  GetBuyOrder(o:Stocks){
	  console.log("Stock Info received for Buy order", o);
    this.buyorder = o;
  }

  GetSellOrder(order:CurrentPosition){
    console.log("At sell service:");
		order.OrderType = "Market";
    order.OrderSide = "Sell";
    order.OrderStatus = "";
    order.LimitPrice = null;
    order.StopPrice = null;
    if(sessionStorage.getItem("Type") == "Trader"){
      order.PMId = null;
      order.UserId = parseInt( sessionStorage.getItem("UserId") );
      console.log(order);

      this.globalService.PostMethod(order,this.urlTrader).subscribe(
        response => console.log(response),
        error => console.error(error),
      () => console.log("Success")
      );
    }
    else{
      //order.PMId = null;
      console.log(order);

      this.globalService.PostMethod(order,this.urlPM).subscribe(
        response => console.log(response),
        error => console.error(error),
      () => console.log("Success")
      );
    }
  }

  AddBuyOrder(r:any)
  {
    /* alert("Traders received at my service");
    console.log(r+"traders");
    this.globalService.PostMethod(r,this._baseUrl).subscribe(
        response => response,
        error => console.error(error),
        //() => this.getTraders()
    );
    console.info(r); */

    return this.globalService.PostMethod(r,this.urlTrader);
  }

  AddBuyPMOrder(r:any){
    return this.globalService.PostMethod(r,this.urlPM);
  }

}
