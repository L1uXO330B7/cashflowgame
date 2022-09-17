export class FiInfo {
  ConnectId: string | any;
  UserId: number | any;
  UserName: string | any;
  CurrentMoney: number | any=0;
  TotalIncomce: number | any=0;

  TotalEarnings: number | any=0;
  TotalExpense: number | any=0;

  /**最高現金 */
  TotoalNetProfit: number | any=0;
  /**最高負債 */
  Debt: number | any;
  /**最高收入 */
  Revenue: number | any;
  /**最高淨收入 */
  NetProfit: number | any;


  CashFlowIncome: any;
  CashFlowExpense: any;
  Asset: any;
  Liabilities: any;
  NowCardId: number | any;
  NowCardAsset: any | null;

  ValueInterest: any | null;
}
