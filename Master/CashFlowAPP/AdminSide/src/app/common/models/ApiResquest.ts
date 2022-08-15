export class ApiRequest<T> {
  /**參數本體 */
  Args: T | undefined;
  /**排序 */
  OrderBy: any | null;
  /**第幾頁 */
  PageIndex: number | null = null;
  /**一頁幾筆 */
  PageSize: number | null = null;
}
