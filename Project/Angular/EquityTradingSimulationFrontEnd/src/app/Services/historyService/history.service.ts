import { Injectable } from '@angular/core';
import { GlobalService } from '../global.service';

@Injectable()
export class HistoryService {

  private url:string = "api/TransactionHistory"

  constructor(public globalService:GlobalService) { }


  getHistory(userId)
  {
    console.log("At history serrvice",userId);
    return this.globalService.GetWithId(this.url,userId);
  }

}
