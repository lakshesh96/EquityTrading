import { Injectable } from '@angular/core';
import { Stocks } from "../../Models/stocks";
import { CurrentPosition } from "../../Models/current-position";
import { GlobalService } from "../../Services/global.service";
import { CurrentPositionModelView } from '../../Models/current-position-model-view';
import { Sell } from '../../Models/sell';

@Injectable()
export class BuySellService {

	/*private _baseUrl: string = "http://localhost:52705/api/Trader/Orders";
	private _baseUrl1: string = "http://localhost:52705/api/PM/Orders";*/

	private urlTrader: string = "api/Trader/Orders";
	private urlPM: string = "api/PM/Orders";

	constructor(private globalService:GlobalService) { }

	buyorder: Stocks;
	sellOrder;

	GetBuyOrder(o:Stocks){
		console.log("Stock Info received for Buy order", o);
		this.buyorder = o;
	}

	GetSellOrder(order: CurrentPositionModelView){
		this.sellOrder = order;
		console.log("Stock Info received for Sell order", this.sellOrder);
	}

	AddBuyOrSellOrder(r:any) {
		return this.globalService.PostMethod(r,this.urlTrader);
	}

	AddBuyPMOrder(r:any){
	return this.globalService.PostMethod(r,this.urlPM);
	}

}
