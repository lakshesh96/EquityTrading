import { Component, OnInit } from '@angular/core';
import { HistoryService } from '../../Services/historyService/history.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  userId = null;
  type = null;
  historyList:any[]=[];
  constructor(public historyService:HistoryService) {
   }

  ngOnInit() {
    this.userId = sessionStorage.getItem("UserId");
    this.type = sessionStorage.getItem("Type");
    if(this.userId&&this.type=="Trader")
        this.getHistory();
    else
        alert("Invalid Request")
  }

  getHistory() {

		this.historyService.getHistory(this.userId).subscribe(
			response => this.historyList = response,
			error => console.error(error),
			() => {
				console.log("Received", this.historyList.length, "entries", this.historyList);
				}
		);
	}
}
