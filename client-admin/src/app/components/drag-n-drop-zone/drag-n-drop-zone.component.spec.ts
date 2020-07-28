import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DragNDropZoneComponent } from './drag-n-drop-zone.component';

describe('DragNDropZoneComponent', () => {
  let component: DragNDropZoneComponent;
  let fixture: ComponentFixture<DragNDropZoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DragNDropZoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DragNDropZoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
