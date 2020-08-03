import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BonaFideListComponent } from './bona-fide-list.component';

describe('BonaFideListComponent', () => {
  let component: BonaFideListComponent;
  let fixture: ComponentFixture<BonaFideListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BonaFideListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BonaFideListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
