import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InsurancePlanItemComponent } from './insurance-plan-item.component';

describe('InsurancePlanItemComponent', () => {
  let component: InsurancePlanItemComponent;
  let fixture: ComponentFixture<InsurancePlanItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InsurancePlanItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InsurancePlanItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
