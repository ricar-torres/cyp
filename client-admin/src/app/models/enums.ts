export const MenuRoles = {
  USERS: ['User.Read', 'User.Create', 'User.Update', 'User.Delete'],
  USERS_CREATE: ['User.Create'],
  USERS_UPDATE: ['User.Update'],
  USERS_DELETE: ['User.Delete'],

  AGENCIES: ['Agency.Read', 'Agency.Create', 'Agency.Update', 'Agency.Delete'],
  AGENCIES_CREATE: ['Agency.Create'],
  AGENCIES_UPDATE: ['Agency.Update'],
  AGENCIES_DELETE: ['Agency.Delete'],


  INSURANCE_COMPANY: ['HealthPlans.Read', 'HealthPlans.Create', 'HealthPlans.Update', 'HealthPlans.Delete'],
  INSURANCE_COMPANY_CREATE: ['HealthPlans.Create'],
  INSURANCE_COMPANY_UPDATE: ['HealthPlans.Update'],
  INSURANCE_COMPANY_DELETE: ['HealthPlans.Delete'],

  BONAFIDE_UPDATE: ['Bonafide.Update'],
  BONAFIDE_CREATE: ['Bonafide.Create'],
  BONAFIDE_DELETE: ['Bonafide.Delete'],

  CLIENT_UPDATE: ['Bonafide.Update'],
  CLIENT_CREATE: ['Bonafide.Create'],
  CLIENT_DELETE: ['Bonafide.Delete'],

  CHAPTER_UPDATE: ['Chapter.Update'],
  CHAPTER_CREATE: ['Chapter.Create'],
  CHAPTER_DELETE: ['Chapter.Delete'],

  QUALIFYING_EVENT_UPDATE: ['QualifyingEvent.Update'],
  QUALIFYING_EVENT_CREATE: ['QualifyingEvent.Create'],
  QUALIFYING_EVENT_DELETE: ['QualifyingEvent.Delete'],

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
