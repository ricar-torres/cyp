import { title } from 'process';
export class DocCall {
  id: number;
  callTypeId: number;
  callType: string;
  comments: string;
  confirmationNumber: string;
  clientId: number;
  userId: number;
  createdDate: string;
  /**
   *
   */
  constructor(
    id?: number,
    calltypeId?: number,
    callType?: string,
    createddate?: string,
    comment?: string,
    confirmationNumber?: string,
    clientid?: number,
    userId?: number
  ) {
    this.id = id;
    this.callType = callType;
    this.callTypeId = calltypeId;
    this.comments = comment;
    this.createdDate = createddate;
    this.confirmationNumber = confirmationNumber;
    this.clientId = clientid;
    this.userId = userId;
  }
}
