import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencySearchComponent } from './agency-search.component';

describe('AgencySearchComponent', () => {
  let component: AgencySearchComponent;
  let fixture: ComponentFixture<AgencySearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencySearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
