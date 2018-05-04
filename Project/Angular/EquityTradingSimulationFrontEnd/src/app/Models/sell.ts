export class Sellmodel {
    constructor(
        public Id:number,
        public OrderType:string,
        public OrderSide:string,
        public Quantity:number,
        public StockID:number,
        public UserID:number,
        public OrderStatus:string,
        public LimitPrice:number,
        public StopPrice:number,
        public BlockID:number,
        public PMId:number
        ){}
}


export class Sell {
	public OrderType: number;
	public OrderSide: number;
	public StocksId: number;
	public Quantity: number;
	public UserId: number;
	public PMId: number;
	public OrderStatus: number;
	public LimitPrice: number;
	public StopPrice: number;
	public BlockId: number;
	public Positions
}