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
		console.log("Formatting: ", list);
		let formattedList: CurrentPositionModelView[] = [];
		
		formattedList.push(new CurrentPositionModelView(list[0]));

		// for (var i = 1; i < list.length; i++) {
		// 	formattedList.forEach(element => {
		// 		console.log("i:", i, "outside");
		// 		if (!element.AddCurrentPosition(list[i])) {
		// 			console.log("i:", i, "created new entry");
		// 			formattedList.push(new CurrentPositionModelView(list[i]));
		// 		} else {
		// 			console.log("Added index:", i);
		// 			break;
		// 		}
		// 	});
		// 	console.log("outside list"); 
		// }


		for (var i = 1; i < list.length; i++) {
			for (var j = 0; j < formattedList.length; j++) {
				console.log("i:", i, "outside");
				if (!formattedList[j].AddCurrentPosition(list[i])) {
					console.log("i:", i, "created new entry");
					formattedList.push(new CurrentPositionModelView(list[i]));
				} else {
					console.log("Added index:", i);
					break;
				}
			}
			console.log("outside list"); 
		}
		return formattedList;
	}
}
