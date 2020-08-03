import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationMethodComponent } from './communication-method.component';

describe('CommunicationMethodComponent', () => {
  let component: CommunicationMethodComponent;
  let fixture: ComponentFixture<CommunicationMethodComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunicationMethodComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunicationMethodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
