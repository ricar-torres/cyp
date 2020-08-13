import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DocsCallsList } from './docs-calls-list.component';

describe('DocsCallsListComponent', () => {
  let component: DocsCallsList;
  let fixture: ComponentFixture<DocsCallsList>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DocsCallsList],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DocsCallsList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
