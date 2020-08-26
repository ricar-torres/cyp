import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllianceWizardComponent } from './alliance-wizard.component';

describe('AllianceWizardComponent', () => {
  let component: AllianceWizardComponent;
  let fixture: ComponentFixture<AllianceWizardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllianceWizardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllianceWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
