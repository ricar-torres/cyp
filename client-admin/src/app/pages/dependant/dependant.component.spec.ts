import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DependantComponent } from './dependant.component';

describe('DependantComponent', () => {
  let component: DependantComponent;
  let fixture: ComponentFixture<DependantComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DependantComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DependantComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
