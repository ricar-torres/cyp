import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllianceInscriptionSheetFirtsMedicalComponent } from './alliance-inscription-sheet-firts-medical.component';

describe('AllianceInscriptionSheetFirtsMedicalComponent', () => {
  let component: AllianceInscriptionSheetFirtsMedicalComponent;
  let fixture: ComponentFixture<AllianceInscriptionSheetFirtsMedicalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllianceInscriptionSheetFirtsMedicalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllianceInscriptionSheetFirtsMedicalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
