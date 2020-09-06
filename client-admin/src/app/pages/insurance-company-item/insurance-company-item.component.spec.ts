import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InsuranceCompanyItemComponent } from './insurance-company-item.component';

describe('InsuranceCompanyItemComponent', () => {
  let component: InsuranceCompanyItemComponent;
  let fixture: ComponentFixture<InsuranceCompanyItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InsuranceCompanyItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InsuranceCompanyItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
