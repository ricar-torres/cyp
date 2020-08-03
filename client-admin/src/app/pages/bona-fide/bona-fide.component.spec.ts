import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BonaFideComponent } from './bona-fide.component';

describe('BonaFideComponent', () => {
  let component: BonaFideComponent;
  let fixture: ComponentFixture<BonaFideComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BonaFideComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BonaFideComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
