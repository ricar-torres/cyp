import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyCreateEditComponent } from './agency-create-edit.component';

describe('AgencyCreateEditComponent', () => {
  let component: AgencyCreateEditComponent;
  let fixture: ComponentFixture<AgencyCreateEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyCreateEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyCreateEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
