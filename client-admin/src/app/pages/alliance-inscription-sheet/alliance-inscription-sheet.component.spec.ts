import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllianceInscriptionSheetComponent } from './alliance-inscription-sheet.component';

describe('AllianceInscriptionSheetComponent', () => {
  let component: AllianceInscriptionSheetComponent;
  let fixture: ComponentFixture<AllianceInscriptionSheetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllianceInscriptionSheetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllianceInscriptionSheetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
