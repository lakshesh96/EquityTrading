import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Stocks } from "../../Models/stocks";
import { Sellmodel } from "../../Models/sell";
import { Buy, OrderSide, OrderType } from '../../Models/buy';
import { BuySellService } from "../../Services/buy-sell/buy-sell.service";
@Component({
  selector: 'app-sell-request',
  templateUrl: './sell-request.component.html',
  styleUrls: ['./sell-request.component.css']
})
export class SellRequestComponent implements OnInit {
  	order;
	placedOrder:Sellmodel;
	sell: FormGroup;

	constructor(private BSservice:BuySellService) {
		this.order = this.BSservice.sellOrder;
		console.log("Sell Component Loaded. Sell order for", this.order);
	}

	ngOnInit() {
		this.sell = new FormGroup({
			StocksId: new FormControl('', [Validators.required]),
			Quantity: new FormControl('', [Validators.required]),
			StopPrice: new FormControl('', [Validators.required]),
			LimitPrice: new FormControl('', [Validators.required]),
			OrderType: new FormControl('', [Validators.required])
		}); 
	}

	onSubmit({ value, valid }: { value: Buy, valid: boolean }) {
		//console.log("Sell Order Submit", value);
		console.log(this.order);
		value.StocksId = this.order.StockId;
		if (value.Quantity > this.order.TotalQuantity) {
			alert("Quantity error");
			return;
		}

		value.OrderSide = OrderSide.Sell;
		if(value.OrderType.toString() == "Market"){
			console.log("Market");
			value.StopPrice=0;
			value.LimitPrice=0;
		}
		else if(value.OrderType.toString()=="Limit"){
			console.log("Limit");
			value.StopPrice=0;
		}
		else if(value.OrderType.toString() == "Stop"){
			console.log("Stop");
			value.LimitPrice=0;
		}
		value.BlockId = null;
		value.PMId = null;
		
		value.UserId = +sessionStorage.getItem('UserId');
		console.log("Sell order: ", value, valid);
		this.BSservice.AddBuyOrSellOrder(value).subscribe(
			response => response,
			error => console.error(error),
			() => alert("Sell Order placed.")
		);
	}

	LimitFlag:boolean =true;
	StopFlag:boolean =true;

	Toggle(value) {
		if (value == "Stop") {
			this.StopFlag = false;
			this.LimitFlag = true;
		} else if (value == "Limit") {
			this.LimitFlag = false;
			this.StopFlag = true;
		} else if (value == "StopLimit") {
			this.LimitFlag = false;
			this.StopFlag = false;
		} else if (value == "Market") {
			this.LimitFlag = true;
			this.StopFlag = true;
		}
	}
}
 