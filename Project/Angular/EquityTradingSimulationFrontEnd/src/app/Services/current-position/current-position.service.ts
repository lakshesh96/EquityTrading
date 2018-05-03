import { Injectable } from '@angular/core';
import { CurrentPosition } from "../../Models/current-position"
import { GlobalService } from '../../Services/global.service';
import { Stocks } from "../../Models/stocks"

@Injectable()
export class CurrentPositionService {

	CurrentS : CurrentPosition[];

	//url = "http://localhost:52705/api/Position/Approved?userId=";
	url = "api/Position/Approved?userId=";

	constructor(private globalService: GlobalService) { }

	FetchCurrentPositions() {
		let userId = sessionStorage.getItem("UserId");
		console.log("Current Position for UserId:", userId, "| API:", this.url);
		return this.globalService.GetMethod(this.url + userId);
	}

	performViewFormatting(list: CurrentPosition[]) {
		let formattedList: CurrentPositionModelView[] = [];
		
		formattedList.push(new CurrentPositionModelView(list[0]));

		for (var i = 1; i < list.length; i++) {
			formattedList.forEach(element => {
				if (!element.AddCurrentPosition(list[i])) {
					formattedList.push(new CurrentPositionModelView(list[i]));
				}
			});
		}
		return formattedList;
	}
}


export class CurrentPositionModelView {
	StockId: number;
	StockName: string;
	StockSymbol: string;
	AvgQuantity: number;
	AvgBuyingPrice: number;
	CurrentPrice: number;
	TotalValue: number;
	CurrentPositionList: CurrentPosition[] = [];

	constructor(position: CurrentPosition) {
		this.StockId = position.StockId;
		this.Configure(position);
		//this.CurrentPositionList = [];
	}

	AddCurrentPosition(position: CurrentPosition) {
		if (this.StockId == position.StockId) {
			this.Configure(position);
			return true;
		} 
		return false;
	}

	Configure(position: CurrentPosition) {
		this.CurrentPositionList.push(position);
		this.StockName = position.Stock_Name;
		this.StockSymbol = position.Symbol;
		this.CurrentPrice = position.Current_Price;
		this.AvgQuantity = this.CalculateTotalFor("Quantity");
		this.AvgBuyingPrice = this.CalculateAverageFor("Buying_Price");
		this.TotalValue = this.CalculateTotalFor("Total_Value");
	}

	CalculateTotalFor(property: any) {
		let sum: number = 0;
		this.CurrentPositionList.forEach(element => {
			sum += element[property];
		});
		return sum;
	}

	CalculateAverageFor(property: any) {
		let sum: number = 0;
		this.CurrentPositionList.forEach(element => {
			sum += element[property];
		});
		return (sum/this.CurrentPositionList.length);
	}
}