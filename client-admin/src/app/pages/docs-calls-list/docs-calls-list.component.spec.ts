import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocsCallsListComponent } from './docs-calls-list.component';

describe('DocsCallsListComponent', () => {
  let component: DocsCallsListComponent;
  let fixture: ComponentFixture<DocsCallsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DocsCallsListComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocsCallsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
