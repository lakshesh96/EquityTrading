import { Injectable } from '@angular/core';
import { CurrentPosition } from "../../Models/current-position"
import { GlobalService } from '../../Services/global.service';
import { Stocks } from "../../Models/stocks";
import { CurrentPositionModelView } from '../../Models/current-position-model-view';

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
