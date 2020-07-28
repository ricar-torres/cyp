import { User } from './User';

export interface Task {
  id?: number;
  applicationId?: string;
  key?: string;
  details?: string;
  status?: string;
  taskUserId?: number;
  templateDocId?: number;
  documentTypeId?: number;
  documentId?: number;
  eSign?: boolean;
  eSignDate?: Date;
  comments?: string;
  adminComments?: string;
  documentTypeList?: number[];
  taskUser?: User;
  updateUser?: User;
  createdUser?: User;
  documentType?: any;
  document?: any;
  documentTypeName?: string;
  createDt?: Date;
  updDt?: Date;
  fCreateUserId?: number;
  fUpdUserId?: number;
  //templateFile?: FormData;
}
