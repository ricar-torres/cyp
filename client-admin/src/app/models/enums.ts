export const MenuRoles = {
  USERS: ['User.Read', 'User.Create', 'User.Update', 'User.Delete'],
  USERS_CREATE: ['User.Create'],
  USERS_UPDATE: ['User.Update'],
  USERS_DELETE: ['User.Delete'],

  AGENCIES: ['Agency.Read', 'Agency.Create', 'Agency.Update', 'Agency.Delete'],
  AGENCIES_CREATE: ['Agency.Create'],
  AGENCIES_UPDATE: ['Agency.Update'],
  AGENCIES_DELETE: ['Agency.Delete'],

  CAMPAIGNS: [
    'Campaign.Read',
    'Campaign.Create',
    'Campaign.Update',
    'Campaign.Delete',
  ],
  CAMPAIGNS_CREATE: ['Campaign.Create'],
  CAMPAIGNS_UPDATE: ['Campaign.Update'],
  CAMPAIGNS_DELETE: ['Campaign.Delete'],
};

export interface PERMISSION {
  read: boolean;
  create: boolean;
  update: boolean;
  delete: boolean;
  upload: boolean;
}
