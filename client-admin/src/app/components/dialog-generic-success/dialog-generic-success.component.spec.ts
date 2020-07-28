import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogGenericSuccessComponent } from './dialog-generic-success.component';

describe('DialogGenericSuccessComponent', () => {
  let component: DialogGenericSuccessComponent;
  let fixture: ComponentFixture<DialogGenericSuccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogGenericSuccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogGenericSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
