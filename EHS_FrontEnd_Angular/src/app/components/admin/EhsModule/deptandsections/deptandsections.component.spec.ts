import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptandsectionsComponent } from './deptandsections.component';

describe('DeptandsectionsComponent', () => {
  let component: DeptandsectionsComponent;
  let fixture: ComponentFixture<DeptandsectionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeptandsectionsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeptandsectionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
