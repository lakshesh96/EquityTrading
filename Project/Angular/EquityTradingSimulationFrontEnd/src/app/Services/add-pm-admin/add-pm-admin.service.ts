import { Injectable } from '@angular/core';
import {Http,Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map'; 
import 'rxjs/add/operator/catch'; 
import { GlobalService } from "../global.service";
import {User  } from "../../Models/user";

@Injectable()
export class AddPmAdminService {

  /*private _baseUrl: string = "http://localhost:52705/api/Users/PutList";
  private _baseUrl1: string = "http://localhost:52705/api/PM/Approved";
  private _baseUrl2: string = "http://localhost:52705/api/PM/Unapproved";
  private _baseUrl3: string = "http://localhost:52705/api/Users/Approve?id=";*/

  private Url:string = "api/Users/PutList";
  private Url1:string = "api/PM/Approved";
  private Url2:string = "api/PM/Unapproved";
  private Url3:string = "api/Users/Approve?id=";

  pms:any;

  constructor(private _http: Http,private globalService:GlobalService) {
  }

  getPMApproved()
  {
      /* this.globalService.GetMethod(this._baseUrl1).subscribe
          (response => this.pms = response,
          error => console.error(error),
          () => { console.info(this.pms)}
      );  */
      return this.globalService.GetMethod(this.Url1);
  }

  getPMUnapproved()
  {
      /* this.globalService.GetMethod(this._baseUrl2).subscribe
          (response => this.pms = response,
          error => console.error(error),
          () => { console.info(this.pms)}
      );  */
      return this.globalService.GetMethod(this.Url2);
    }
  

  AddPMs(r:any)
  {
    /* alert("Traders received at my service");
    console.log(r+"PMs");
    this.globalService.PostMethod(r,this._baseUrl).subscribe(
        response => response,
        error => console.error(error),
        //() => this.getPMs()
    );
    console.info(r); */

    return this.globalService.PostMethod(r,this.Url);
  }

  TogglePM(r:User){
    /* // alert("Traders received at my service toggle");
     this.globalService.PostMethod(r,this._baseUrl3+r.Id).subscribe(
         response => response,
         error => console.error(error),
         //() => this.getTraders()
     );
     console.info(r); */

     return this.globalService.PostMethod(r,this.Url3+r.Id);
   }


}
