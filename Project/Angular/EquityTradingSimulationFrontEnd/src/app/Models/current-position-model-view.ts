import { CurrentPosition } from './current-position';

export class CurrentPositionModelView {
	StockId: number;
	StockName: string;
	StockSymbol: string;
	TotalQuantity: number;
	ShowDetails: boolean = false;
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
		this.TotalQuantity = this.CalculateTotalFor("Quantity");
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