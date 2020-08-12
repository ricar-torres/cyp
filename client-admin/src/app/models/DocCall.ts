import { title } from 'process';
export class DocCall {
  id: number;
  callType: string;
  callTypeName: string;
  comments: string;
  confirmationNumber: string;
  clientId: number;
  userId: number;
  createdAt: string;
  /**
   *
   */
  constructor(
    id: number,
    calltype?: string,
    callTypeName?: string,
    creadtedAt?: string,
    comment?: string,
    confirmationNumber?: string,
    clientid?: number,
    userId?: number
  ) {
    this.id = id;
    this.callTypeName = callTypeName;
    this.callType = calltype;
    this.comments = comment;
    this.createdAt = creadtedAt;
    this.confirmationNumber = confirmationNumber;
    this.clientId = clientid;
    this.userId = userId;
  }
}
