import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BeneficiariesBenefitDistributionComponent } from './beneficiaries-benefit-distribution.component';

describe('BeneficiariesBenefitDistributionComponent', () => {
  let component: BeneficiariesBenefitDistributionComponent;
  let fixture: ComponentFixture<BeneficiariesBenefitDistributionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BeneficiariesBenefitDistributionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BeneficiariesBenefitDistributionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
