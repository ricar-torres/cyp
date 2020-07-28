export interface User {
  applicationId?: number;
  applicationKey?: number;
  id?: number;
  username?: string;
  firstName?: string;
  lastName?: string;
  fullName?: string;
  token?: string;
  email?: string;
  password?: string;
  fileType?: string;
  ssno?: string;
  claims?: [];
  userType?: string;
  isChgPwd?: boolean;
}
