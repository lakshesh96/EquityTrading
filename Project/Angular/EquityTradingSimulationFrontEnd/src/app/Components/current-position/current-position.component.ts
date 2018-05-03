import { Component, OnInit } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { CurrentPosition } from "../../Models/current-position"
import { CurrentPositionService } from "../../Services/current-position/current-position.service"
import { BuySellService  } from "../../Services/buy-sell/buy-sell.service";

@Component({
	selector: 'app-current-position',
	templateUrl: './current-position.component.html',
	styleUrls: ['./current-position.component.css']
})
export class CurrentPositionComponent implements OnInit {

	list: CurrentPosition[];
	otherList: any[];
	show: boolean = true;
	
	constructor(private currentPositionService: CurrentPositionService, private buysellservice: BuySellService) { }

	getCurrentPositions() {
		this.currentPositionService.FetchCurrentPositions().subscribe(
			response => this.list = response,
			error => console.error(error),
			() => {
				console.log("Received", this.list.length, "entries", this.list);
				if (this.list.length > 0) {
					this.otherList = this.currentPositionService.performViewFormatting(this.list);
					console.log(this.otherList);
				}
			}
		);
	}

	ngOnInit() { 
		this.getCurrentPositions();
	}

	Sell(order) {
		this.buysellservice.GetSellOrder(order);
	}
}
