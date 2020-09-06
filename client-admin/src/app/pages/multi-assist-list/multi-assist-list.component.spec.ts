import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiAssistListComponent } from './multi-assist-list.component';

describe('MultiAssistListComponent', () => {
  let component: MultiAssistListComponent;
  let fixture: ComponentFixture<MultiAssistListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiAssistListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiAssistListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
