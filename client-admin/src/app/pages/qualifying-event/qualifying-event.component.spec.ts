import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualifyingEventComponent } from './qualifying-event.component';

describe('QualifyingEventComponent', () => {
  let component: QualifyingEventComponent;
  let fixture: ComponentFixture<QualifyingEventComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualifyingEventComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualifyingEventComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
