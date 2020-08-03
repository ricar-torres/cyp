import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualifyingEventListComponent } from './qualifying-event-list.component';

describe('QualifyingEventListComponent', () => {
  let component: QualifyingEventListComponent;
  let fixture: ComponentFixture<QualifyingEventListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualifyingEventListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualifyingEventListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
