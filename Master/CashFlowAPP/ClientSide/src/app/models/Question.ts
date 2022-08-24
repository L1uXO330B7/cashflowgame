
    export class Question {
        /**問卷流水號 */
        Id: number|any;
        /**問卷類型 1. 單選 2. 多選 3. 自由填文字 4. 數字 */
        Type: number|any;
        /**題目名稱 ,Ex.生幾個小孩 */
        Name: string|any;
        /**選項答案 :[&quot;1個,2個&quot;] */
        Answer: string[]|any;
        /**狀態 0. 停用 1. 啟用 2. 刪除 */
        Status: number|any;
    }

