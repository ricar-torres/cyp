import { faMailBulk } from '@fortawesome/free-solid-svg-icons';

export class MultiAssist {
  id: number;
  coverId: number;
  effectiveDate: Date;
  eligibleWaitingPeriodDate: Date;
  endDate: Date;
  cost: number;
  statusId: number;
  accountType: string;
  bankName: string;
  accountHolderName: string;
  routingNum: string;
  accountNum: string;
  expDate: Date;
  debDay: number;
  debRecurringType: string;
  beneficiaries: Array<Beneficiaries>;
  constructor(
    id: number,
    coverId: number,
    effectiveDate: Date,
    eligibleWaitingPeriodDate: Date,
    endDate: Date,
    cost: number,
    statusId: number,
    accountType: string,
    bankName: string,
    accountHolderName: string,
    routingNum: string,
    accountNum: string,
    expDate: Date,
    debDay: number,
    debRecurringType: string,
    beneficiaries: Array<Beneficiaries>
  ) {
    this.id = id;
    this.coverId = coverId;
    this.effectiveDate = effectiveDate;
    this.eligibleWaitingPeriodDate = eligibleWaitingPeriodDate;
    this.endDate = endDate;
    this.cost = cost;
    this.statusId = statusId;
    this.accountType = accountType;
    this.bankName = bankName;
    this.accountHolderName = accountHolderName;
    this.routingNum = routingNum;
    this.accountNum = accountNum;
    this.expDate = expDate;
    this.debDay = debDay;
    this.debRecurringType = debRecurringType;
    this.beneficiaries = beneficiaries;
  }
}

export class Beneficiaries {
  id: number;
  name: string;
  birthDate: Date;
  relationship: string;
  percent: string;
  ssn: string;
  multiAssistId: number;
  /**
   *
   */
  constructor(
    id: number,
    name: string,
    birthDate: Date,
    relationship: string,
    percent: string,
    ssn: string,
    multiAssistId: number
  ) {
    this.id = id;
    this.name = name;
    this.birthDate = birthDate;
    this.relationship = relationship;
    this.percent = percent;
    this.ssn = ssn;
    this.multiAssistId = multiAssistId;
  }
}

export class MultiAssistsVehicle {
  id: number;
  multiAssistId: number;
  make: string;
  model: string;
  year: number;
  vin: string;
  /**
   *
   */
  constructor(
    id: number,
    multiAssistId: number,
    make: string,
    model: string,
    year: number,
    vin: string
  ) {
    this.id = id;
    this.multiAssistId = multiAssistId;
    this.make = make;
    this.model = model;
    this.year = year;
    this.vin = vin;
  }
}
