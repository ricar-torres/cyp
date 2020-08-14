import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BonafidesAssociatorComponent } from './bonafides-associator.component';

describe('BonafidesAssociatorComponent', () => {
  let component: BonafidesAssociatorComponent;
  let fixture: ComponentFixture<BonafidesAssociatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BonafidesAssociatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BonafidesAssociatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
