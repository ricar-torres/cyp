import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommunicationMethodsListComponent } from './communication-methods-list.component';

describe('CommunicationMethodsListComponent', () => {
  let component: CommunicationMethodsListComponent;
  let fixture: ComponentFixture<CommunicationMethodsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommunicationMethodsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommunicationMethodsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
