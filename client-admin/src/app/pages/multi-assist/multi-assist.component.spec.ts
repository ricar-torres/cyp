import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiAssistComponent } from './multi-assist.component';

describe('MultiAssistComponent', () => {
  let component: MultiAssistComponent;
  let fixture: ComponentFixture<MultiAssistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiAssistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiAssistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
