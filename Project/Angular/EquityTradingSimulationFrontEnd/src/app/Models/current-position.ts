export class CurrentPosition {
    constructor(
         public Trader_Name :string,
         public Stock_Name:string,  
         public Symbol:string, 
         public Quantity:number,
         public Buying_Price: number,
         public Current_Price :number ,
         public Total_Value: number,
         public OrderType: string, 
         public OrderSide: string,
         public OrderStatus: string,
         public LimitPrice: number,
         public StopPrice: number,
         public UserId: number,
         public PMId: number,
         public StockId: number
         ){
}
}
