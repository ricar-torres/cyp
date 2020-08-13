import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocumentationCallComponent } from './documentation-call.component';

describe('DocumentationCallComponent', () => {
  let component: DocumentationCallComponent;
  let fixture: ComponentFixture<DocumentationCallComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DocumentationCallComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentationCallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
