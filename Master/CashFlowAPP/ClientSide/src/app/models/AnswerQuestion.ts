export class AnswerQuestion {
        /**使用者問卷答案流水號 */
        Id:number=0;
        /**答案:[&quot;1個&quot;] */
        Answer: string|any;
        /**問卷流水號 ( 外鍵 ) */
        QusetionId: number|any;
        /**使用者流水號 ( 外鍵 ) */
        UserId: number|any;

    }
