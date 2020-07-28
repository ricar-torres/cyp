import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SnackBarConfirmationComponent } from './snack-bar-confirmation.component';

describe('SnackBarConfirmationComponent', () => {
  let component: SnackBarConfirmationComponent;
  let fixture: ComponentFixture<SnackBarConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SnackBarConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SnackBarConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
