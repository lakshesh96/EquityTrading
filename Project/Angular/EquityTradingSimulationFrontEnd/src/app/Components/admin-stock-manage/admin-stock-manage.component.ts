import { Component, OnInit} from '@angular/core';
import {Contains} from '../../Models/contains';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AddStockAdminService } from '../../Services/add-stock-admin/add-stock-admin.service';
import { ExceltojsonService } from "../../Services/exceltojson/exceltojson.service";
import { AdminstocksService } from "../../Services/adminstocks/adminstocks.service";

@Component({
  selector: 'app-admin-stock-manage',
  templateUrl: './admin-stock-manage.component.html',
  styleUrls: ['./admin-stock-manage.component.css']
})
export class AdminStockManageComponent implements OnInit{

  stock:FormGroup;
  list:Contains[];
  listnew:Contains[];
  //stocks: any[];
  constructor(public ser:AddStockAdminService,public stocksService:AdminstocksService){
    this.stock = new FormGroup({
      Name: new FormControl('', [Validators.required, Validators.minLength(2),Validators.pattern("[aA-zZ]*")]),
      Symbol: new FormControl('',[Validators.required, Validators.minLength(2),Validators.pattern("[aA-zZ]*")]),
      CurrentPrice: new FormControl('',[Validators.required,Validators.pattern("[0-9]*")]),
      VolumeAvailable: new FormControl('',[Validators.required,Validators.pattern("[0-9]*")])
    });
  
  }
  


  onAdd({ value, valid }: { value: Contains, valid: boolean }){
    if(valid){
      var p =new Contains(value.Symbol,value.Name,value.CurrentPrice,value.VolumeAvailable);
      this.ser.onAdd(p).subscribe(
        response => response,
        error => console.error(error),
        () => this.DemoRefreshTable()
      );
      this.list=this.ser.list;
    }
    else 
      alert("Invalid Input");
  }

  DemoRefreshTable(){
    this.stocksService.getStocks().subscribe
    (response => this.listnew = response,
    error => console.error(error),
    () => { console.info()}
  );
  }

  ngOnInit() {
    this.DemoRefreshTable();
    //() => { console.info(this.listnew)} 
  }


  //Section for excel to json
  result: any;
  private xlsxToJsonService: ExceltojsonService = new ExceltojsonService();

  handleFile(event) {
    let file = event.target.files[0];
    this.xlsxToJsonService.processFileToJson({}, file).subscribe(data => {
      this.result = data['sheets'].Sheet1;
      console.log(JSON.stringify(this.result));
      this.AddP(this.result);
      for(var i in this.result){
        if(this.result[i].Symbol!=null && this.result[i].Name!=null && this.result[i].CurrentPrice!=null&&this.result[i].VolumeAvailable!=null)
        {
          var p =new Contains(this.result[i].Symbol,this.result[i].Name,this.result[i].CurrentPrice,this.result[i].VolumeAvailable);
          this.ser.list.push(p);
          this.list=this.ser.list;
        }
      }
    })
    //console.log(this.result);
  }

  AddP(result)
  {
    this.stocksService.AddStocks(result).subscribe(
      response => response,
      error => console.error(error),
      () => this.DemoRefreshTable()
  );
  }

}