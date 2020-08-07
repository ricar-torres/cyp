export class DocCall {
  callType: number;
  comments: string;
  clientId: number;
  userId: number;
  /**
   *
   */
  constructor(
    callreason: number,
    comment: string,
    clientid: number,
    userId: number
  ) {
    this.callType = callreason;
    this.comments = comment;
    this.clientId = clientid;
    this.userId = userId;
  }
}
