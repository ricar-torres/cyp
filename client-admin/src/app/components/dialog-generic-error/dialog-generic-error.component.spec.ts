import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogGenericErrorComponent } from './dialog-generic-error.component';

describe('DialogGenericErrorComponent', () => {
  let component: DialogGenericErrorComponent;
  let fixture: ComponentFixture<DialogGenericErrorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogGenericErrorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogGenericErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
