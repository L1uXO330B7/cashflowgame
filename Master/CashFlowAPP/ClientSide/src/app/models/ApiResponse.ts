export class ApiResponse {
  /**執行成功與否 */
  Success: boolean = true;
  /**結果代碼 ( 0000 = 成功，其餘為錯誤代號 ) */
  Code: number | null = null;
  /**訊息 */
  Message: string | null = null;
  /**資料時間 */
  DataTime: Date | string = "";
  /**資料本體 */
  Data: any | null;
  /**資料本體數目 */
  TotalDataCount: number | null = null;
}
