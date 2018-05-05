import { Component, OnInit } from '@angular/core';
import {BlockserviceService} from "../../Services/blockservice/blockservice.service";
import { OrderService } from '../../Services/Order/order.service';
import { ListService } from '../../Services/list-service/list.service';
import { StocksService } from '../../Services/StocksList/stocks.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-block-creation',
  templateUrl: './block-creation.component.html',
  styleUrls: ['./block-creation.component.css']
})
export class BlockCreationComponent implements OnInit {
  pendingBlocks=[];
  OrderId:number;
  constructor(private blockService:BlockserviceService,private router:Router) {
    
   }

  ngOnInit() {
    this.OrderId= sessionStorage["OrderId"];
    if(this.OrderId == null)
      alert("Order not selected please try again");
    else
      this.getPendingBlocks();
  }
 AddtoBlock(BlockId)
  {
    
    if(this.OrderId == null)
      alert("Order not selected please try again");
    else
    {
      this.blockService.AddToBlock(this.OrderId,BlockId);
      this.router.navigateByUrl("/Trader");
    }
  }
  getPendingBlocks(){
    this.blockService.get_pendingblock(this.OrderId).subscribe(
      response => this.pendingBlocks= response,
      error => console.error(error),
      () => {
        console.log(this.pendingBlocks);
        if(this.pendingBlocks.length==0)
          {
            alert("No Matching Block Found. Please create a new block");
            this.router.navigateByUrl("/Trader/PendingOrders");
          }
    }
    );
  }
 
}
